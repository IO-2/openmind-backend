namespace OpenMind.Models
{
    public class UserInterestModel
    {
        public int Id { get; set; }
        public virtual UserModel User { get; set; }
        public virtual InterestsModel Interest { get; set; }
    }
}