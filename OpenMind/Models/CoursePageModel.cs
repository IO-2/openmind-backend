namespace OpenMind.Models
{
    public class CoursePageModel : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PageNumber { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
    }
}