using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class CreateCardsRequest
    {
        [Required]
        public IEnumerable<CourseCardContract> Cards { get; set; }
    }
}