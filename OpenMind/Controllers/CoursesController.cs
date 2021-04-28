using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts.Requests.Courses;
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
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseRequest request)
        {
            // TODO: Add only admin access
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
        public async Task<IActionResult> CreateCourseLesson([FromBody] CreateCourseLessonRequest request)
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
        
        [HttpGet("get")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get(string locale, int page, string query)
        {
            var result = await _coursesService.GetAsync(locale, page, query);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as ListResult<CourseThumbnailResult>).Data);
        }
        
        // DEPRECATED
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
        
        [HttpGet("get-info")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInfo(int id)
        {
            var result = await _coursesService.GetInfoAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as SingleObjectResult<CourseResult>).Data);
        }
        
        [HttpGet("get-lesson")]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCourseLessonsInfoPrivilege(int id, int lessonNumber)
        {
            var result = await _coursesService.GetLessonAsync(id, lessonNumber);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok((result as SingleObjectResult<CourseLessonResult>).Data);
        }
    }
}
