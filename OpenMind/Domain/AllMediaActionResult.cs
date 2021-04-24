using System.Collections.Generic;
using OpenMind.Contracts.Responses;

namespace OpenMind.Domain
{
    public class AllMediaActionResult : ServiceActionResult
    {
        public IEnumerable<MediaResponseContract> Medias { get; set; }
    }
}