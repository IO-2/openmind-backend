using OpenMind.Models;

namespace OpenMind.Contracts
{
    public class CourseBenefiterContract : Localizable
    {
        public int CourseId { get; set; }
        public int BenefiterNumber { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}