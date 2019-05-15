using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jcr.Data;
using Jcr.Api.Helpers;
namespace Jcr.Business
{
    public class TracerMobileServices
    {

        ExceptionLogServices exceptionLog = new ExceptionLogServices();
        //Get Tracer Observation Questions     
        public List<ApiMobileGetObservationDetailsByIdReturnModel> GetObservationDetailsById(int? tracerCustomID, int? ResponseId, bool? IsGuestAccess)
        {
            List<ApiMobileGetObservationDetailsByIdReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {
                try {
                    _result = db.ApiMobileGetObservationDetailsById(tracerCustomID, ResponseId, IsGuestAccess);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileGetObservationDetailsById("  + tracerCustomID + "," + ResponseId + "," + IsGuestAccess  + ")";
                    string methodName = "JCRAPI/Business/TracerMobileServices/GetObservationDetailsById";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }
            }
            return _result;
        }
        public ApiGetMobileTracerHeaderReturnModel GetTracerHeader(int tracerCustomId, int tracerResponseId, int siteId, int programId)
        {
            ApiGetMobileTracerHeaderReturnModel _result;
            using (var db = new DBMEdition01Context())
            {
                try
                {
                    _result = db.ApiGetMobileTracerHeader(tracerCustomId, tracerResponseId, siteId, programId).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetMobileTracerHeader("  + tracerCustomId + "," + tracerResponseId + "," + siteId + "," + programId + ")";
                    string methodName = "JCRAPI/Business/TracerMobileServices/GetTracerHeader";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }
            }
            return _result;
        }
        public List<ApiMobileResponsesSelectByTracerIdReturnModel> GetTracerResponses(int? tracerId, string responseStatusCSV, int createdByUserID = 0)
        {
            List<ApiMobileResponsesSelectByTracerIdReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {
               
                try
                {
                    _result = db.ApiMobileResponsesSelectByTracerId(tracerId, responseStatusCSV);

                    if (createdByUserID > 0)
                        _result = _result.Where(item => item.CreatedByID == createdByUserID).ToList();

                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileResponsesSelectByTracerId("  + tracerId + "," + responseStatusCSV + ")";
                    string methodName = "JCRAPI/Business/TracerMobileServices/GetTracerResponses";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }
        public List<ApiMobileTracerGetQuestionDetailReturnModel> GetTracerQuestionDetail(int? tracerCustomId, int? tracerQuestionId, int? tracerResponseId, int? userId)
        {
            List<ApiMobileTracerGetQuestionDetailReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {
                
                try
                {
                    _result = db.ApiMobileTracerGetQuestionDetail(tracerCustomId, tracerQuestionId, tracerResponseId, userId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileTracerGetQuestionDetail(" + tracerCustomId + "," + tracerQuestionId + "," + tracerResponseId + "," + userId + ")";
                    string methodName = "JCRAPI/Business/TracerMobileServices/GetTracerQuestionDetail";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName,userId, null, sqlParam, string.Empty);

                    return null;
                }
            }
            return _result;
        }
        public List<ApiMobileGetObservationDetailsByIdWithValidationMessageReturnModel> GetObservationDetailsByIdWithValidationMessage(int? tracerCustomID, int? ResponseId, bool? IsGuestAccess)
        {


            List<ApiMobileGetObservationDetailsByIdWithValidationMessageReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {
                
                try
                {
                    _result = db.ApiMobileGetObservationDetailsByIdWithValidationMessage(tracerCustomID, ResponseId, IsGuestAccess);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileGetObservationDetailsByIdWithValidationMessage(" + tracerCustomID + "," + ResponseId + "," + IsGuestAccess +")";
                    string methodName = "JCRAPI/Business/TracerMobileServices/GetObservationDetailsByIdWithValidationMessage";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }
            }
            return _result;
        }

        public List<ApiMobileSaveTracerResponseReturnModel> SaveTracerResponse(int? tracerId, int? observationId, string title, System.DateTime? observationDate, string medicalStaffInvolved, string staffInterviewed, int observationsCount, string surveyTeam, string departmentId, string location, int? observationStatusId, int? tracerErrorValue, string note, int? userId, string medicalRecordNumber, string equipmentObserved, string contractedService, bool? isCalledByGuestAccess)
        {
            List<ApiMobileSaveTracerResponseReturnModel> _result;

            surveyTeam = surveyTeam == null ? string.Empty : surveyTeam;
            medicalStaffInvolved = medicalStaffInvolved == null ? string.Empty : medicalStaffInvolved;
            staffInterviewed = staffInterviewed == null ? string.Empty : staffInterviewed;
            location = location == null ? string.Empty : location;
            medicalRecordNumber = medicalRecordNumber == null ? string.Empty : medicalRecordNumber;
            equipmentObserved = equipmentObserved == null ? string.Empty : equipmentObserved;
            contractedService = contractedService == null ? string.Empty : contractedService;
            //observationsCount = observationsCount < 1 ? 1 : observationsCount;
            if (observationDate == null)
            {
               
                string sqlParam = "ApiMobileSaveTracerResponse("  + tracerId + "," + observationId + "," + title + "," + observationDate + "," + medicalStaffInvolved + "," + staffInterviewed + "," + observationsCount.ToString() + "," + surveyTeam + "," + departmentId + "," + location + "," + observationStatusId + "," + tracerErrorValue + "," + note + "," + userId + "," + medicalRecordNumber + "," + equipmentObserved + "," + contractedService + "," + isCalledByGuestAccess + ")";
                string methodName = "JCRAPI/Business/TracerMobileServices/SaveTracerResponse";
                exceptionLog.ExceptionLogInsert("ObservationStartDate is Null", "", methodName, null, null, sqlParam, string.Empty);

                observationDate = DateTime.Today;

            }

            using (var db = new DBMEdition01Context())
            {

                if (observationId == null)
                    observationId = 0;
                try {
                    _result = db.ApiMobileSaveTracerResponse(tracerId, observationId, title, observationDate, medicalStaffInvolved, staffInterviewed, observationsCount, surveyTeam, departmentId, location, observationStatusId, tracerErrorValue, note, userId, medicalRecordNumber, equipmentObserved, contractedService, isCalledByGuestAccess);
                }               
                catch (Exception ex) {
                   
                    string sqlParam = "ApiMobileSaveTracerResponse(" + "," + tracerId + "," + observationId + "," + title + "," + observationDate + "," + medicalStaffInvolved + "," + staffInterviewed + "," + observationsCount.ToString() + ","  + surveyTeam + "," + departmentId + "," + location + "," + observationStatusId + "," + tracerErrorValue + "," + note + "," + userId + "," + medicalRecordNumber + "," + equipmentObserved + "," + contractedService + "," + isCalledByGuestAccess + ")";
                    string methodName = "JCRAPI/Business/TracerMobileServices/SaveTracerResponse";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }
            }
            return _result;
        }
        public List<ApiMobileUpdateTracerQuestionAnswerReturnModel> UpdateTracerQuestionAnswer(int? observationId, int? userId, int? tracerQuestionId, int? tracerQuestionAnswerId, decimal? numerator, decimal? denominator, int? questionAnswer, int? questionNoteId, string questionNote, bool? IsResponseRequired, out string rtnMessage)
        {
            List<ApiMobileUpdateTracerQuestionAnswerReturnModel> _result = new List<ApiMobileUpdateTracerQuestionAnswerReturnModel>();
            rtnMessage = string.Empty;
            bool IsNotApplicable = false;
            if (numerator == 0 && denominator == 0)
                IsNotApplicable = true;

            if (!IsNotApplicable)
            {
                if (denominator < 1)
                    rtnMessage = "Denominator must be greater than 0.";
                if (numerator > denominator)
                    rtnMessage = "Numerator cannot be greater than the denominator.";
                if (IsResponseRequired != null)
                {
                    if (IsResponseRequired == true && numerator == null && denominator == null)
                        rtnMessage = "Response to this question is required.";
                }

                _result = null;
            }

            if (rtnMessage == string.Empty)
            {

                using (var db = new DBMEdition01Context())
                {
                    try
                    {
                        _result = db.ApiMobileUpdateTracerQuestionAnswer(observationId, userId, tracerQuestionId, tracerQuestionAnswerId, numerator, denominator, questionAnswer, questionNoteId, questionNote);
                    }
                    catch (Exception ex)
                    {

                        string sqlParam = "ApiMobileUpdateTracerQuestionAnswer(" + observationId + "," + userId + "," + tracerQuestionId + "," + tracerQuestionAnswerId + "," + numerator + "," + denominator + "," + questionAnswer + "," + questionNoteId + "," + questionNote + ")";
                        string methodName = "JCRAPI/Business/TracerMobileServices/UpdateTracerQuestionAnswer";
                        exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                        return null;
                    }
                  
                }
            }

            return _result;
        }
        public bool SaveTracerResponseToNotApplicable(List<Api.Models.TracerQuestionAnswerGroup> groupData, int observationId, int userId)
        {
            bool rtn = false;

            List<ApiMobileUpdateTracerQuestionAnswerReturnModel> _rtnData;

            using (var db = new DBMEdition01Context())
            {

                foreach (var item in groupData)
                {
                    
                    try
                    {
                        _rtnData = db.ApiMobileUpdateTracerQuestionAnswer(observationId, userId, item.TracerQuestionId, item.TracerQuestionAnswerId, 0, 0, 11, null, null);
                        if (_rtnData == null)
                        {
                            rtn = false;
                            break;
                        }

                        else
                            rtn = true;
                    }
                    catch (Exception ex)
                    {

                        string sqlParam = "ApiMobileUpdateTracerQuestionAnswer(" + observationId + "," + userId + "," + item.TracerQuestionId + "," + item.TracerQuestionAnswerId + ", 0 , 0, 11 ,null,null)";
                        string methodName = "JCRAPI/Business/TracerMobileServices/SaveTracerResponseToNotApplicable";
                        exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                        return false;
                    }
                   
                }
                return rtn;
            }


        }
    }
}
