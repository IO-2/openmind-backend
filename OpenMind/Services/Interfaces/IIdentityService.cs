using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenMind.Domain;

namespace OpenMind.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<ServiceActionResult> Register(string email, string name, string password, string dreamingAbout, string inspirer, string whyInspired);
        Task<ServiceActionResult> Login(string email, string password);
        Task<ServiceActionResult> RefreshTokenAsync(string token, string refreshToken);
        Task<ServiceActionResult> DeleteAsync(string token);
        Task<ServiceActionResult> GetInfo(string token);
        Task<ServiceActionResult> SetAvatarAsync(IFormFile avatar, string email);
        Task<ServiceActionResult> IsEmailValid(string email);
        Task<ServiceActionResult> GetAvatarAsync(string email);

    }
}