using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.UserMenuState {
    public class Init {
        public int UserID { get; set; }
        public int SiteID { get; set; }
        public int RoleID { get; set; }
        public bool AccessToEdition { get; set; }
        public bool AccessToAMP { get; set; }
        public bool AccessToTracers { get; set; }
        public bool AccessToERAMP { get; set; }
        public bool AccessToERTracers { get; set; }
    }
}
