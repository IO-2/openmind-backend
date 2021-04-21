using Microsoft.AspNetCore.Http;

namespace OpenMind.Contracts.Requests
{
    public class SetAvatarRequest
    {
        public IFormFileCollection Files { get; set; }
    }
}