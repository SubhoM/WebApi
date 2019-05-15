using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jcr.Api.Models
{
    public class GuestUserInfo
    {
        public GuestUserInfo()
        {
            UserSiteInfo = new UserSite();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string FormattedFullName
        {
            get { return LastName + ", " + FirstName; }
        }
        public string EmailAddress { get; set; }
        public int UserID { get; set; }
        public string MiddleName { get; set; }

        public bool IsActive { get; set; }

        public UserSite UserSiteInfo { get; set; }
        public bool IsUserSiteMapPresent { get; set; }
        public bool IsEulaAccepted { get; set; }
        public bool IsGlobalAdmin { get; set; }
        public bool IsNewUser { get; set; }

        public bool IsRegisteredUser { get; set; }
        public bool IsValidatedUser { get; set; }
        public class UserSite
        {
            public int SiteID { get; set; }
            public string SiteFullName { get; set; }
            public int RoleID { get; set; }
            public string SiteName { get; set; }

            public bool IsTracersAccess { get; set; }
            public bool IsSiteActive { get; set; }

            public List<Program> Programs { get; set; }
        }
        public class Program
        {
            // For Advanced Certification Programs, this property is a concatenation of ProgramID & CertificationItemID. This is done to guarantee a unique key.
            public int ProgramServiceID { get; set; }
            public string ProgramSettingName { get; set; }

            // For certification programs, like "Disease Specific Care" or "Palliative Care", CertificationItemID will have value of 0.
            public int CertificationItemID { get; set; }

            // Advanced-Diseases, such as Heart Failure will have BOTH of the following properties set to true.
            // Certifications suc as Disease Specific Care or Palliative Care will only have IsCertification set to true.
            public bool IsCertification { get; set; }
            public bool IsAdvanced { get; set; }

            public int ParentID { get; set; }
            public int StandardGroupID { get; set; }  // 1 is for accreditation programs, 2 is for certification programs.
            public int TJCProgramID { get; set; }
        }
    }
}