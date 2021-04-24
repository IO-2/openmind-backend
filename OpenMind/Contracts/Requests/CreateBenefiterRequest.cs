using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class CreateBenefiterRequest
    {
        [Required]
        public IEnumerable<CourseBenefiterContract> Benefiters { get; set; }
    }
}