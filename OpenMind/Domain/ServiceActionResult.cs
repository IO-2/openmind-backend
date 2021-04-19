using System.Collections;
using System.Collections.Generic;

namespace OpenMind.Domain
{
    public class ServiceActionResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}