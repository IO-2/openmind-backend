using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Hosting;
using OpenMind.Contracts;
using OpenMind.Contracts.Requests.Courses;
using OpenMind.Data;
using OpenMind.Domain;
using OpenMind.Domain.Courses;
using OpenMind.Models.Courses;
using OpenMind.Services.Interfaces;
using PagedList.Core;

namespace OpenMind.Services
{
    public class CourseService : FileWorkerService, ICoursesService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly DataContext _context;

        protected override IList<string> AllowedFiles { get; set; } = new List<string> {"image/png", "image/jpg", "image/jpeg"};
        protected override string ContentsFolderName { get; set; } = "Courses";

        public CourseService(IWebHostEnvironment environment, DataContext context)
        {
            _environment = environment;
            _context = context;
        }

        public async Task<ServiceActionResult> CreateCourseAsync(CreateCourseRequest contract)
        {
            try
            {
                SaveFileResponse resultImage = null;
                SaveFileResponse resultSpeakerPicture = null;
                
                if (contract.Image.Length > 0 && AllowedFiles.Contains(contract.Image.ContentType))
                {
                    resultImage = await SaveFile(_environment.WebRootPath, ContentsFolderName, contract.Image);
                    if (resultImage is null)
                    {
                        throw new Exception("Unable to save course picture");
                    }                    
                }
                else
                {
                    throw new Exception("No picture provided to course");
                }

                if (contract.SpeakerPicture.Length > 0 && AllowedFiles.Contains(contract.SpeakerPicture.ContentType))
                {
                    resultSpeakerPicture = await SaveFile(_environment.WebRootPath, "Courses", contract.SpeakerPicture);
                    if (resultSpeakerPicture is null)
                    {
                        throw new Exception("Unable to save speaker picture");
                    }   
                }else
                {
                    throw new Exception("No picture provided to speaker");
                }
                
                var course =  _context.Courses.Add(new CourseModel
                {
                    Title = contract.Title,
                    VideoUrl = contract.VideoUrl,
                    ImageUrl = resultImage!.SavedFilePath,
                    Locale = contract.Locale,
                    Description = contract.Description,
                    LessonsDescription = contract.LessonsDescription,
                    LessonsAmount = contract.LessonsAmount,
                    WhatWillBeLearned = contract.WhatWillBeLearned,
                    Speaker = new SpeakerModel
                    {
                        Name = contract.SpeakerName,
                        ImageUrl = resultSpeakerPicture!.SavedFilePath,
                        Description = contract.SpeakerDescription,
                    },
                    Section = contract.Section,
                    CourseDuration = contract.CourseDuration,
                    UploadedTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                });
                await _context.SaveChangesAsync();
                return new IdResult
                {
                    Success = true,
                    Id = course.Entity.Id
                };
            }
            catch (Exception e)
            {
                return BadServiceActionResult(e.Message);
            }
        }

        public async Task<ServiceActionResult> CreateBenefitersAsync(IEnumerable<CourseBenefiterContract> benefiters)
        {
            try
            {
                foreach (var benefiter in benefiters)
                {
                    _context.Courses
                        .First(x => x.Id == benefiter.CourseId)
                        .Benefiters
                        .Add(new CourseBenefitersModel
                        {
                            BenefiterNumber = benefiter.BenefiterNumber,
                            Locale = benefiter.Locale,
                            Text = benefiter.Text,
                            Title = benefiter.Title
                        });
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return BadServiceActionResult("One or more benefiters has nonexistent CourseId field");
            }

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> CreateCardsAsync(IEnumerable<CourseCardContract> cards)
        {
            try
            {
                foreach (var card in cards)
                {
                    _context.Courses
                        .First(x => x.Id == card.CourseId)
                        .Cards
                        .Add(new CourseCardModel
                        {
                            CardNumber = card.CardNumber,
                            Locale = card.Locale,
                            Text = card.Text,
                            Title = card.Title
                        });
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return BadServiceActionResult("One or more cards has nonexistent CourseId field");
            }

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> CreateCourseLessonAsync(CreateCourseLessonRequest contract)
        {
            try
            {
                _context.Courses
                    .First(x => x.Id == contract.CourseId)
                    .Lessons
                    .Add(new CourseLessonModel
                    {
                        Description = contract.Description,
                        LessonNumber = contract.LessonNumber,
                        Locale = contract.Locale,
                        Title = contract.Title,
                        VideoUrl = contract.VideoUrl
                    });
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadServiceActionResult("Specified CourseId not found");
            }
            
            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> DeleteCourseAsync(int id)
        {
            var course = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (course is null)
            {
                return BadServiceActionResult("Course not found");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> GetAsync(string locale, int page, string query)
        {
            var courses = _context.Courses
                .Where(x => x.Locale == locale && (!query.IsNullOrEmpty() ? x.Title.ToLower().Contains(query.ToLower()) : true))
                .OrderBy(x => x.UploadedTime)
                .Reverse()
                .ToList();

            var data = courses.Select(x => new CourseThumbnailResult
                {
                    Id = x.Id,
                    Section = x.Section,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    CourseDuration = x.CourseDuration
                })
                .AsQueryable()
                .ToPagedList(page, 20)
                .ToList();
            
            var pagedResult = new ListResult<CourseThumbnailResult>
            {
                Success = true,
                Data = data
            };

            return pagedResult;
        }

        // DEPRECATED
        public async Task<ServiceActionResult> GetCoursePictureAsync(int id)
        {
            var course = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (course is null)
            {
                return BadServiceActionResult("Course not found");
            }

            return new FileActionResult
            {
                FileName = course.ImageUrl.Substring(course.ImageUrl.LastIndexOf("/") + 1),
                FilePath = course.ImageUrl,
                FileType = "image/" + course.ImageUrl.Substring(course.ImageUrl.LastIndexOf("." + 1)),
                Success = true
            };
        }

        public async Task<ServiceActionResult> GetInfoAsync(int id)
        {
            var course = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (course is null)
            {
                return BadServiceActionResult("Course not found");
            }

            return new SingleObjectResult<CourseResult>
            {
                Success = true,
                Data = new CourseResult
                {
                    Id = course.Id,
                    Title = course.Title,
                    Benefiters = course.Benefiters.Select(x => new CourseBenefiterResult
                    {
                        BenefiterNumber = x.BenefiterNumber,
                        Id = x.Id,
                        Text = x.Text,
                        Title = x.Title
                    }),
                    Cards = course.Cards.Select(x => new CourseCardResult
                    {
                        CardNumber = x.CardNumber,
                        Id = x.Id,
                        Text = x.Text,
                        Title = x.Title
                    }),
                    Lessons = course.Lessons.Select(x => new BriefCourseLessonResult
                    {
                        LessonNumber = x.LessonNumber,
                        Title = x.Title,
                        Description = x.Description
                    }),
                    CourseDuration = course.CourseDuration,
                    Description = course.Description,
                    LessonsDescription = course.LessonsDescription,
                    LessonsAmount = course.LessonsAmount,
                    Section = course.Section,
                    Speaker = new SpeakerResult
                    {
                        Name = course.Speaker.Name,
                        ImageUrl = course.Speaker.ImageUrl,
                        Description = course.Speaker.Description,
                    },
                    UploadedTime = course.UploadedTime,
                    VideoUrl = course.VideoUrl,
                    WhatWillBeLearned = course.WhatWillBeLearned,
                    ImageUrl = course.ImageUrl
                }
            };
        }

        public async Task<ServiceActionResult> GetLessonAsync(int id, int lessonNumber)
        {
            var lesson = _context.Courses
                .First(x => x.Id == id)
                .Lessons
                .FirstOrDefault(x => x.LessonNumber == lessonNumber);
            
            if (lesson is null)
            {
                return BadServiceActionResult("Course not found");
            }

            return new SingleObjectResult<CourseLessonResult>
            {
                Success = true,
                Data = new CourseLessonResult
                {
                    Description = lesson.Description,
                    LessonNumber = lesson.LessonNumber,
                    Title = lesson.Title,
                    VideoUrl = lesson.VideoUrl
                }
            };
        }
    }
}
