using OpenMind.Models;

namespace OpenMind.Contracts.Requests.Users
{
    public class AddProgressRequest : Localizable
    {
        public string SectionNumber { get; set; }
        public string Progress { get; set; }
    }
}