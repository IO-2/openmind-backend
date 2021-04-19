using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenMind.Contracts;

namespace OpenMind.Controllers
{
    [ApiVersion("1.0")]
    public class ChecklistController : MyControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        
        public ChecklistController(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }
        
        [HttpGet("get")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get()
        {
            return Ok("Hello, world!");
        }

        [HttpPost("create")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromForm] ChecklistCreateContract contract)
        {
            // TODO: NUll reference, cannot upload file with text
            var file = contract.File;
            try
            {
                if (file.Length > 0 && file.ContentType == "pdf")
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Checklists\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Checklists\\");
                    }

                    // TODO: Hash name
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Checklists\\" +
                                                                         file.FileName))
                    {
                        await file.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                        return Ok("\\Checklists\\" + file.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            return BadRequest("File error");
        }
    }
}