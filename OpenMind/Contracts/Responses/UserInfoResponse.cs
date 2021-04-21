using System;

namespace OpenMind.Contracts.Responses
{
    public class UserInfoResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string DreamingAbout { get; set; }
        public string Inspirer { get; set; }
        public string WhyInspired { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
    }
}