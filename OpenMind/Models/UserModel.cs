using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OpenMind.Models
{
    public class UserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Country { get; set; }
        public string AvatarUrl { get; set; }
        public int PrivilegeLevel { get; set; }
        public string DreamingAbout { get; set; }
        public string Inspirer { get; set; }
        public string WhyInspired { get; set; }
        public long SubscriptionEndDate { get; set; }
        public virtual ICollection<InterestModel> Interests { get; set; }
        public virtual ICollection<UserProgressBySectionModel> Progresses { get; set; }
        public virtual ICollection<UserRateCourseModel> UserRatresCources { get; set; }
        
        public virtual RefreshTokenModel RefreshToken { get; set; }
    }
}