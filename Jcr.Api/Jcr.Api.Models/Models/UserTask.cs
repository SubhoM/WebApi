using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.Models
{
  public  class UserTask
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public int TaskTypeID { get; set; }
        public int SiteID { get; set; }
        public int ProgramID { get; set; }
        public int TracerCustomID { get; set; }
        public string TracerName { get; set; }
        public int TracerResponseID { get; set; }

        public int TracerQuestionID { get; set; }
        public int TracerQuestionAnswerID { get; set; }
        public int EPTextID { get; set; }        
        public string Std { get; set; }
        public int CMSStandardID { get; set; }
        public string AssignedDate { get; set; }
        public int AssignedToUserID { get; set; }
        public string AssignedToUserName { get; set; }
        public int AssignedByUserID { get; set; }
        public string AssignedByUserName { get; set; }
        public int CreatedByUserID { get; set; }
        public int UpdatedByUserID { get; set; }
        public string TaskDetails { get; set; }
        public string CCUserIDs { get; set; }
        public string DueDate { get; set; }
        public int TaskStatus { get; set; }
        public string StatusName { get; set; }
        public Boolean ReminderEmailRequired { get; set; }
        public string TaskResolution { get; set; }
        public string CompleteDate { get; set; }
        public string LstUsers { get; set; }
        public string ObservationTitle { get; set; }
    }
}
