using System;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts.Requests;
using OpenMind.Contracts.Requests.Courses;
using OpenMind.Contracts.Responses;
using OpenMind.Domain;
using OpenMind.Domain.Courses;
using OpenMind.Services.Interfaces;

namespace OpenMind.Controllers
{
    [ApiVersion("1.0")]
    public class CoursesController : MyControllerBase
    {
        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        [HttpPost("create-course")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            var result = await _coursesService.CreateCourseAsync(request);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result as IdResult);
        }
        
        [HttpPost("create-benefiters")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateBenefiters([FromBody] CreateBenefiterRequest request)
        {
            var result = await _coursesService.CreateBenefitersAsync(request.Benefiters);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        
        [HttpPost("create-cards")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCards([FromBody] CreateCardsRequest request)
        {
            var result = await _coursesService.CreateCardsAsync(request.Cards);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        
        [HttpPost("create-course-lesson")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseLessonRequest request)
        {
            var result = await _coursesService.CreateCourseLessonAsync(request);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        
        [HttpDelete("delete-course")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _coursesService.DeleteCourseAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        
        [HttpGet("get-for-search")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetForSearch(string locale, int page, string query)
        {
            var result = await _coursesService.GetForSearchAsync(locale, page, query);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as ListResponse<BriefCourseResult>).Data);
        }
        
        [HttpGet("get-course-picture")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCoursePicture(int id)
        {
            var result = await _coursesService.GetCoursePictureAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            var fileResult = result as FileActionResult;
            return PhysicalFile(fileResult.FilePath, fileResult.FileType, fileResult.FileName);
        }
    }
}