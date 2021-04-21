using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class SectionCreateRequest : Localizable
    {
        public string Name { get; set; }
        public int SectionNumber { get; set; }
    }
}