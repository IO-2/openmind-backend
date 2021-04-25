using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenMind.Domain;

namespace OpenMind.Services
{
    public interface IMediaService
    {
        Task<ServiceActionResult> CreateAsync(string name, string text, int type, IFormFile file, string url, int category);
        Task<ServiceActionResult> GetInfoAsync(int id);
        Task<ServiceActionResult> GetFileAsync(int id);
        Task<ServiceActionResult> DeleteAsync(int id);
        Task<ServiceActionResult> GetInfoAllAsync(string locale);
        Task<ServiceActionResult> GetInfoByCategory(int page, int category, string locale);
    }
}