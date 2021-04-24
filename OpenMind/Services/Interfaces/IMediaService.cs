using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenMind.Domain;

namespace OpenMind.Services
{
    public interface IMediaService
    {
        Task<ServiceActionResult> CreateAsync(string name, string text, int type, IFormFile file, string url);
        Task<ServiceActionResult> GetInfoAsync(int id);
        Task<ServiceActionResult> GetFileAsync(int id);
        Task<ServiceActionResult> DeleteAsync(int id);
        Task<ServiceActionResult> GetInfoAllAsync(int? page, string locale);
    }
}