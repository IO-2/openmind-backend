namespace OpenMind.Models.Courses
{
    public class CourseBenefitersModel : Localizable
    {
        public int Id { get; set; }
        public int BenefiterNumber { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual CourseModel Course { get; set; }
    }
}