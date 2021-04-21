namespace OpenMind.Domain
{
    public class AuthActionResult : ServiceActionResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}