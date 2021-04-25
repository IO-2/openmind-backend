using System.Collections.Generic;
using OpenMind.Contracts.Responses;
using PagedList.Core;

namespace OpenMind.Domain
{
    public class PaginatingResponse<T> : ServiceActionResult
    {
        public IList<T> Page { get; set; }
    }
}