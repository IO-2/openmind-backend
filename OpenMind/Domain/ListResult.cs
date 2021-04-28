using System.Collections.Generic;

namespace OpenMind.Domain
{
    public class ListResult<T> : ServiceActionResult
    {
        public IList<T> Data { get; set; }
    }
}