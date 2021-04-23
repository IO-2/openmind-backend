using System.ComponentModel.DataAnnotations;

namespace OpenMind.Models
{
    public class Localizable
    {
        [Required]
        public string Locale { get; set; }
    }
}