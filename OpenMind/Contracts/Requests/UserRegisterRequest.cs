using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenMind.Contracts.Requests
{
    public class UserRegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string DreamingAbout { get; set; }
        [Required]
        public string Inspirer { get; set; }
        [Required]
        public string WhyInspired { get; set; }
        [Required]
        public ICollection<int> Interests { get; set; }
    }
}