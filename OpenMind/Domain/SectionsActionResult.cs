using System.Collections.Generic;
using OpenMind.Contracts.Responses;

namespace OpenMind.Domain
{
    public class SectionsActionResult : ServiceActionResult
    {
        public IEnumerable<SectionResponseContract> Sections { get; set; }
    }
}