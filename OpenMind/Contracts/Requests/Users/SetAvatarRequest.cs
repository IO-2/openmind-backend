using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OpenMind.Contracts.Requests.Users
{
    public class SetAvatarRequest
    {
        [Required]
        public IFormFileCollection File { get; set; }
    }
}