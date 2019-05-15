using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Jcr.Api.Enumerators
{
    public class Enums
    {

        public enum EProduct
        {
            AMP = 1,
            Tracers = 2,
            Edition = 3,
            OrderProcessing = 4,
            DataExchange = 5,
            IPortal = 6,
            IApp = 7,
            CCM = 8,
            GlobalAdmin = 9,
            Portal = 10,
            Reports = 11,
            EnterPriseReporting = 12,
            ECM = 13

        }
        public enum CodeCategoryEnum
        {
            SecurityQuestions = 64,
            Role = 67,
            UserPreference = 96,
            DeemedStatus = 5,
            PriorityFocusAreas = 7,
            CriticalChange = 11,
            Requiredocumentation = 14,
            Methodofassessment = 15,
            MedicationandDispensing = 17,
            Occupancy = 18,
            DeemedStatusByServices = 19,
            OPDDeemingForBHC = 20,
            CSATOpoidGuidlines = 21,
            ScoringTier = 50,
            ScoringChapter = 51,
            StandardType = 52,
            EPMOSIndicator = 53,
            Scoringcategory = 54,
            EPScores = 55,
            USStates = 56,
            Countries = 57,
            CommentsCompanies = 58,
            UserSecurityAttributeType = 59,
            ServiceType = 60,
            SubscriptionType = 61,
            SubscriptionLevel = 62,
            SubscriptionDeploymentOption = 63,
            SubscriptionStatusID = 65,
            AddressTypeID = 66,
            MaintenanceTable = 71,
            AddressOwnerType = 72,
            SubscriptionLengthOption = 73,
            PaymentStatus = 74,
            HistoryTrackerStatus = 75,
            ScoreStatus = 76,
            TaskStatus = 77,
            ScoreType = 78,
            ReleaseType = 79,
            SerialNumberStatus = 80,
            EventType = 81,
            SiteSettings = 82,
            SendEmailTask = 83,
            DoNotSendEmailTask = 84,
            SendEmailAssignEP = 85,
            DoNotSendEmailAssignEP = 86,
            EncryptionType = 87,
            SitePasswordLength = 88,
            SitePasswordResetInterval = 89,
            SitePasswordSpecialRequirements = 90,
            SitePasswordUpperCaseRequirements = 91,
            SitePasswordNumericRequirements = 92,
            SpecialSurveyOptions = 9,
            ServiceDeemedOnly = 93
        }
        public enum UserSecurityAttributeType
        {
            Password = 308,
            PasswordAnswer1 = 309,
            PasswordAnswer2 = 310,
            RegistrationCode = 307,
            PasswordAnswer3 = 311,
            ActivationCode = 312,
            ForcePasswordReset = 300,
            LastAccessED = 301,
            LastAccessAMP = 302,
            LastAccessECM = 303,
            LastAccessTracers = 304,
            LastAccessEnterpriseReporting = 327,
            AppLogins = 330
        }

        public enum DeviceType
        {
            Desktop = 1,
            Mobile = 2
        }

        public enum ActionTypeEnum
        {
            TracersEmailed = 6,
            TracerSetUpPage = 43,
            TaskCreatedViaTaskAssignmentPage = 42,
            TaskcreatedviaAddEditObservationpage = 46,
            TaskAssignmentEdit = 127,
            TaskAssignmentDelete = 128,
            GuestAccessPassCode = 129,
            TaskNew = 164,
            TaskEdit = 165,
            TaskCompleted = 166,
            TaskDueSoon = 167,
            TaskOverdue = 168,
            TaskReportSchedule = 169,
            TaskReassign = 171
        }

        public enum CategoryHierarchy
        {
            Campus = 3,
            Building = 2,
            Department = 1
        }
        public enum ApplicationCode
        {
            Tracers,
            TracerswithAMP
        }
        public enum TracerStatusID
        {
            Published = 1,
            Unpublished = 2,
            Closed = 3,
            Deleted = 12
        }
        public enum RoleType
        {
            [Description("Program Administrator")]
            ProgramAdministrator = 1,
            [Description("Site Manager")]
            SiteManager = 2,
            [Description("Team Coordinator")]
            TeamCoordinator = 3,
            [Description("Staff Member")]
            StaffMember = 4,
            [Description("Global Admin")]
            GlobalAdministrator = 5,
            [Description("Support")]
            Support = 6,
            [Description("Guest User")]
            GuestUser = 7,
            [Description("Corporate User")]
            CorporateUser = 8,
            [Description("Corporate Reviewer")]
            CorporateReviewer = 9
        }

        public enum ExternalUserRoleType
        {
            TracerAssignmentGuest = 101
        }

        public enum TaskEmailTemplateID
        {
            NewTask = 18,
            EditTask = 19,
            DueSoon = 20,
            OverDue = 21,
            CompletedTask = 22,
            NewTaskToCC = 23,
            TaskReportSchedule = 24,
            TaskReassignedToPrevAssignee = 26,
            TaskReassignedToNewAssignee = 27,
            TaskReassignedToCCUser = 28
        }

        public enum TaskStatus
        {
            Open = 1,
            Complete = 2,
            Deleted = 3
        }

        public enum TaskType
        {
            Generic = 1,
            AMPEP = 2,
            Tracer = 3,
            TracerObservation = 4,
            TracerObservationQuestion = 5,
            CMSStandard = 6
        }

        public enum DisableTaskNotificationScheduleType
        {
            EmailSetting = 1,
            TaskChanged = 2
        }

    }
}
