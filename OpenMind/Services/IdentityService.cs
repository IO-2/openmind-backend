#nullable enable
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenMind.Contracts.Responses.Users;
using OpenMind.Data;
using OpenMind.Domain;
using OpenMind.Domain.Users;
using OpenMind.Models.Users;
using OpenMind.Options;
using OpenMind.Services.Interfaces;
using OpenMind.Services.Validators.Interfaces;
using StatusCodeResult = OpenMind.Domain.StatusCodeResult;

namespace OpenMind.Services
{
    public class IdentityService : FileWorkerService, IIdentityService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        private readonly IValidator _emailValidator;
        private readonly IValidator _passwordValidator;

        protected override IList<string> AllowedFiles { get; set; } = new List<string>{ "image/jpeg", "image/png", "image/jpg" };
        protected override string ContentsFolderName { get; set; } = "Avatars";

        public IdentityService(UserManager<UserModel> userManager, JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters, DataContext context, IWebHostEnvironment environment, IEmailValidator emailValidator, IPasswordValidator passwordValidator, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._jwtOptions = jwtOptions;
            this._tokenValidationParameters = tokenValidationParameters;
            this._context = context;
            this._environment = environment;
            this._emailValidator = emailValidator;
            this._passwordValidator = passwordValidator;
            _configuration = configuration;
        }

        public async Task<ServiceActionResult> RegisterAsync(string email, string name, string password, string dreamingAbout, string inspirer, string whyInspired, ICollection<int> interests)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new StatusCodeResult
                {
                    Success = false,
                    StatusCode = 456
                };
            }

            var newUser = new UserModel
            {
                FirstName = name,
                Email = email,
                UserName = email,
                DreamingAbout = dreamingAbout,
                Inspirer = inspirer,
                WhyInspired = whyInspired,
                Progresses = new List<UserProgressBySectionModel>
                {
                    new()
                    {
                        Section = 1,
                        CompletedAmount = 0
                    },new()
                    {
                        Section = 2,
                        CompletedAmount = 0
                    },
                    new()
                    {
                        Section = 3,
                        CompletedAmount = 0
                    },
                    new()
                    {
                        Section = 4,
                        CompletedAmount = 0
                    }
                }
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new StatusCodeResult
                {
                    StatusCode = 455
                };
            }

            newUser.Interests = interests.Select(x => new InterestModel
            {
                Interest = x
            }).ToList();
            await _userManager.UpdateAsync(newUser);

            return await GenerateAuthenticationResultForUser(newUser);
        }

        public async Task<ServiceActionResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new StatusCodeResult
                {
                    Success = false,
                    StatusCode = 459
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new StatusCodeResult
                {
                    Success = false,
                    StatusCode = 458
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

        public async Task<ServiceActionResult> DeleteAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthActionResult
                {
                    Errors = new[] {"User does not exists"}
                };
            }
            
            DeleteFile(user.AvatarUrl);

            _context.Users.Remove(await _context.Users.SingleOrDefaultAsync(x => x.Email == email));
            await _context.SaveChangesAsync();

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> GetInfoAsync(string email, string locale)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthActionResult
                {
                    Success = false,
                    Errors = new[] {"User does not exists"}
                };
            }

            // TODO: Create another method for this
            var courses = await _context.Courses.Where(x => x.Locale == locale).ToListAsync();

            var successes = new Dictionary<int, float>();

            for (int i = 1; i <= 4; i++)
            {
                // Check if user has progress on section created
                UserProgressBySectionModel? first = null;
                foreach (var x in user.Progresses)
                {
                    if (x.Section == i)
                    {
                        first = x;
                        break;
                    }
                }

                var lessonsBySection = courses
                    .Where(x => x.Section == i)
                    .SelectMany(x => x.Lessons).Count();
                // Check if no lessons in this section, set to zero
                if (lessonsBySection == 0)
                {
                    successes.Add(i, 0f);
                    continue;
                }
                if (first != null) successes.Add(i, (float)first.CompletedAmount / lessonsBySection);
            }
            

            return new UserInfoActionResult
            {
                Success = true,
                User = new UserInfoResponse
                {
                    Name = user.FirstName,
                    Email = user.Email,
                    DreamingAbout = user.DreamingAbout,
                    Inspirer = user.Inspirer,
                    SubscriptionEndDate = await IsSubscribed(email) ? user.SubscriptionEndDate : null,
                    WhyInspired = user.WhyInspired,
                    Interests = user.Interests.Select(x => x.Interest).ToList(),
                    Successes = successes,
                    AvatarUrl = user.AvatarUrl
                }
            };
        }

        public async Task<ServiceActionResult> SetAvatarAsync(IFormFile file, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new StatusCodeResult
                {
                    Success = false,
                    StatusCode = 459,
                    Errors = new[] {"User does not exists"}
                };
            }
            
            DeleteFile(user.AvatarUrl);
            
            try
            {
                var result = await SaveFile(_environment.WebRootPath, ContentsFolderName, file);

                user.AvatarUrl = result.SavedFilePath;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                
                return OkServiceActionResult();
            }
            catch (Exception e)
            {
                return BadServiceActionResult(e.Message);
            }
        }

        public async Task<ServiceActionResult> IsEmailValidAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            int validationCode = await _emailValidator.ValidateAsync(email);
            if (validationCode != 200)
            {
                return new StatusCodeResult
                {
                    Success = false,
                    StatusCode = validationCode
                };
            }

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> IsPasswordValidAsync(string password)
        {
            var result = await _passwordValidator.ValidateAsync(password);

            var success = result == 200;

            return new StatusCodeResult
            {
                Success = success,
                StatusCode = result
            };
        }

        public async Task<ServiceActionResult> GetAvatarAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new StatusCodeResult
                {
                    Success = false,
                    StatusCode = 459
                };
            }

            if (user.AvatarUrl.IsNullOrEmpty())
            {
                return new ServiceActionResult
                {
                    Errors = new[] {"No avatar"}
                };
            }
            
            return new FileActionResult
            {
                FileName = user.AvatarUrl.Substring(user.AvatarUrl.LastIndexOf("/") + 1),
                FilePath = user.AvatarUrl,
                FileType = "image/" + user.AvatarUrl.Substring(user.AvatarUrl.LastIndexOf(".") + 1),
                Success = true
            };
        }

        public async Task<ServiceActionResult> AddProgressAsync(string email, int sectionNumber, int progress)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new StatusCodeResult
                {
                    Success = false,
                    StatusCode = 459
                };
            }

            user.Progresses.FirstOrDefault(x => x.Section == sectionNumber)!.CompletedAmount += progress;
            await _context.SaveChangesAsync();

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> SendReceiptAsync(string receipt, string email, string locale)
        {
            var user = await _userManager.FindByEmailAsync(email);
            string resultString = string.Empty;

            try
            {
                long lastDate = await GetLastDate(receipt, email);
            }
            catch
            {
                return BadServiceActionResult("Invalid status code");
            }

            return await GetInfoAsync(email, locale);
        }

        private async Task<long> GetLastDate(string? receipt, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            string resultString = string.Empty;
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data = $"{{\"password\": \"{_configuration["AppstorePassword"]}\"," +
                              $" \"receipt-data\": \"{receipt ?? user.Receipt}\", \"exclude-old-transactions\": false}}";
                // TODO: Find a better way to JSON Serialization
                resultString = client.UploadString("https://buy.itunes.apple.com/verifyReceipt", data);
            }

            dynamic result = JsonConvert.DeserializeObject(resultString);
            if (result.status != 0)
            {
                throw new Exception("Invalid status code");
            }

            long lastDate = 0;
            try
            {
                lastDate = result.latest_receipt_info[0].expires_date_ms / 1000;
                foreach (dynamic purchase in result.receipt.in_app)
                {
                    if (purchase.product_id == "study.open.mind.forever")
                    {
                        lastDate = 0;
                    }
                }
                user.SubscriptionEndDate = lastDate;
            }
            catch
            {
                user.SubscriptionEndDate = 0;
            }
            user.Receipt = receipt ?? user.Receipt;
            await _context.SaveChangesAsync();

            return (long)user.SubscriptionEndDate;
        }

        public async Task<bool> IsSubscribed(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user.SubscriptionEndDate is null)
            {
                return false;
            }

            if (user.SubscriptionEndDate < DateTimeOffset.Now.ToUnixTimeSeconds() && user.SubscriptionEndDate != 0)
            {
                long lastDate = await GetLastDate(null, email);

                if (lastDate > DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return true;
                }

                return false;
            }
            return true;
        }

        public async Task<bool> IsAdminAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user.PrivilegeLevel >= 4;
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
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
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
