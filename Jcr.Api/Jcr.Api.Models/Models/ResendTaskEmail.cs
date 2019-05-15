using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.Models
{
    public class ResendTaskEmail
    {
        public string TaskIDs { get; set; }
        public int SiteID { get; set; }
        public int ProgramID { get; set; }
        public int UserID { get; set; }
    }
}
