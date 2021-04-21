using OpenMind.Contracts.Responses;

namespace OpenMind.Domain
{
    public class UserInfoActionResult : ServiceActionResult
    {
        public UserInfoResponse User { get; set; }
    }
}