using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenMind.Contracts.Requests;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Services.Interfaces;

namespace OpenMind.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        
        public UsersController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }
        
        [HttpGet("get")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _identityService.GetInfo(email);

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
            var result = await _identityService.SetAvatarAsync(request.Files.FirstOrDefault(), email);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        
        [HttpGet("email-validation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> IsEmailValid(string email)
        {
            var result = await _identityService.IsEmailValid(email);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            
            return Ok(new ActionResponseContract
            {
                Success = true
            });
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
            var result = await _identityService.Register(request.Email, request.Name, request.Password, request.DreamingAbout, request.Inspirer, request.WhyInspired);

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

        [HttpPost("login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await _identityService.Login(request.Email, request.Password);

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
    }
}