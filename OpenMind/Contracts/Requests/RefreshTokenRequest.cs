using System.ComponentModel.DataAnnotations;

namespace OpenMind.Contracts.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}