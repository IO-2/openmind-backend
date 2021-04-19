using System.Collections.Specialized;

namespace OpenMind.Models
{
    public class CourseModel : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageAmount { get; set; }
        public string ImageUrl { get; set; }
        public int PeoplePassedAmount { get; set; }
        
        public virtual SectionModel SectionModel { get; set; }
        
    }
}