namespace OpenMind.Contracts.Responses
{
    public class BadRequestWithMessage
    {
        public bool Success { get; set; }
        public string Reason { get; set; }
    }
}