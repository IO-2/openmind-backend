namespace OpenMind.Contracts.Responses
{
    public class BriefCourseResponseContract
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int LessonNumber { get; set; }
    }
}