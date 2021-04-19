using OpenMind.Models;

namespace OpenMind.Contracts.Responses
{
    public class SectionResponseContract : Localizable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionNumber { get; set; }
    }
}