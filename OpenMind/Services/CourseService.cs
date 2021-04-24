using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMind.Contracts.Requests;
using OpenMind.Domain;
using OpenMind.Services.Interfaces;

namespace OpenMind.Services
{
    public class CourseService : FileWorkerService, ICoursesService
    {
        public Task<ServiceActionResult> CreateCourseAsync(CreateCourseRequest contract)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> CreateBenefitersAsync(IEnumerable<CreateBenefiterRequest> benefiters)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> CreateCardsAsync(IEnumerable<CreateCardsModel> cards)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> CreateCourseLessonAsync(CreateCourseLessonRequest contract)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> DeleteCourseAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> GetForSearchAsync(string local, int page, string query)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> GetCoursePictureAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> GetInfoAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceActionResult> GetCourseLessonsInfoPrivilegeAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}