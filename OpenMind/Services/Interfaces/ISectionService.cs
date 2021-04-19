using System.Threading.Tasks;
using OpenMind.Domain;

namespace OpenMind.Services
{
    public interface ISectionService
    {
        Task<ServiceActionResult> AddSection(string name, int sectionNumber, string locale);
        Task<ServiceActionResult> DeleteSection(int id);
        Task<ServiceActionResult> UpdateSection(int id, string name = null, int? sectionNumber=null, string locale=null);
        Task<ServiceActionResult> GetAll(string locale);
    }
}