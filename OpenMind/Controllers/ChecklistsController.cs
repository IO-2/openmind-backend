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
    public class ChecklistsController : MyControllerBase
    {
        private readonly IChecklistService _checklistService;
        
        public ChecklistsController(IChecklistService checklistService)
        {
            this._checklistService = checklistService;
        }
        
        [HttpGet("get-info")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInfo([FromQuery] ActionWithIdRequest request)
        {
            var result = await _checklistService.GetInfo(request.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as ChecklistActionResult).Checklist);
        }
        
        [HttpGet("get-file")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFile([FromQuery] ActionWithIdRequest request)
        {
            var result = await _checklistService.GetFile(request.Id);
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
            var result = await _checklistService.Delete(request.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new ActionResponseContract{Success = true});
        }

        [HttpPost("create")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromForm] ChecklistCreateRequest request)
        {
            var file = request.File.FirstOrDefault();
            var result = await _checklistService.Create(request.Title, file, request.Locale);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new ActionResponseContract{ Success = true });
        }
    }
}