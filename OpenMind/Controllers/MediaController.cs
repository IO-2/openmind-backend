using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts;
using OpenMind.Contracts.Requests;
using OpenMind.Contracts.Requests.Media;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Domain.Media;
using OpenMind.Services;
using OpenMind.Services.Interfaces;

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
            var result = await _mediaService.GetInfoAsync(request.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as MediaActionResult).Media);
        }
        
        [HttpGet("get-info-all")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInfoAll(string locale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }
            
            var result = await _mediaService.GetInfoAllAsync(locale);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as ListResponse<BriefMediaResult>).Data);
        }
        
        [HttpGet("get-info-by-category")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInfoAll(int page, string locale, int category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }
            
            var result = await _mediaService.GetInfoByCategory(page, category, locale);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as ListResponse<BriefMediaResult>).Data);
        }
        
        [HttpGet("get-file")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFile([FromQuery] ActionWithIdRequest request)
        {
            var result = await _mediaService.GetFileAsync(request.Id);
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
            var result = await _mediaService.DeleteAsync(request.Id);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }
            
            var file = request.File.FirstOrDefault();
            var result = await _mediaService.CreateAsync(request.Title, request.Text, request.Type, file, request.Locale, request.Category);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new ActionResponseContract{ Success = true });
        }
    }
}