using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.Models
{
    public class ReAssignTasks
    {
        public string LstTaskIDs { get; set; }
        public int AssignedToUserID { get; set; }
        public string CCUserIDs { get; set; }
        public string DueDate { get; set; }
        public int SiteID { get; set; }
        public int ProgramID { get; set; }        
        public int UpdatedByUserID { get; set; }
    }
}
