using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenMind.Contracts.Requests.Courses
{
    public class CreateBenefiterRequest
    {
        [Required]
        public IEnumerable<CourseBenefiterContract> Benefiters { get; set; }
    }
}