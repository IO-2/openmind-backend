namespace OpenMind.Models
{
    public class UserProgressBySectionModel
    {
        public int Id { get; set; }
        public int Section { get; set; }
        public int CompletedAmount { get; set; }
        
        public virtual UserModel User { get; set; }
    }
}