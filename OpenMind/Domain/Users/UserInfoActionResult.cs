using OpenMind.Contracts.Responses;
using OpenMind.Contracts.Responses.Users;

namespace OpenMind.Domain.Users
{
    public class UserInfoActionResult : ServiceActionResult
    {
        public UserInfoResponse User { get; set; }
    }
}