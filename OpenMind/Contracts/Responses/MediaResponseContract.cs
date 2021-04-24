using OpenMind.Models;

namespace OpenMind.Contracts.Responses
{
    public class MediaResponseContract : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
    }
}