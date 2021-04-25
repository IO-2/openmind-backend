using OpenMind.Models.Courses;

namespace OpenMind.Models.Users
{
    public class UserRateCourseModel : Localizable
    {
        public int Id { get; set; }
        public virtual CourseModel Course { get; set; }
        public virtual UserModel User { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}