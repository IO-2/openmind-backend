using OpenMind.Models;

namespace OpenMind.Contracts.Requests.Users
{
    public class AddProgressRequest : Localizable
    {
        public int SectionNumber { get; set; }
        public int Progress { get; set; }
    }
}