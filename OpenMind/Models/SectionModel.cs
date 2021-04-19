using System.Xml;

namespace OpenMind.Models
{
    public class SectionModel : Localizable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionNumber { get; set; } // 1 - IT, 2 - SMM, 3 - Design, 4 - Content
    }
}