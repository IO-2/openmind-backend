using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenMind.Domain;

namespace OpenMind.Services
{
    public interface IChecklistService
    {
        Task<ServiceActionResult> Create(string name, IFormFile file, string url);
        Task<ServiceActionResult> GetInfo(int id);
        Task<ServiceActionResult> GetFile(int id);
        Task<ServiceActionResult> Delete(int id);
    }
}