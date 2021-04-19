using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace OpenMind.Contracts
{
    public class ChecklistCreateContract
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
    }
}