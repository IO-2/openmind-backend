using System.Threading.Tasks;
using OpenMind.Domain;

namespace OpenMind.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<ServiceActionResult> Register(string email, string password, string dreamingAbout, string inspirer, string whyInspired);

        Task<ServiceActionResult> Login(string email, string password);
        Task<ServiceActionResult> RefreshTokenAsync(string token, string refreshToken);

    }
}