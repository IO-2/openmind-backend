using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OpenMind.Contracts.Responses;
using OpenMind.Data;
using OpenMind.Domain;
using OpenMind.Models;

namespace OpenMind.Services
{
    public class SectionService : Service, ISectionService
    {
        // Change when adding new section
        private int maxSectionNumber = 4;
        
        private readonly DataContext _context;
        
        public SectionService(DataContext context)
        {
            this._context = context;
        }
        
        public async Task<ServiceActionResult> AddSection(string name, int sectionNumber, string locale)
        {
            if (!IsValid(sectionNumber))
            {
                return BadServiceActionResult("Section number is invalid");
            }
            
            await _context.Sections.AddAsync(new SectionModel()
            {
                Name = name,
                SectionNumber = sectionNumber,
                Locale = locale
            });
            await _context.SaveChangesAsync();

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> DeleteSection(int id)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(x => x.Id == id);

            if (section == null)
            {
                return BadServiceActionResult($"Section with id {id} not found");
            }
            
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();

            return OkServiceActionResult();
        }

        public async Task<ServiceActionResult> UpdateSection(int id, string name = null, int? sectionNumber = null, string locale = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceActionResult> GetAll(string locale)
        {
            var list = await _context.Sections
                .Where(x => x.Locale == locale)
                .Select(x => new SectionResponseContract
                {
                    Id = x.Id,
                    Name = x.Name,
                    Locale = x.Locale,
                    SectionNumber = x.SectionNumber
                })
                .ToListAsync();
            
            if (list.Count == 0)
            {
                return BadServiceActionResult($"No sections with locale {locale}");
            }

            return new SectionsActionResult
            {
                Sections = list,
                Success = true
            };
        }

        // If section is in range
        private bool IsValid(int sectionNumber)
        {
            return sectionNumber >= 1 && sectionNumber <= maxSectionNumber;
        }
    }
}