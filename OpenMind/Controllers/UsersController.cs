using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenMind.Contracts.Requests;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Services;
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


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Hello, world!");
        }
        
        [HttpPost("register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await _identityService.Register(request.Email, request.Password, request.DreamingAbout, request.Inspirer, request.WhyInspired);

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