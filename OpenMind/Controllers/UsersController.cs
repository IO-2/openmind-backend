using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts.Requests;
using OpenMind.Contracts.Requests.Users;
using OpenMind.Contracts.Responses.Users;
using OpenMind.Domain;
using OpenMind.Domain.Users;
using OpenMind.Services.Interfaces;
using StatusCodeResult = OpenMind.Domain.StatusCodeResult;

namespace OpenMind.Controllers
{
    public class UsersController : MyControllerBase
    {
        private readonly IIdentityService _identityService;
        
        public UsersController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }
        
        [HttpGet("get")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get([Required] string locale)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _identityService.GetInfoAsync(email, locale);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok((result as UserInfoActionResult).User);
        }
        
        [HttpGet("get-avatar")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAvatar()
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _identityService.GetAvatarAsync(email);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var fileResult = result as FileActionResult;
            return PhysicalFile(fileResult.FilePath, fileResult.FileType, fileResult.FileName);
        }
        
        [HttpPost("upload-avatar")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadAvatar([FromForm] SetAvatarRequest request)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _identityService.SetAvatarAsync(request.File.FirstOrDefault(), email);

            if (!result.Success)
            {
                if (result is StatusCodeResult)
                {
                    return StatusCode((result as StatusCodeResult).StatusCode);
                }
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        
        [HttpDelete("delete")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete()
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _identityService.DeleteAsync(email);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok(result);
        }
        
        [HttpPost("register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var emailVerificationResult = await _identityService.IsEmailValidAsync(request.Email);
            if (!emailVerificationResult.Success)
            {
                return StatusCode((emailVerificationResult as StatusCodeResult).StatusCode);
            }

            var passwordValidationResult = await _identityService.IsPasswordValidAsync(request.Password);
            if (!passwordValidationResult.Success)
            {
                return StatusCode((passwordValidationResult as StatusCodeResult).StatusCode);
            }
            
            var result = await _identityService.RegisterAsync(request.Email, request.Name, request.Password, request.DreamingAbout, request.Inspirer, request.WhyInspired, request.Interests);

            if (!result.Success)
            {
                if (result is StatusCodeResult)
                {
                    return StatusCode((result as StatusCodeResult).StatusCode);
                }
                return BadRequest(result.Errors);
            }

            var tokenResult = result as AuthActionResult;
            return Ok(new AuthSuccessResponse
            {
                Token = tokenResult.Token,
                RefreshToken = tokenResult.RefreshToken
            });
        }
        
        [HttpPost("refresh")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var result = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var tokenResult = result as AuthActionResult;
            return Ok(new AuthSuccessResponse
            {
                Token = tokenResult.Token,
                RefreshToken = tokenResult.RefreshToken
            });
        }
        
        [HttpPost("send-receipt")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SendReceipt([FromBody] VerifyReceiptRequest request)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _identityService.SendReceiptAsync(request.Receipt, email, request.Locale);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as UserInfoActionResult).User);
        }

        [HttpPost("login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var emailVerificationResult = await _identityService.IsEmailValidAsync(request.Email);
            if (!emailVerificationResult.Success)
            {
                return StatusCode((emailVerificationResult as StatusCodeResult).StatusCode);
            }
            
            var result = await _identityService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                if (result is StatusCodeResult)
                {
                    return StatusCode((result as StatusCodeResult).StatusCode);
                }
                return BadRequest(result.Errors);
            }

            var tokenResult = result as AuthActionResult;
            return Ok(new AuthSuccessResponse
            {
                Token = tokenResult.Token,
                RefreshToken = tokenResult.RefreshToken
            });
        }
        
        [HttpPost("add-progress")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddProgress([FromBody] AddProgressRequest request)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _identityService
                .AddProgressAsync(email, Int32.Parse(request.SectionNumber), Int32.Parse(request.Progress));

            if (!result.Success)
            {
                if (result is StatusCodeResult)
                {
                    return StatusCode((result as StatusCodeResult).StatusCode);
                }
                return BadRequest(result.Errors);
            }

            var userResult = await _identityService.GetInfoAsync(email, request.Locale);

            return Ok((userResult as UserInfoActionResult).User);
        }
    }
}