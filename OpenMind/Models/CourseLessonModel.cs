namespace OpenMind.Models
{
    public class CourseLessonModel : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int LessonNumber { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
    }
}