using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class PaginatingSearchRequest : Localizable
    {
        public int Page { get; set; }
        public string Query { get; set; }
    }
}