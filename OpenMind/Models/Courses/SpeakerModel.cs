using System.Collections.Generic;

namespace OpenMind.Models.Courses
{
    public class SpeakerModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CourseModel> Courses { get; set; }
    }
}