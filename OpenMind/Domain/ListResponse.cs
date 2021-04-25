using System.Collections.Generic;

namespace OpenMind.Domain
{
    public class ListResponse<T> : ServiceActionResult
    {
        public IList<T> Data { get; set; }
    }
}