using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Services;

namespace OpenMind.Controllers
{
    [ApiVersion("1.0")]
    public class ChecklistController : MyControllerBase
    {
        private readonly IChecklistService _checklistService;
        
        public ChecklistController(IChecklistService checklistService)
        {
            this._checklistService = checklistService;
        }
        
        [HttpGet("get_info")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInfo([FromQuery] ActionWithIdContract contract)
        {
            var result = await _checklistService.GetInfo(contract.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as ChecklistActionResult).Checklist);
        }
        
        [HttpGet("get_file")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFile([FromQuery] ActionWithIdContract contract)
        {
            var result = await _checklistService.GetFile(contract.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var fileResult = (result as FileActionResult);
            return PhysicalFile(fileResult.FilePath, fileResult.FileType, fileResult.FileName);
        }
        
        [HttpDelete("delete")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(ActionWithIdContract contract)
        {
            var result = await _checklistService.Delete(contract.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(true);
        }

        [HttpPost("create")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromForm] ChecklistCreateContract contract)
        {
            var file = contract.File.FirstOrDefault();
            var result = await _checklistService.Create(contract.Title, file, contract.Locale);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}