using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class CreateCardsModel
    {
        [Required]
        public IEnumerable<CourseCardModel> Cards { get; set; }
    }
}