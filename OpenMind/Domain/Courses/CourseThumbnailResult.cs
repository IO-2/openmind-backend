namespace OpenMind.Domain.Courses
{
    public class CourseThumbnailResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Section { get; set; }
        public string ImageUrl { get; set; }
        public int CourseDuration { get; set; }
    }
}