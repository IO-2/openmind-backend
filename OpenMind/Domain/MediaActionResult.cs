using OpenMind.Contracts.Responses;

namespace OpenMind.Domain
{
    public class MediaActionResult : ServiceActionResult
    {
        public MediaResponseContract Media { get; set; }
    }
}