using System;

namespace Jcr.Api.Models
{
    public class AssignedToUserTaskEmail
    {
        public string AssignedToFirstName { get; set; }
        public string AssignedToLastName { get; set; }
        public string AssignedToFullName { get { return AssignedToFirstName + " " + AssignedToLastName; } }
        public string AssignedToFormattedName { get { return AssignedToLastName + ", " + AssignedToFirstName; } }
        public string AssignedToEmail { get; set; }

    }
    public class UserTaskEmail: AssignedToUserTaskEmail
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public int TaskTypeID { get; set; }
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public int? HCOID { get; set; }
        public int ProgramID { get; set; }
        public int AssignedByUserID { get; set; }
        public int AssignedToUserID { get; set; }
        public int UpdatedByUserID { get; set; }
        public int CreatedByUserID { get; set; }

        public string AssignedByFirstName { get; set; }
        public string AssignedByLastName { get; set; }
        public string AssignedByFullName { get { return AssignedByFirstName + " " + AssignedByLastName; } }
        public string AssignedByFormattedName { get { return AssignedByLastName + ", " + AssignedByFirstName; } }

        public string UpdatedByFirstName { get; set; }
        public string UpdatedByLastName { get; set; }
        public string UpdatedByFullName { get { return UpdatedByFirstName + " " + UpdatedByLastName; } }
        public string UpdatedByFormattedName { get { return UpdatedByLastName + ", " + UpdatedByFirstName; } }

        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public string CreatedByFullName { get { return CreatedByFirstName + " " + CreatedByLastName; } }
        public string CreatedByFormattedName { get { return CreatedByLastName + ", " + CreatedByFirstName; } }

        public string AssignedByEmail { get; set; }
        public string UpdatedByEmail { get; set; }
        public string CreatedByEmail { get; set; }
        public string AssignedOn { get; set; }
        public string DueDate { get; set; }
        public string CompletionDate { get; set; }
        public string TaskDetails { get; set; }
        public string CCUserIDs { get; set; }
        public int TaskStatus { get; set; }
        public string TaskStatusName { get; set; }
        public string CompletionDetails { get; set; }
        public string TaskLinkUrl { get; set; }
        public string ItemAssociated { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int TracerCustomID { get; set; }
        public int TracerResponseID { get; set; }
        public int TracerQuestionID { get; set; }
        public int TracerQuestionAnswerID { get; set; }
        public int EPTextID { get; set; }
        public int CMSStandardID { get; set; }
        public string ProgramName { get; set; }
    }
}
