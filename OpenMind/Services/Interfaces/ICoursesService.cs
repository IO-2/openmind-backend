using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMind.Contracts.Requests;
using OpenMind.Domain;

namespace OpenMind.Services.Interfaces
{
    public interface ICoursesService
    {
        Task<ServiceActionResult> CreateCourseAsync(CreateCourseRequest contract);
        Task<ServiceActionResult> CreateBenefitersAsync(IEnumerable<CreateBenefiterRequest> benefiters);
        Task<ServiceActionResult> CreateCardsAsync(IEnumerable<CreateCardsModel> cards);
        Task<ServiceActionResult> CreateCourseLessonAsync(CreateCourseLessonRequest contract);
        Task<ServiceActionResult> DeleteCourseAsync(int id);
        Task<ServiceActionResult> GetForSearchAsync(string local, int page, string query);
        Task<ServiceActionResult> GetCoursePictureAsync(int id);
        Task<ServiceActionResult> GetInfoAsync(int id);
        Task<ServiceActionResult> GetCourseLessonsInfoPrivilegeAsync(int id);
    }
}