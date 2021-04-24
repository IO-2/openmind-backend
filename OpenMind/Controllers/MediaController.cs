using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts;
using OpenMind.Contracts.Requests;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Services;

namespace OpenMind.Controllers
{
    [ApiVersion("1.0")]
    public class MediaController : MyControllerBase
    {
        private readonly IMediaService _mediaService;
        
        public MediaController(IMediaService mediaService)
        {
            this._mediaService = mediaService;
        }
        
        [HttpGet("get-info")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInfo([FromQuery] ActionWithIdRequest request)
        {
            var result = await _mediaService.GetInfo(request.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as MediaActionResult).Media);
        }
        
        [HttpGet("get-info-all")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInfoAll(string locale, int page)
        {
            var result = await _mediaService.GetInfoAll(page, locale);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as AllMediaActionResult).Medias);
        }
        
        [HttpGet("get-file")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFile([FromQuery] ActionWithIdRequest request)
        {
            var result = await _mediaService.GetFile(request.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var fileResult = (result as FileActionResult);
            return PhysicalFile(fileResult.FilePath, fileResult.FileType, fileResult.FileName);
        }
        
        [HttpDelete("delete")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(ActionWithIdRequest request)
        {
            var result = await _mediaService.Delete(request.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new ActionResponseContract{Success = true});
        }

        [HttpPost("create")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromForm] MediaCreateRequest request)
        {
            var file = request.File.FirstOrDefault();
            var result = await _mediaService.Create(request.Title, request.Text, request.Type, file, request.Locale);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new ActionResponseContract{ Success = true });
        }
    }
}