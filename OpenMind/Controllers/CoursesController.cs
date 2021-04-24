using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts.Requests;
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
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            throw new NotImplementedException();
        } 
    }
}