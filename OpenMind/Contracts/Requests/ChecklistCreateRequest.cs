using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class ChecklistCreateRequest : Localizable
    {
        public string Title { get; set; }
        public IFormFileCollection File { get; set; }
    }
}