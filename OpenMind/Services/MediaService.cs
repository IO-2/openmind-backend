using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenMind.Contracts.Responses.Media;
using OpenMind.Data;
using OpenMind.Domain;
using OpenMind.Domain.Media;
using OpenMind.Models.Media;
using OpenMind.Services.Interfaces;
using PagedList.Core;

namespace OpenMind.Services
{
    public class MediaService : FileWorkerService, IMediaService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly DataContext _context;

        private int CategoryAmount { get; set; } = 4;

        protected override string ContentsFolderName { get; set; } = "Media";
        protected override IList<string> AllowedFiles { get; set; } = new List<string> {"image/png", "image/jpg", "image/jpeg"};

        public MediaService(DataContext context, IWebHostEnvironment environment)
        {
            this._environment = environment;
            this._context = context;
        }
        
        public async Task<ServiceActionResult> CreateAsync(string name, string text, int type, IFormFile file, string locale, int category)
        {
            try
            {
                if (file.Length > 0 && AllowedFiles.Contains(file.ContentType))
                {
                    var result = await SaveFile(_environment.WebRootPath, ContentsFolderName, file);
                    
                    _context.Media.Add(new MediaModel
                    {
                        Title = name,
                        ImageUrl = result.SavedFilePath,
                        Locale = locale,
                        Text = text,
                        Type = type,
                        UploadedTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        Category = category
                    });
                    await _context.SaveChangesAsync();
                    return OkServiceActionResult();
                }
            }
            catch (Exception e)
            {
                return BadServiceActionResult(e.Message);
            }
            return BadServiceActionResult("File error");
        }

        public async Task<ServiceActionResult> GetInfoAsync(int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(x => x.Id == id);
            if (media == null)
            {
                return BadServiceActionResult($"Media with id {id} not found");
            }

            return new MediaActionResult
            {
                Media = new MediaResponseContract
                {
                    Id = media.Id,
                    Title = media.Title,
                    Text = media.Text,
                    Type = media.Type,
                    Category = media.Category,
                    UploadedTime = media.UploadedTime,
                    ImageUrl = media.ImageUrl
                },
                Success = true
            };
        }

        public async Task<ServiceActionResult> GetFileAsync(int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(x => x.Id == id);
            if (media == null)
            {
                return BadServiceActionResult($"Media with id {id} not found");
            }

            return new FileActionResult
            {
                FileName = media.ImageUrl.Substring(media.ImageUrl.LastIndexOf("/") + 1),
                FilePath = media.ImageUrl,
                FileType = "image/" + media.ImageUrl.Substring(media.ImageUrl.LastIndexOf(".") + 1),
                Success = true
            };
        }

        public async Task<ServiceActionResult> DeleteAsync(int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(x => x.Id == id);
            if (media == null)
            {
                return BadServiceActionResult($"Media with id {id} not found");
            }
            
            DeleteFile(media.ImageUrl);

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
            
            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> GetInfoAllAsync(string locale)
        {
            var resultMedias = new List<MediaModel>();
            for (int i = 1; i <= CategoryAmount; i++)
            {
                var medias = _context.Media
                    .Where(x => x.Locale == locale && x.Category == i)
                    .Take(8);
                
                resultMedias.AddRange(medias);
            }

            resultMedias = resultMedias
                .OrderBy(x => x.UploadedTime)
                .Reverse()
                .ToList();
            
            var result = new ListResult<BriefMediaResult>
            {
                Data = resultMedias.Select(x => new BriefMediaResult
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Type = x.Type,
                        Category = x.Category,
                        UploadedTime = x.UploadedTime,
                        ImageUrl = x.ImageUrl
                    }).ToList(),
                Success = true
            };
            
            return result;
        }

        public async Task<ServiceActionResult> GetInfoByCategory(int page, int category, string locale)
        {
            var medias = _context.Media
                .Where(x => x.Locale == locale && x.Category == category)
                .OrderBy(x => x.UploadedTime)
                .Reverse()
                .ToList();

            var selected = medias.Select(x => new BriefMediaResult
            {
                Id = x.Id,
                Title = x.Title,
                Type = x.Type,
                Category = x.Category,
                UploadedTime = x.UploadedTime,
                ImageUrl = x.ImageUrl
            }).AsQueryable(); // AsQueryable is very important in this context. Causes error without it
            var paged = selected.ToPagedList(page, 20);
            var listed = paged.ToList();
            
            var result = new ListResult<BriefMediaResult>
            {
                Data = listed,
                Success = true
            };
            
            return result;
        }
    }
}
