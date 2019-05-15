using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jcr.Api.Models
{
   
    public class GuestLink
    {
        public int GuestLinkID { get; set; }
        public Guid GuestLinkGUID { get; set; }

        public bool IsEnabled { get; set; }
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public int TracerCustomID { get; set; }
        public string TracerCustomName { get; set; }
        public int ObservationID { get; set; }

        public int TracerStatusID { get; set; }
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public DateTime? ExpirationDate1 { get; set; }
        public string ExpirationDate { get; set; }
        public int CreatedByID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByID { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string GuestLinkUrl { get; set; }
        public int CycleID { get; set; }

        public bool IsExpired { get; set; }

        public string LinkStatus { get; set; }

        public bool IsSiteActive { get; set; }
        public DateTime TracerExpirationDate { get; set; }

        public bool IsSetupForGuestAccess { get; set; }
        public bool IsCaptchaValidated { get; set; }

        public bool? IsCmsLicenseActive { get; set; }
        public string TracerType { get; set; }
        // public UserInfo CurrentUser { get; set; }
    }
}