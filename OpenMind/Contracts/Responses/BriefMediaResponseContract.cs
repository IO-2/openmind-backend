using OpenMind.Models;

namespace OpenMind.Contracts.Responses
{
    public class BriefMediaResponseContract : Localizable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
    }
}