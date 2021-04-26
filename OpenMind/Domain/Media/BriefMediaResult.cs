using OpenMind.Models;

namespace OpenMind.Domain.Media
{
    public class BriefMediaResult : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public long UploadedTime { get; set; }
    }
}