namespace OpenMind.Contracts.Requests
{
    public class UserRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        
        public string DreamingAbout { get; set; }
        public string Inspirer { get; set; }
        public string WhyInspired { get; set; }
    }
}