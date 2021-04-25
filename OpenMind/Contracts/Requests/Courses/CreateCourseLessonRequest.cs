using System.ComponentModel.DataAnnotations;
using OpenMind.Models;

namespace OpenMind.Contracts.Requests.Courses
{
    public class CreateCourseLessonRequest : Localizable
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string VideoUrl { get; set; }
        [Required]
        public int LessonNumber { get; set; }
    }
}