using System.ComponentModel.DataAnnotations;

namespace OpenMind.Contracts.Requests.Users
{
    public class UserLoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}