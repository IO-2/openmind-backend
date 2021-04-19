using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OpenMind.Models;

namespace OpenMind.Contracts
{
    public class ChecklistCreateContract : Localizable
    {
        public string Title { get; set; }
        public IFormFileCollection File { get; set; }
    }
}