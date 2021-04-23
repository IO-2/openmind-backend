using System.ComponentModel.DataAnnotations;

namespace OpenMind.Contracts.Requests
{
    public class ActionWithIdRequest
    {
        [Required]
        public int Id { get; set; }
    }
}