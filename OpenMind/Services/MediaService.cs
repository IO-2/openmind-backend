using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenMind.Contracts.Responses;
using OpenMind.Data;
using OpenMind.Domain;
using OpenMind.Models;
using PagedList.Core;

namespace OpenMind.Services
{
    public class MediaService : FileWorkerService, IMediaService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly DataContext _context;

        public MediaService(DataContext context, IWebHostEnvironment environment)
        {
            this._environment = environment;
            this._context = context;
            base.AllowedFiles = new List<string> {"image/png", "image/jpg", "image/jpeg"};
        }
        
        public async Task<ServiceActionResult> Create(string name, string text, int type, IFormFile file, string locale)
        {
            try
            {
                if (file.Length > 0 && AllowedFiles.Contains(file.ContentType))
                {
                    var result = await SaveFile(_environment.WebRootPath, "Media", file);
                    
                    _context.Media.Add(new MediaModel
                    {
                        Title = name,
                        ImageUrl = result.SavedFilePath,
                        Locale = locale,
                        Text = text,
                        Type = type,
                        UploadedTime = DateTimeOffset.Now.ToUnixTimeSeconds()
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

        public async Task<ServiceActionResult> GetInfo(int id)
        {
            var media = await _context.Media.FirstOrDefaultAsync(x => x.Id == id);
            if (media == null)
            {
                return BadServiceActionResult($"Media with id {id} not found");
            }

            return new MediaActionResult()
            {
                Media = new MediaResponseContract
                {
                    Title = media.Title,
                    Locale = media.Locale,
                    Text = media.Text
                },
                Success = true
            };
        }

        public async Task<ServiceActionResult> GetFile(int id)
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
                FileType = "images/" + media.ImageUrl.Substring(media.ImageUrl.LastIndexOf(".") + 1),
                Success = true
            };
        }

        public async Task<ServiceActionResult> Delete(int id)
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

        public async Task<ServiceActionResult> GetInfoAll(int page, string locale)
        {
            var medias = _context.Media
                .Where(x => x.Locale == locale)
                .OrderBy(x => x.UploadedTime)
                .Reverse();

            return new AllMediaActionResult
            {
                Medias = medias.ToPagedList(page, 20).Select(x => new MediaResponseContract
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    Locale = x.Locale,
                    Type = x.Type
                }),
                Success = true
            };
        }
    }
}