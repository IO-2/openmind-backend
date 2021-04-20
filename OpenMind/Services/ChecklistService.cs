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

namespace OpenMind.Services
{
    public class ChecklistService : FileWorkerService, IChecklistService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly DataContext _context;
        private readonly List<string> _allowedFiles = new List<string> { "application/pdf" };

        public ChecklistService(DataContext context, IWebHostEnvironment environment)
        {
            this._environment = environment;
            this._context = context;
        }
        
        public async Task<ServiceActionResult> Create(string name, IFormFile file, string locale)
        {
            try
            {
                if (file.Length > 0 && _allowedFiles.Contains(file.ContentType))
                {
                    var checklistsPath = Path.Combine(_environment.WebRootPath, "Checklists");
                    if (!Directory.Exists(checklistsPath))
                    {
                        Directory.CreateDirectory(checklistsPath);
                    }
                    
                    // TODO: Hash name
                    var newFilename = file.FileName;

                    using (FileStream fileStream = System.IO.File.Create(Path.Combine(checklistsPath, newFilename)))
                    {
                        await file.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                    }

                    _context.Checklists.Add(new ChecklistModel
                    {
                        Title = name,
                        MediaUrl = Path.Combine(checklistsPath, newFilename),
                        Locale = locale
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
            var checklist = await _context.Checklists.FirstOrDefaultAsync(x => x.Id == id);
            if (checklist == null)
            {
                return BadServiceActionResult($"Checklist with id {id} not found");
            }

            return new ChecklistActionResult()
            {
                Checklist = new ChecklistResponseContract
                {
                    Title = checklist.Title,
                    Locale = checklist.Locale
                },
                Success = true
            };
        }

        public async Task<ServiceActionResult> GetFile(int id)
        {
            var checklist = await _context.Checklists.FirstOrDefaultAsync(x => x.Id == id);
            if (checklist == null)
            {
                return BadServiceActionResult($"Checklist with id {id} not found");
            }

            return new FileActionResult
            {
                FileName = checklist.MediaUrl.Substring(checklist.MediaUrl.LastIndexOf("/") + 1),
                FilePath = checklist.MediaUrl,
                FileType = "application/pdf",
                Success = true
            };
        }

        public async Task<ServiceActionResult> Delete(int id)
        {
            // TODO: Remove file from filesystem
            var checklist = await _context.Checklists.FirstOrDefaultAsync(x => x.Id == id);
            if (checklist == null)
            {
                return BadServiceActionResult($"Checklist with id {id} not found");
            }

            _context.Checklists.Remove(checklist);
            await _context.SaveChangesAsync();
            
            return OkServiceActionResult();
        }
    }
}