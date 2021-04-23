using OpenMind.Contracts.Responses;

namespace OpenMind.Models
{
    public class InterestModel
    {
        public int Id { get; set; }
        public int Interest { get; set; }
        public virtual UserModel User { get; set; }
    }
}