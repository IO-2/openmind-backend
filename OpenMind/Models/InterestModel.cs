namespace OpenMind.Models
{
    public class InterestModel
    {
        public int Id { get; set; }
        public virtual UserModel User { get; set; }
        public int Interest { get; set; }
    }
}