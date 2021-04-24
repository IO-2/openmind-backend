using OpenMind.Models;

namespace OpenMind.Contracts
{
    public class CourseCardContract : Localizable
    {
        public int CourseId { get; set; }
        public int CardNumber { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}