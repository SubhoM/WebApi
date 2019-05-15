using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.Models
{
    public class UserPreference
    {
        public int UserID { get; set; }
        public int SiteID { get; set; }
        public int ProgramID { get; set; }
        public int? preferenceID { get; set; }
        public string preferenceType { get; set; }
        public string PreferenceValue { get; set; }

    }
}
