using OpenMind.Models;

namespace OpenMind.Contracts.Responses.Media
{
    public class MediaResponseContract : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public long UploadedTime { get; set; }
        public string ImageUrl { get; set; }
    }
}