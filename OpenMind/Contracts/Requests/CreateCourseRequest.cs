using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class CreateCourseRequest : Localizable
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string VideoUrl { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int LessonsNumber { get; set; }
        [Required]
        public string LessonsDescription { get; set; }
        [Required]
        public string WhatWillBeLearned { get; set; }
        [Required]
        public IFormFile SpeakerPicture { get; set; }
        [Required]
        public string SpeakerDescription { get; set; }
        [Required]
        public int Section { get; set; }
        [Required]
        public int CourseDuration { get; set; }
    }
}