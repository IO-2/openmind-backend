using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace OpenMind.Models
{
    public class CourseModel : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int PeoplePassedAmount { get; set; }
        
        public virtual ICollection<UserRateCourseModel> Rates { get; set; }

        // Description
        public string Description { get; set; }
        public virtual ICollection<CourseCardModel> Cards { get; set; }
        public string WhatWillBeLearned { get; set; }
        public string SpeakerPictureUrl { get; set; }
        public string SpeakerDescription { get; set; }
        
        // Lessons
        public string LessonsDescription { get; set; }
        public virtual ICollection<CoursePageModel> Pages { get; set; }
        public int Section { get; set; }
        public int CourseDuration { get; set; } // Minutes
    }
}