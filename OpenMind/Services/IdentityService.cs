using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenMind.Data;
using OpenMind.Domain;
using OpenMind.Models;
using OpenMind.Options;
using OpenMind.Services.Interfaces;

namespace OpenMind.Services
{
    public class IdentityService : Service, IIdentityService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;

        public IdentityService(UserManager<UserModel> userManager, JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters, DataContext context)
        {
            this._userManager = userManager;
            this._jwtOptions = jwtOptions;
            this._tokenValidationParameters = tokenValidationParameters;
            this._context = context;
        }

        public async Task<ServiceActionResult> Register(string email, string password, string dreamingAbout, string inspirer, string whyInspired)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthActionResult
                {
                    Errors = new []{"User with this email address already exists"}
                };
            }

            var newUser = new UserModel
            {
                Email = email,
                UserName = email,
                DreamingAbout = dreamingAbout,
                Inspirer = inspirer,
                WhyInspired = whyInspired
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new AuthActionResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return await GenerateAuthenticationResultForUser(newUser);
        }

        public async Task<ServiceActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthActionResult
                {
                    Errors = new[] {"User does not exists"}
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthActionResult
                {
                    Errors = new[] {"User/password combination is wrong"}
                };
            }
            
            return await GenerateAuthenticationResultForUser(user);
        }

        public async Task<ServiceActionResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return BadServiceActionResult("Invalid token");
            }

            var expiryDateunix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateunix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return BadServiceActionResult("This token hasn't expired yet");
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return BadServiceActionResult("Refresh token does not exists");
            }
            
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return BadServiceActionResult("This refresh token is expired");
            }

            if (storedRefreshToken.Invalidated)
            {
                return BadServiceActionResult("This refresh token has been invalidated");
            }

            if (storedRefreshToken.Used)
            {
                return BadServiceActionResult("This refresh token has been used");
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return BadServiceActionResult("This refresh token does not match this JWT");
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthenticationResultForUser(user);
        }

        private async Task<AuthActionResult> GenerateAuthenticationResultForUser(UserModel newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id)
                }),
                Expires = DateTime.UtcNow.Add(_jwtOptions.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Remove all used tokens
            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.User == newUser));

            var refreshToken = new RefreshTokenModel
            {
                JwtId = token.Id,
                UserId = newUser.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(2)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthActionResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        } 
    }
}