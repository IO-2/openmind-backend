using System.Collections.Generic;
using OpenMind.Models;
using OpenMind.Models.Courses;

namespace OpenMind.Domain.Courses
{
    public class CourseResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public long UploadedTime { get; set; }
        public string Description { get; set; }
        public string LessonsDescription { get; set; }
        public int LessonsAmount { get; set; }
        public string WhatWillBeLearned { get; set; }
        public SpeakerResult Speaker { get; set; }
        public int Section { get; set; }
        public int CourseDuration { get; set; }
        public IEnumerable<CourseCardResult> Cards { get; set; }
        public IEnumerable<CourseBenefiterResult> Benefiters { get; set; }
        public IEnumerable<BriefCourseLessonResult> Lessons { get; set; }

        public string ImageUrl { get; set; }
    }
}