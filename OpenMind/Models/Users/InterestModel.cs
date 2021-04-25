namespace OpenMind.Models.Users
{
    public class InterestModel
    {
        public int Id { get; set; }
        public int Interest { get; set; }
        public virtual UserModel User { get; set; }
    }
}