using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts;
using OpenMind.Contracts.Requests;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Services;

namespace OpenMind.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SectionsController : MyControllerBase
    {
        private readonly ISectionService _sectionService;
        
        public SectionsController(ISectionService sectionService)
        {
            this._sectionService = sectionService;
        }

        [HttpGet("all")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(string locale)
        {
            var result = await _sectionService.GetAll(locale);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok((result as SectionsActionResult).Sections);
        }
        
        [HttpPost("create")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create(SectionCreateRequest request)
        {
            var result = await _sectionService.AddSection(request.Name, request.SectionNumber, request.Locale);
            // TODO: Add validation when item exists
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok(new ActionResponseContract{Success = true});
        }
        
        [HttpDelete("delete")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(ActionWithIdRequest request)
        {
            var result = await _sectionService.DeleteSection(request.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok(new ActionResponseContract{Success = true});
        }
    }
}