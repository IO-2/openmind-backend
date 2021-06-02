using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenMind.Domain;

namespace OpenMind.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<ServiceActionResult> RegisterAsync(string email, string name, string password, string dreamingAbout, string inspirer, string whyInspired, ICollection<int> interests);
        Task<ServiceActionResult> LoginAsync(string email, string password);
        Task<ServiceActionResult> RefreshTokenAsync(string token, string refreshToken);
        Task<ServiceActionResult> DeleteAsync(string token);
        Task<ServiceActionResult> GetInfoAsync(string email, string locale);
        Task<ServiceActionResult> SetAvatarAsync(IFormFile avatar, string email);
        Task<ServiceActionResult> IsEmailValidAsync(string email);
        Task<ServiceActionResult> IsPasswordValidAsync(string password);
        Task<ServiceActionResult> GetAvatarAsync(string email);
        Task<ServiceActionResult> AddProgressAsync(string email, int sectionNumber, int progress);
        Task<ServiceActionResult> SendReceiptAsync(string receipt, string email);
        Task<bool> IsSubscribed(string email);

        Task<bool> IsAdminAsync(string email);
    }
}