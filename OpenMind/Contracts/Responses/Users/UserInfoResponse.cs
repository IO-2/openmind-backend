using System.Collections.Generic;

namespace OpenMind.Contracts.Responses.Users
{
    public class UserInfoResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string DreamingAbout { get; set; }
        public string Inspirer { get; set; }
        public string WhyInspired { get; set; }
        public long SubscriptionEndDate { get; set; }
        public ICollection<int> Interests { get; set; }
        public Dictionary<int, float> Successes { get; set; }
        public string AvatarUrl { get; set; }
    }
}