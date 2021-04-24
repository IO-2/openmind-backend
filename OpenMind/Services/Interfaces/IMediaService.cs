using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenMind.Domain;

namespace OpenMind.Services
{
    public interface IMediaService
    {
        Task<ServiceActionResult> Create(string name, string text, int type, IFormFile file, string url);
        Task<ServiceActionResult> GetInfo(int id);
        Task<ServiceActionResult> GetFile(int id);
        Task<ServiceActionResult> Delete(int id);
        Task<ServiceActionResult> GetInfoAll(int? page, string locale);
    }
}