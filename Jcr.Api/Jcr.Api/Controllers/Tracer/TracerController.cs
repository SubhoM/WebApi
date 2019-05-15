using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Specialized;
using Microsoft.Reporting.WebForms;
using Jcr.Business;
using Jcr.Data;
using Jcr.Api.ActionFilters;
using Jcr.Api.Helpers;
using Jcr.Api.Models;
using Jcr.Api.Enumerators;
using Jcr.Api.Filters;
using System.Collections;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Jcr.Api.Models.Models;

namespace Jcr.Api.Controllers
{

    
    [System.Web.Http.RoutePrefix("TracerInfo")]
    public class TracerController : ApiController
    {
        protected TrackingRepository TRepository;
        protected TracerService tracerService;
        protected TracerMobileServices tracerMobileService;
        public TracerController()
        {
            tracerService = new TracerService();
            tracerMobileService = new TracerMobileServices();
            TRepository = new TrackingRepository();
        }
        /// <summary>
        /// Get PUBLISHED Tracer By SiteID & ProgramID
        /// </summary>
        /// 
        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("TracerDetail/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracerDetailBySite(int? siteId, int? programId, int? statusId = 1)
        {

            //  TracerServices service = new TracerServices();

            try
            {
                //  TRepository.AddTracking();
                var tracerDetail = tracerService.GetTracerDetail(siteId, programId, statusId);
                if (tracerDetail != null)
                    return Request.CreateResponse(HttpStatusCode.OK, tracerDetail);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tracer found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteID", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracerDetailBySite");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("GetTracerCategoryNames/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracerCategoryNames(int siteId, int programId)
        {

            //  TracerServices service = new TracerServices();

            try
            {
                var result = tracerService.GetTracerCategoryNames(siteId, programId);
                if (result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Category Names found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteID", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracerCategoryNames");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("GetDepartmentHierarchy/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDepartmentHierarchy(int? siteId, int? programId, int? rankId, bool? isCategoryActive, bool? isCategoryItemActive)
        {

            //  TracerServices service = new TracerServices();
            //if (isCategoryActive == null) isCategoryActive = true;
            //if (isCategoryItemActive == null) isCategoryItemActive = true;
            try
            {
                var result = tracerService.GetDepartmentHierarchy(siteId, programId, rankId, isCategoryActive, isCategoryItemActive);
                if (result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No GetDepartment Hierarchy found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteID", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetDepartmentHierarchy");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [AuthorizationRequired(ParameterName = "tracerId")]
        [VersionedRoute("TracerObservations/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracerObservations(int tracerId, string responseStatusCSV, int createdByUserID = 0)
        {

            if (responseStatusCSV == string.Empty)
                responseStatusCSV = "7,8";

            try
            {

                // TracerServices service = new TracerServices();
                List<ApiResponsesSelectByTracerIdReturnModel> _result;

                _result = tracerService.GetTracerResponses(tracerId, responseStatusCSV).Where(item => (createdByUserID == 0 || item.CreatedByID == createdByUserID)).ToList();
                
                if (_result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, _result);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tracer found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerId", tracerId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracerHeader");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        [AuthorizationRequired(ParameterName = "tracerId")]
        [VersionedRoute("MobileTracerObservations/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetMobileTracerObservations(int tracerId, string responseStatusCSV, int createdByUserID = 0)
        {

            if (responseStatusCSV == string.Empty)
                responseStatusCSV = "7,8";

            try
            {
                //  TracerMobileServices service = new TracerMobileServices();
                List<ApiMobileResponsesSelectByTracerIdReturnModel> _result;

                _result = tracerMobileService.GetTracerResponses(tracerId, responseStatusCSV, createdByUserID);

                if (_result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, _result);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tracer found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("MobileTracerId", tracerId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MobileTracerObservations");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("TracerHeader/{id?}", 1)]
        [System.Web.Http.HttpGet]
        //Get Tracer Detail
        public HttpResponseMessage GetTracerHeader(int tracerCustomId, int tracerResponseId, int siteId, int programId)
        {
            try
            {
                //  TracerServices service = new TracerServices();
                ApiGetTracerHeaderReturnModel tracerDetail = new ApiGetTracerHeaderReturnModel();

                tracerDetail = tracerService.GetTracerHeader(tracerCustomId, tracerResponseId, siteId, programId);

                if (tracerDetail != null)
                    return Request.CreateResponse(HttpStatusCode.OK, tracerDetail);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tracer found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerCustomId", tracerCustomId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracerHeader");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }


        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("MobileTracerHeader/{id?}", 1)]
        [System.Web.Http.HttpGet]
        //Get Tracer Detail
        public HttpResponseMessage GetMobileTracerHeader(int tracerCustomId, int tracerResponseId, int siteId, int programId)
        {
            try
            {
                //  TracerMobileServices service = new TracerMobileServices();

                ApiGetMobileTracerHeaderReturnModel tracerDetail = new ApiGetMobileTracerHeaderReturnModel();

                tracerDetail = tracerMobileService.GetTracerHeader(tracerCustomId, tracerResponseId, siteId, programId);

                if (tracerDetail != null)
                    return Request.CreateResponse(HttpStatusCode.OK, tracerDetail);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tracer found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerCustomId", tracerCustomId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracerHeader");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
        //  
        /// <summary>
        /// Get Observation Questions image info
        /// </summary>
        /// 
        [AuthorizationRequired]
        [VersionedRoute("GetQuestionAnswerImageInfo/{id?}", 1)]
        [System.Web.Http.HttpGet]
        //Get Tracer Detail
        public HttpResponseMessage GetQuestionAnswerImages(int? tracerQuestionId, int? tracerResponseId, int? userId)
        {
            try
            {

                //  TracerServices service = new TracerServices();
                List<ApiGetQuestionAnswerImagesReturnModel.ResultSetModel1> result;
                result = tracerService.GetQuestionAnswerImages(tracerQuestionId, tracerResponseId, userId);

                if (result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No image found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("tracerResponseId", tracerResponseId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetQuestionAnswerImages");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        [AuthorizationRequired(ParameterName = "tracerCustomId")]
        /// <summary>
        /// Get Observation Questions - List Of Questions
        /// </summary>
        [VersionedRoute("MobileQuestions/{id?}", 1)]
        [System.Web.Http.HttpGet]
        //Get Tracer Detail
        public HttpResponseMessage MobileGetObservationDetails(int tracerCustomId, int tracerResponseId, bool? IsGuestAccess)
        {
            try
            {

                //  TracerMobileServices service = new TracerMobileServices();
                var result = new List<ApiMobileGetObservationDetailsByIdReturnModel>();
                result = tracerMobileService.GetObservationDetailsById(tracerCustomId, tracerResponseId, IsGuestAccess);

                foreach(var question in result)
                {
                    question.QuestionText = question.QuestionText.Replace("<br>", @"
                                                            ");
                }
                if (result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tracer found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("tracerResponseId", tracerResponseId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MobileObservationDetails");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [AuthorizationRequired(ParameterName = "tracerCustomId")]
        [VersionedRoute("MobileQuestionsWithValidationMessage/{id?}", 1)]
        [System.Web.Http.HttpGet]
        //Get Tracer Detail
        public HttpResponseMessage MobileGetObservationDetailsWithValidationMessage(int tracerCustomId, int tracerResponseId, bool? IsGuestAccess)
        {
            try
            {

                //  TracerMobileServices service = new TracerMobileServices();
                var result = new List<ApiMobileGetObservationDetailsByIdWithValidationMessageReturnModel>();
                result = tracerMobileService.GetObservationDetailsByIdWithValidationMessage(tracerCustomId, tracerResponseId, IsGuestAccess);

                if (result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tracer question found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("tracerResponseId", tracerResponseId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/MobileQuestionsWithValidationMessage");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }
        /// <summary>
        /// Get Question Detail - QuestionText, IsResponseMandatory,QuestionNo,TracerQuestionAnswerID,Num,Den, IsNot Applicable, NoteID, Note         
        /// </summary>
        /// 
        [AuthorizationRequired(ParameterName = "tracerCustomId")]
        [VersionedRoute("MobileTracerQuestion/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetObservationQuestionDetail(int? tracerCustomId, int? tracerQuestionId, int? tracerResponseId, int? userId)
        {
            try
            {
                // TracerMobileServices service = new TracerMobileServices();
                List<ApiMobileTracerGetQuestionDetailReturnModel> _result;

                _result = tracerMobileService.GetTracerQuestionDetail(tracerCustomId, tracerQuestionId, tracerResponseId, userId);

                if (_result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, _result);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Question found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerCustomId", tracerCustomId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetObservationQuestionDetail");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [AuthorizationRequired]
        [VersionedRoute("GetTracerEULA/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracerEULA(int UserId)
        {
            try
            {
                ApiGetEulaStatusReturnModel isUserAcceptedEULA = new ApiGetEulaStatusReturnModel();
                string html;
                String _result = "";

                UserServices service = new UserServices();

                isUserAcceptedEULA = service.CheckEULAStatus(UserId);
                if (isUserAcceptedEULA != null)
                {
                    if (isUserAcceptedEULA.IsEulaAccepted == 0)
                    {
                        using (var reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(@"~\GuestAccess\_EULA_Mobile.cshtml")))
                        {
                            html = reader.ReadToEnd();
                        }
                        html = Regex.Replace(html, @"\r\n?|\n", "");

                        _result = "{ Html: '" + html + "'}";
                        if (_result == null)
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No EULA found");
                    }
                }


                var response = Request.CreateResponse(HttpStatusCode.OK, _result);
                response.Headers.Add("ForceResetPassword", isUserAcceptedEULA.ForceResetPassword);
                return response;

            }
            catch (Exception ex)
            {
                ex.Data.Add("EULA", "EULA load error");
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetObservationQuestionDetail");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [AuthorizationRequired]
        [VersionedRoute("MobileSaveObservation/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage MobileSaveObservation([FromBody] TracerObservation Observation)
        {
            //int? tracerId, int? observationId, string title, System.DateTime? observationDate, string medicalStaffInvolved, string staffInterviewed, string surveyTeam, string departmentId, string location, int? observationStatusId, int? tracerErrorValue, string note, int? userId, string medicalRecordNumber, string equipmentObserved, string contractedService, System.Data.DataTable gridData, bool? isCalledByGuestAccess, ;

            string logName = string.Empty;
            if (Observation == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }

            var t = Observation.TracerId.ToString();
            try
            {

                logName = "MobileTracerId";
                if (Observation.Note == null)
                    Observation.Note = string.Empty;


                List<ApiMobileSaveTracerResponseReturnModel> _result = new List<ApiMobileSaveTracerResponseReturnModel>();

                _result = tracerMobileService.SaveTracerResponse
                    (Observation.TracerId, Observation.ObservationId, Observation.Title, Observation.ObservationDate, Observation.MedicalStaffInvolved, Observation.StaffInterviewed, Observation.ObservationsCount, Observation.SurveyTeam, Observation.DepartmentId,
                    Observation.Location, Observation.ObservationStatusId, Observation.TracerErrorValue, Observation.Note, Observation.UserId, Observation.MedicalRecordNumber, Observation.EquipmentObserved, Observation.ContractedService, Observation.IsCalledByGuestAccess);

                if (_result != null)
                {

                    foreach (ApiMobileSaveTracerResponseReturnModel item in _result)
                    {
                        if (item.ObservationID == -1)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Observation Title already exists. Please change the title and Save.");
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, _result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Question found for this id");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("MobileTracerId", Observation.TracerId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MobileSaveObservation");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [AuthorizationRequired]
        [VersionedRoute("MobileGroupUpdateToNotApplicable/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage MobileGroupUpdateToNotApplicable([FromBody] List<TracerQuestionAnswerGroup> groupData, int ObservationId, int UserId)
        {
            //int? tracerId, int? observationId, string title, System.DateTime? observationDate, string medicalStaffInvolved, string staffInterviewed, string surveyTeam, string departmentId, string location, int? observationStatusId, int? tracerErrorValue, string note, int? userId, string medicalRecordNumber, string equipmentObserved, string contractedService, System.Data.DataTable gridData, bool? isCalledByGuestAccess, ;

            bool _rtn;

            if (groupData == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }

            try
            {
                _rtn = tracerMobileService.SaveTracerResponseToNotApplicable(groupData, ObservationId, UserId);
              
                if (_rtn)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Error during  update no response as Not Applicable.");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("UserId", UserId);
                ex.Data.Add("ObservationId", ObservationId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MobileGropuUpdateToNotApplicabler");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }



        [AuthorizationRequired]
        [VersionedRoute("MobileUpdateTracerResponseStatus/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage MobileUpdateTracerResponseStatus(int? tracerResponseId, int? responseStatusID)
        {
            try
            {
                int _result = 0;

                _result = tracerService.UpdateTracerStatus((int)tracerResponseId, (int)responseStatusID);

                if (_result != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Observation status updated");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error Updating Observation status");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerCustomID", tracerResponseId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MobileUpdateTracerResponseStatus");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error Updating Observation status");
            }
        }


        [AuthorizationRequired]
        [VersionedRoute("MobileUpdateQuestionAnswer/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage MobileUpdateTracerQuestionAnswers([FromBody] TracerQuestionAnswer Answer)
        {
            //int? tracerId, int? observationId, string title, System.DateTime? observationDate, string medicalStaffInvolved, string staffInterviewed, string surveyTeam, string departmentId, string location, int? observationStatusId, int? tracerErrorValue, string note, int? userId, string medicalRecordNumber, string equipmentObserved, string contractedService, System.Data.DataTable gridData, bool? isCalledByGuestAccess, ;

            string logName = string.Empty;
            if (Answer == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }

            try
            {

                logName = "MobileTracerId";
                //  TracerMobileServices service = new TracerMobileServices();
                List<ApiMobileUpdateTracerQuestionAnswerReturnModel> _result = new List<ApiMobileUpdateTracerQuestionAnswerReturnModel>();
                string errMsg;
                _result = tracerMobileService.UpdateTracerQuestionAnswer(Answer.ObservationId, Answer.UserId, Answer.TracerQuestionId, Answer.TracerQuestionAnswerId, Answer.Numerator, Answer.Denominator, Answer.QuestionAnswer, Answer.QuestionNoteID, Answer.QuestionNote, Answer.IsResponseRequired,  out errMsg);

                if (errMsg.Length > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
                }
                if (_result != null)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, _result);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No Question found for this id");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("ObservationId", Answer.ObservationId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MobileUpdateQuestionAnswer");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [AuthorizationRequired]
        [VersionedRoute("DeleteObservation/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteObservation(int tracerResponseId, int updatedByID)
        {
            try
            {
                //  TracerServices service = new TracerServices();
                int _result;

                _result = tracerService.DeleteObservation(tracerResponseId, updatedByID);

                if (_result != 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Observation deleted");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error on Delete Observation");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerCustomID", tracerResponseId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/DeleteObservation");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }





        [AuthorizationRequired(ParameterName = "tracerID")]
        [VersionedRoute("PublishTracer/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage PublishTracer(int tracerID, int updatedByID)
        {
            try
            {
                // TracerServices service = new TracerServices();
                int _result;

                _result = tracerService.PublishTracer(tracerID, updatedByID);

                if (_result != 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Tracer Published");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This tracer cannot be published because it contains no questions");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/PublishTracer");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }



        [AuthorizationRequired(ParameterName = "tracerID")]
        [VersionedRoute("UnpblishTracer/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UnpublishTracer(int tracerID, int updatedByID)
        {
            try
            {
                // TracerServices service = new TracerServices();
                int _result;

                _result = tracerService.UnpublishTracer(tracerID, updatedByID);

                if (_result != 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Tracer Unpublished");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tracer Unpblish failed");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/UnpublishTracer");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
        
        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("SendTracerEmail/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SendTracerEmail([FromBody] Email email, string AttachmentName, int userId, int siteId, int? programId, int TracerId, string siteName, string fullName, string programName, int pIsGuestUser = 0, int pIsErrorOnly = 0, string formType = "emptyform")
        {
            bool emailSuccess = true;

            var customSections = 0;
            int? detailCustomSections = 0;
            int actionTypeId = (int)Enums.ActionTypeEnum.TracersEmailed;//34
            try
            {
                byte[] fileContents = null;
                EmailServices service = new EmailServices();

                if (formType == "emptyform")
                {
                    fileContents = service.TracerEmptyPrintForm(TracerId, ref customSections, userId, siteId, programId, siteName, programName, pIsGuestUser, formType);

                }
                else
                {
                    fileContents = service.ObservationPrintForm(TracerId, ref detailCustomSections, userId, siteId, programId, siteName, programName, pIsErrorOnly, pIsGuestUser, formType);

                }

                // email.AttachmentLocation[0] = service.SavePDF(AttachmentName, fileContents);

                emailSuccess = service.SendEmailAttachemnt(email, actionTypeId, fileContents, userId, siteId, siteName, fullName, AttachmentName);

                if (!emailSuccess)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email sending failed");
                    // emailMessage = WebConstants.Email_Failed;

                }
                return Request.CreateResponse(HttpStatusCode.OK, "Email sent");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email sending failed " + ex.Message.ToString());
                //  emailMessage = WebConstants.Email_Failed;
            }

            // return Json(emailMessage);

        }

        /// <summary>
        /// Send Tracer Email
        /// </summary>
        [AuthorizationRequired]
        [VersionedRoute("SendTracerCustomerSupportEmail/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SendTracerCustomerSupportEmail([FromBody] CustomerSupport customerSupportEmail)
        {

            try
            {

                CustomerSupportEmailServices service = new CustomerSupportEmailServices();
                service.TracerMobileCustomerSupportEmail(customerSupportEmail);
                return Request.CreateResponse(HttpStatusCode.OK, "Tracer Mobile Customer Support Email sent");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tracer Mobile Customer Support Email sending failed " + ex.Message.ToString());
                //  emailMessage = WebConstants.Email_Failed;
            }

            // return Json(emailMessage);

        }


        [AuthorizationRequired]
        [VersionedRoute("get/guestlinkaccessed", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GuestLinkAccessed(int userId, Guid guestLinkGuid)
        {

            // TracerServices service = new TracerServices();
            try
            {
                tracerService.InsertGuestLinkAccessed(userId, guestLinkGuid);

                return Request.CreateResponse(HttpStatusCode.OK, "Success");

            }
            catch (Exception ex)
            {
                ex.Data.Add("GuestLinkAccessed", guestLinkGuid);
                ex.Data.Add("HTTPReferrer", "JCRAPI/post/guestlinkaccessed");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }


        [AuthorizationRequired]
        [VersionedRoute("GetUsersForBrowsing/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetUsersForBrowsing(string searchString, string siteList)
        {            
            try
            {
                var _usrList = tracerService.GetUsersForBrowsing(searchString, siteList);

             

                return Request.CreateResponse(HttpStatusCode.OK, _usrList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetUsersForBrowsing", siteList);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetUsersForBrowsing");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }


        [AuthorizationRequired]
        [VersionedRoute("GetUsersDetails/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetUsersDetails(string lstUserIDs, int siteID)
        {            
            try
            {
                var _usrList = tracerService.GetUsersDetails(lstUserIDs, siteID);                

                return Request.CreateResponse(HttpStatusCode.OK, _usrList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetUsersDetails", lstUserIDs);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetUsersDetails");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }


        [AuthorizationRequired]
        [VersionedRoute("CreateGuestUserByEmailIds/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage CreateGuestUserByEmailIds([FromBody] GuestUser gusr)
        {
            try
            {
                var _usrList = tracerService.CreateGuestUserByEmailIds(gusr.lstNewUserEmails,  gusr.siteID, gusr.updateByID);

                return Request.CreateResponse(HttpStatusCode.OK, _usrList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetUsersDetails", gusr.lstNewUserEmails);
                ex.Data.Add("Site", gusr.siteID.ToString());
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/CreateGuestUserByEmailIds");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }

        [AuthorizationRequired]
        [VersionedRoute("GetInactiveUserEmailIds/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetInactiveUserEmailIds(string lstEmailIds)
        {
            try
            {
                var _usrList = tracerService.GetInactiveUserEmailIds(lstEmailIds);

                return Request.CreateResponse(HttpStatusCode.OK, _usrList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetInactiveUserEmailIds", lstEmailIds);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetInactiveUserEmailIds");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }

		[AuthorizationRequired(ParameterName = "tracerCustomId")]
		[VersionedRoute("GetMobileTracerQuestionStdDetailsInfo/{id?}", 1)]
		[System.Web.Http.HttpGet]
		public HttpResponseMessage GetMobileTracerQuestionStdDetailsInfo(int? tracerCustomId, int? tracerResponseId, int? tracerQuestionId, bool? isGuestAccess = false)
		{
			try
			{
				var _quesInfo = TracerService.GetTracerQuestionInfo(tracerCustomId, tracerQuestionId);
				var _stdList = TracerService.GetStdDetailsByTracerQuestion(tracerCustomId, tracerResponseId, tracerQuestionId, isGuestAccess);

				return Request.CreateResponse(HttpStatusCode.OK, new { TracerQuestion = _quesInfo, StandardDetails = _stdList });

			}
			catch (Exception ex)
			{
				ex.Data.Add("GetStdDetailsByTracerQuestion", tracerQuestionId);
				ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetMobileTracerQuestionStdDetailsInfo");
				WebExceptionHelper.LogException(ex, null);
				return null;
			}

		}

		[AuthorizationRequired(ParameterName = "tracerCustomId")]
        [VersionedRoute("GetTracerQuestionInfo/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracerQuestionInfo(int? tracerCustomId, int? tracerQuestionId)
        {
            try
            {
                var _quesInfo = TracerService.GetTracerQuestionInfo(tracerCustomId, tracerQuestionId);

                return Request.CreateResponse(HttpStatusCode.OK, _quesInfo);

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetTracerQuestionInfo", tracerQuestionId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracerQuestionInfo");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }


        [AuthorizationRequired(ParameterName = "tracerCustomId")]
        [VersionedRoute("GetStdDetailsByTracerQuestion/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetStdDetailsByTracerQuestion(int? tracerCustomId, int? tracerResponseId, int? tracerQuestionId, bool? isGuestAccess = false)
        {
            try
            {
                var _stdList = TracerService.GetStdDetailsByTracerQuestion(tracerCustomId, tracerResponseId, tracerQuestionId, isGuestAccess);

                return Request.CreateResponse(HttpStatusCode.OK, _stdList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetStdDetailsByTracerQuestion", tracerQuestionId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetStdDetailsByTracerQuestion");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }

        [AuthorizationRequired(ParameterName="siteID")]
        [VersionedRoute("GetTracersBySiteProgramStatus/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracersBySiteProgramStatus(int? programID, int? siteID, int? statusID, int? userID) {
            // Added by Mark Orlando 01/24/2018 for System Tracers Project. This API will be called by Tracers desktop when the 
            // home page gets loaded or refreshed. 
            try {
                var service = new TracerService();
                var _tracerList = service.GetTracersBySiteProgramStatus(programID, siteID, statusID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("UserID", userID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracersBySiteProgramStatus");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired(ParameterName = "userID")]
        [VersionedRoute("GetTracersThatCanBeCopied/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracersThatCanBeCopied(int siteID, int programID, int userID) {
            // Added by Mark Orlando 01/26/2018 for System Tracers Project. This API will be called by Tracers desktop when the 
            // MultiSite Management page gets loaded or refreshed. 
            try {
                var service = new TracerService();
                var _tracerList = service.GetTracersThatCanBeCopied(siteID, programID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("SiteID", siteID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracersThatCanBeCopied");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired(ParameterName = "siteID")]
        [VersionedRoute("GetTracersThatCanBeDeleted/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracersThatCanBeDeleted(int siteID, int programID, int userID) {
            // Added by Mark Orlando 01/26/2018 for System Tracers Project. This API will be called by Tracers desktop when the 
            // MultiSite Management page and selects delete option from the dropdown. 
            try {
                var service = new TracerService();
                var _tracerList = service.GetTracersThatCanBeDeleted(siteID, programID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("SiteID", siteID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracersThatCanBeDeleted");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired(ParameterName = "tracerID")]
        [VersionedRoute("GetTracerQuestions/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracerQuestions(int tracerID) {
            // Added by Mark Orlando 01/26/2018 for System Tracers Project. This API will be called by Tracers desktop when the 
            // MultiSite Management page and selects delete option from the dropdown. 
            try {
                var service = new TracerService();
                var _tracerList = service.GetTracerQuestions(tracerID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTracerQuestions");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired(ParameterName = "tracerID")]
        [VersionedRoute("GetTargetSitesForCopy/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTargetSitesForCopy(int tracerID, int userID) {
            // Added by Mark Orlando 01/01/2018 for System Tracers Project. This API is called by Tracers desktop from the 
            // MultiSite Management page when developer needs to populate list of sites where tracer can be copied.
            try {
                var service = new TracerService();
                var _tracerList = service.GetTargetSitesForCopy(tracerID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTargetSitesForCopy");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired(ParameterName = "tracerID")]
        [VersionedRoute("GetTargetSitesForDelete/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTargetSitesForDelete(int tracerID, int userID) {
            // Added by Mark Orlando 01/01/2018 for System Tracers Project. This API is called by Tracers desktop from the 
            // MultiSite Management page when developer needs to populate list of sites where child can be copied.
            try {
                var service = new TracerService();
                var _tracerList = service.GetTargetSitesForDelete(tracerID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetTargetSitesForDelete");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired(ParameterName = "userID")]
        [VersionedRoute("SitesNoLongerAuthorizedToAccess/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage SitesNoLongerAuthorizedToAccess(int tracerID, int userID) {
            // Added by Mark Orlando for System Tracers Project. This API is called by Tracers desktop from the 
            // MultiSite Management page when customer selects parent tracer for which the admin no longer has 
            // program admin access to all child sites.
            try {
                var service = new TracerService();
                var _tracerList = service.SitesNoLongerAuthorizedToAccess(tracerID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/SitesNoLongerAuthorizedToAccess");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired(ParameterName = "userID")]
        [VersionedRoute("OptOutOfSystemTracers/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage OptOutOfSystemTracers(int tracerID, int userID) {
            // Added by Mark Orlando for System Tracers Project. This API is called by Tracers desktop from the 
            // MultiSite Management page. If customer selects parent tracer for which the admin no longer has 
            // program admin access to all child sites he gets a prompt asking if he wants tracers in one or more
            // sites removed from the group. If he choses to "opt-out" this API is called.
            try {
                var service = new TracerService();
                int result = service.OptOutOfSystemTracers(tracerID, userID);

                if (result != 1) {
                    return Request.CreateResponse(HttpStatusCode.OK, "Opt-out Successful");
                }
                else {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error on OptOutOfSystemTracers");
                }
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/OptOutOfSystemTracers");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("MultisiteCopy/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage MultisiteCopy([FromBody]Jcr.Api.Models.Models.MultisiteCopy uMultisiteCopy) {
            // Added by Mark Orlando for System Tracers Project. This API is called by Tracers desktop from the 
            // MultiSite Management page when customer clicks the button to copy tracers from a selected parent
            // tracer (tracerId)
            try {
                var service = new TracerService();
                List <ApiMultisiteCopyReturnModel> _result = service.MultisiteCopy(uMultisiteCopy.SelectedSites, 
                    uMultisiteCopy.TracerID, uMultisiteCopy.IsLocked, uMultisiteCopy.UserID);

                string _statusMsg = _result[0].StatusMsg;

                return Request.CreateResponse(HttpStatusCode.OK, _statusMsg);
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", uMultisiteCopy.TracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MultisiteCopy");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("MultisiteDelete/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage MultisiteDelete([FromBody]Jcr.Api.Models.Models.MultisiteDelete uMultisiteDelete) {
            // Added by Mark Orlando for System Tracers Project. This API is called by Tracers desktop from the 
            // MultiSite Management page when customer clicks the button to delete tracers

            bool isDeadlock = false;
            try {
                var service = new TracerService();
                List<ApiMultisiteDeleteReturnModel> _result = service.MultisiteDelete(uMultisiteDelete.SelectedSites,
                    uMultisiteDelete.TracerID, uMultisiteDelete.UserID, out isDeadlock);

                string _statusMsg = _result[0].StatusMsg;

                if (isDeadlock) {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The system is busy and your changes were not saved. Please try again.");                    
                } else {
                    return Request.CreateResponse(HttpStatusCode.OK, _statusMsg);
                }
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", uMultisiteDelete.TracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/MultisiteDelete");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("UnlockedMasterValidation/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage UnlockedMasterValidation(int tracerID, int userID) {
            // Added by Mark Orlando for System Tracers Project. This API is called by Tracers desktop from the 
            // MultiSite Copy Wizard when customer selects unlocked tracer.
            try {
                var service = new TracerService();
                var _tracerList = service.UnlockedMasterValidation(tracerID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _tracerList);
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/UnlockedMasterValidation");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("ChangeMasterSite/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage ChangeMasterSite([FromBody]Jcr.Api.Models.Models.ChangeMasterSite uChangeMasterSite) {
            // Added by Mark Orlando for System Tracers Project. This API is called by Tracers desktop from the 
            // Home Page when user clicks [Locked] or [Unlocked] icon for master tracer.
            try {
                var service = new TracerService();
                var _siteList = service.ChangeMasterSite(uChangeMasterSite.TracerID, uChangeMasterSite.SiteID, uChangeMasterSite.UserID);

                if (_siteList != null) {
                    MultisiteUserMsg userMsg = new MultisiteUserMsg();
                    userMsg.UserMsg = _siteList[0].UserMsg;
                    return Request.CreateResponse(HttpStatusCode.OK, userMsg);
                }
                else {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to change master to requested site.");
                }
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", uChangeMasterSite.TracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/ChangeMasterSite");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("GetSystemTracerInfo/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetSystemTracerInfo(int tracerID, int userID) {
            // Added by Mark Orlando for System Tracers Project. This API is called by Tracers desktop from the 
            // Tracers Home Page when user clicks the [Lock or [Unlock] icon.
            try {
                var service = new TracerService();
                var _siteList = service.GetSystemTracerInfo(tracerID, userID);

                return Request.CreateResponse(HttpStatusCode.OK, _siteList);
            }
            catch (Exception ex) {
                ex.Data.Add("TracerID", tracerID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetSystemTracerInfo");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

		[AuthorizationRequired]
		[VersionedRoute("UpdateTracerCategoryForTracer/{id?}", 1)]
		[System.Web.Http.HttpPost]
		public HttpResponseMessage UpdateTracerCategoryForTracer(int tracerCategoryId, int tracerId, int updatedById)
		{
			try
			{
				var service = new TracerService();
				var result = service.UpdateTracerCategoryForTracer(tracerCategoryId, tracerId, updatedById);

				return Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (Exception ex)
			{
				ex.Data.Add("tracerId", tracerId);
				ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/UpdateTracerCategoryForTracer");
				WebExceptionHelper.LogException(ex, null);
				return null;
			}
		}

	}
}
