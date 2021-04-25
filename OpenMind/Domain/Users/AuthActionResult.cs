namespace OpenMind.Domain.Users
{
    public class AuthActionResult : ServiceActionResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}