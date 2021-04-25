using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OpenMind.Models;

namespace OpenMind.Contracts.Requests.Media
{
    public class MediaCreateRequest : Localizable
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public IFormFileCollection File { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int Category { get; set; }
    }
}