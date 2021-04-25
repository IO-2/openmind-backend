using System.Collections.Generic;
using OpenMind.Contracts.Responses;
using PagedList.Core;

namespace OpenMind.Domain
{
    public class ListResponse<T> : ServiceActionResult
    {
        public IList<T> Data { get; set; }
    }
}