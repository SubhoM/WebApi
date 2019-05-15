using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jcr.Api.Models
{
    public class TracerObservation
    {
        public int? TracerId { get; set; }
        public int? ObservationId { get; set; }

        public string Title { get; set; }
        public System.DateTime? ObservationDate { get; set; }
        public string MedicalStaffInvolved { get; set; }
        public string StaffInterviewed { get; set; }
        public string SurveyTeam { get; set; }
        public string DepartmentId { get; set; }
        public string Location { get; set; }
        public int? ObservationStatusId { get; set; }
        public int? TracerErrorValue { get; set; }
        public string Note { get; set; }
        public int? UserId { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string EquipmentObserved { get; set; }
        public string ContractedService { get; set; }
        public System.Data.DataTable GridData { get; set; }
        public bool? IsCalledByGuestAccess { get; set; }
        public int ObservationsCount { get; set; }
    }

    public class QuestionImage : FileUploadResult
    {
        public int TracerQuestionID { get; set; }

        public int SiteID { get; set; }
        public int ProgramID { get; set; }
        public int TracerCustomID { get; set; }

        public string FileName { get; set; }
    }
}