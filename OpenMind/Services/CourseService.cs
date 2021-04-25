using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMind.Contracts;
using OpenMind.Contracts.Requests;
using OpenMind.Domain;
using OpenMind.Services.Interfaces;

namespace OpenMind.Services
{
    public class CourseService : FileWorkerService, ICoursesService
    {
        public async Task<ServiceActionResult> CreateCourseAsync(CreateCourseRequest contract)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> CreateBenefitersAsync(IEnumerable<CourseBenefiterContract> benefiters)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> CreateCardsAsync(IEnumerable<CourseCardContract> cards)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> CreateCourseLessonAsync(CreateCourseLessonRequest contract)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> DeleteCourseAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> GetForSearchAsync(string local, int page, string query)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> GetCoursePictureAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> GetInfoAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> GetCourseLessonsInfoPrivilegeAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}