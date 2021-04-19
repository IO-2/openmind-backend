using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Services;

namespace OpenMind.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SectionController : MyControllerBase
    {
        private readonly ISectionService _sectionService;
        
        public SectionController(ISectionService sectionService)
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
        public async Task<IActionResult> Create(SectionCreateContract contract)
        {
            var result = await _sectionService.AddSection(contract.Name, contract.SectionNumber, contract.Locale);
            // TODO: Add validation when item exists
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok("Done!");
        }
        
        [HttpDelete("delete")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(ActionWithIdContract contract)
        {
            var result = await _sectionService.DeleteSection(contract.Id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok("Done!");
        }
    }
}