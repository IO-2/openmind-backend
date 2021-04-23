namespace OpenMind.Models
{
    public class CourseCardModel : Localizable
    {
        public int Id { get; set; }
        public int CardNumber { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}