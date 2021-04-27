using System.Collections.Generic;
using OpenMind.Models;

namespace OpenMind.Domain.Courses
{
    public class CourseResult : Localizable
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public long UploadedTime { get; set; }
        public string Description { get; set; }
        public string LessonsDescription { get; set; }
        public int LessonsAmount { get; set; }
        public string WhatWillBeLearned { get; set; }
        public string SpeakerDescription { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerImage { get; set; }
        public int Section { get; set; }
        public int CourseDuration { get; set; }
        public IEnumerable<CourseCardResult> Cards { get; set; }
        public IEnumerable<CourseBenefiterResult> Benefiters { get; set; }
        public IEnumerable<BriefCourseLessonResult> Lessons { get; set; }

        public string ImageUrl { get; set; }
    }
}