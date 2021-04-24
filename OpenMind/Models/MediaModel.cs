namespace OpenMind.Models
{
    public class MediaModel : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get;set; }
        public string ImageUrl { get; set; }
        public int Type { get; set; }
        public long UploadedTime { get; set; }
    }
}