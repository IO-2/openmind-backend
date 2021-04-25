using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenMind.Contracts.Requests.Courses
{
    public class CreateCardsRequest
    {
        [Required]
        public IEnumerable<CourseCardContract> Cards { get; set; }
    }
}