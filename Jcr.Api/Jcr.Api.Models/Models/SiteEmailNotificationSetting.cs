using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.Models
{
    public class SiteEmailNotificationSetting
    {
        public int SiteID { get; set; }
        public bool SendEmailOnTaskCreation { get; set; }
        public bool SendEmailBeforeTaskDue { get; set; }
        public int DaysBeforeTaskDue { get; set; }
        public bool SendEmailsAfterTaskDue { get; set; }
        public int DaysAfterTaskDue { get; set; }
        public bool SendRemainderEmailAfterTaskDue { get; set; }
        public bool SendTaskReportToCC { get; set; }
        public int TaskReportToCCScheduleTypeID { get; set; }
        public bool SendTaskReportToUsers { get; set; }
        public int TaskReportToUsersScheduleTypeID { get; set; }
        public string TaskReportRecipients { get; set; }
        public bool SendEmailOnAssigningEP { get; set; }
        public int TaskDueRecipientType { get; set; }
        public int UpdatedBy { get; set; }
    }
}
