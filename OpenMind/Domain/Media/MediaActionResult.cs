using OpenMind.Contracts.Responses.Media;

namespace OpenMind.Domain.Media
{
    public class MediaActionResult : ServiceActionResult
    {
        public MediaResponseContract Media { get; set; }
    }
}