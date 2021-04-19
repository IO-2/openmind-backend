using OpenMind.Models;

namespace OpenMind.Contracts
{
    public class SectionCreateContract : Localizable
    {
        public string Name { get; set; }
        public int SectionNumber { get; set; }
    }
}