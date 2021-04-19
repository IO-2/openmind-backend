namespace OpenMind.Models
{
    public class CoursePageModel : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUtl { get; set; }
        
        public virtual CourseModel Course { get; set; }
    }
}