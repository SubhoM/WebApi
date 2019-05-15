using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jcr.Business;
using Jcr.Data;
using Jcr.Api.Helpers;
using Jcr.Api.ActionFilters;
using Jcr.Api.Models;
using System.Web.Http.Cors;
using Jcr.Business.Task;
using Jcr.Api.Models.Models;
using static Jcr.Api.Enumerators.Enums;
using System.Threading.Tasks;
using Jcr.Tracers.Model;

namespace Jcr.Api.Controllers
{


    [AuthorizationRequired]
    [RoutePrefix("TaskInfo")]
    public class TaskController : ApiController
    {

        protected TracerService tracerService;

        public TaskController()
        {
            tracerService = new TracerService();
        }

        /// <summary>
        /// Get List of Tasks
        /// </summary>
        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("GetTaskList/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTaskList(string standardEffDate, int? siteId, int? programId, int? assignedToUserId, int? assignedByUserId)
        {
            try
            {
                var taskList = TaskServices.GetTaskList(standardEffDate, siteId, programId, assignedToUserId, assignedByUserId);

                if (taskList != null)
                {
                    taskList.ForEach(m =>
                    {
                        m.TaskName = (m.TaskName.Length > 25 ? m.TaskName.Substring(0, 24) + "...." : m.TaskName);
                        m.TracerName = (m.TracerName.Trim().EndsWith("-") ? m.TracerName.Trim().Replace("-", "") : m.TracerName);
                        m.Std = m.TaskTypeID == 5 & m.TracerResponseID > 0 ? TracerService.GetAllsStdsByTracerQuestion(m.TracerCustomID, m.TracerResponseID, m.TracerQuestionID, false) : m.Std;
                    });

                    return Request.CreateResponse(HttpStatusCode.OK, taskList);

                }

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tasks Found");
            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteID", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetTaskList");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Delete Tasks
        /// </summary>
        [VersionedRoute("DeleteTasks/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage DeleteTasks([FromBody] string taskIds)
        {
            try
            {
                TaskServices.DeleteTasks(taskIds);

                return Request.CreateResponse(HttpStatusCode.OK);


            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/DeleteTasks");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Save Task Landing Page Grid Filters
        /// </summary>
        [VersionedRoute("SaveTaskGridFilters/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage SaveTaskGridFilters([FromBody] UserPreference userPref)
        {
            try
            {
                userPref.preferenceType = "TaskFilter";

                UserServices.UpdateUserPreference(userPref);

                return Request.CreateResponse(HttpStatusCode.OK);


            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/SaveTaskGridFilters");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }


        [VersionedRoute("GetTaskGridFilters/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTaskGridFilters(int userID, int siteID, int programID)
        {
            try
            {
                var TaskUserFilter = UserServices.GetUserPreference(userID, siteID, programID, "TaskFilter");

                return Request.CreateResponse(HttpStatusCode.OK, TaskUserFilter);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetTaskGridFilters");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetTracerList/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTracerList(int siteID, int programID, int statusID)
        {
            try
            {
                var TracersList = TaskServices.GetTracerList(siteID, programID, statusID);

                return Request.CreateResponse(HttpStatusCode.OK, TracersList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetTracerList");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetChapterList/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetChapterList(int? siteId, int? programId, int? userId, int? certificationItemId, string standardEffBeginDate, int? serviceProfileTypeId)
        {
            try
            {
                var ChapterList = TaskServices.GetChapterList(siteId, programId, userId, certificationItemId, standardEffBeginDate, serviceProfileTypeId);

                return Request.CreateResponse(HttpStatusCode.OK, ChapterList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetChapterList");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetStandardByChapterList/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetStandardByChapterList(string standardEffBeginDate, int? productType, int? programId, int? chapterId, int? siteId, int? userId, int? serviceProfileTypeId, int? certificationItemId)
        {
            try
            {
                var StandardList = TaskServices.GetStandardByChapterList(standardEffBeginDate, productType, programId, chapterId, siteId, userId, serviceProfileTypeId, certificationItemId);

                return Request.CreateResponse(HttpStatusCode.OK, StandardList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetStandardByChapterList");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetChapterStandardByEPTextID/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetChapterStandardByEPTextID(string standardEffBeginDate, int epTextID, int programID)
        {
            try
            {
                var ChapterStandard = TaskServices.GetChapterStandardByEPTextID(standardEffBeginDate, epTextID, programID);

                return Request.CreateResponse(HttpStatusCode.OK, ChapterStandard);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetChapterStandardByEPTextID");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetEPsByStandard/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetEPsByStandard(int userId, int productType, int siteId, int programId, int standardTextId, string standardEffBeginDate, int serviceProfileTypeId, int certificationItemId)
        {
            try
            {
                var EPsByStandard = TaskServices.GetEPsByStandard(userId, productType, siteId, programId, standardTextId, standardEffBeginDate, serviceProfileTypeId, certificationItemId);

                var _result = EPsByStandard.Select(m => new
                {
                    EPTextID = m.EPTextID,
                    EPText = m.EPText,
                    EPTextShortened = m.EPText.Length > 150 ? m.EPText.Substring(0, 150) + "..." : m.EPText,
                    SortOrder = m.SortOrder
                });

                return Request.CreateResponse(HttpStatusCode.OK, _result);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetEPsByStandard");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetCOPList/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetCOPList(int? siteId, int? programId)
        {
            try
            {
                var COPList = TaskServices.GetCOPList(siteId, programId);

                return Request.CreateResponse(HttpStatusCode.OK, COPList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetCOPList");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetTagsByCopAndProgramID/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTagsByCopAndProgramID(int? programId, string copName)
        {
            try
            {
                var TagList = TaskServices.GetTagsByCopAndProgramID(programId, copName);

                return Request.CreateResponse(HttpStatusCode.OK, TagList);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetTagsByCopAndProgramID");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }


        [VersionedRoute("GetElementsByTag/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetElementsByTag(int? programId, string tagCode)
        {
            try
            {
                var COPList = TaskServices.GetElementsByCopId(programId, tagCode);

                var _result = COPList.Select(m => new
                {
                    CmsStandardID = m.CmsStandardID,
                    TagCode = m.TagCode,
                    JcrElementText = m.JcrElementText.Replace("<br/>", ""),
                    JcrElementTextShortened = m.JcrElementText.Length > 150 ? m.JcrElementText.Substring(0, 150) + "..." : m.JcrElementText,
                    ElementSortOrder = m.ElementSortOrder
                });

                return Request.CreateResponse(HttpStatusCode.OK, _result);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetElementsByTag");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetCopTagByCmsStandardId/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetCopTagByCmsStandardId(int? programId, int? cmsStandardId)
        {
            try
            {
                var _result = TaskServices.GetCopTagByCmsStandardId(programId, cmsStandardId);

                return Request.CreateResponse(HttpStatusCode.OK, _result);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetCopTagByCmsStandardId");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("GetTracerById/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTracerById(int? tracerCustomId)
        {
            try
            {
                var _result = TaskServices.GetTracerById(tracerCustomId);

                return Request.CreateResponse(HttpStatusCode.OK, _result);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetTracerById");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }

        [VersionedRoute("MobileUpdateUserTask/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage MobileUpdateUserTask([FromBody] UserTask uTask)
        {
            try
            {
                if (uTask != null && !string.IsNullOrEmpty(uTask.LstUsers) && uTask.LstUsers.Length > 0)
                {
					bool isEditTask = uTask.TaskID > 0;

					if (isEditTask && uTask.LstUsers.Split(',').Length > 1)
						return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Task can be reassigned to only one individual.");

					var _usrList = tracerService.CreateGuestUserByEmailIds(uTask.LstUsers, uTask.SiteID, uTask.UpdatedByUserID);
                    var _ccList = new List<AssignedUser>();

                    if (!string.IsNullOrEmpty(uTask.CCUserIDs) && uTask.CCUserIDs.Length > 0)
                        _ccList = tracerService.CreateGuestUserByEmailIds(uTask.CCUserIDs, uTask.SiteID, uTask.UpdatedByUserID);

                    uTask.LstUsers = string.Join(",", _usrList.Select(x => x.UserId.ToString()));
                    uTask.CCUserIDs = string.Join(",", _ccList.Select(x => x.UserId.ToString()));

					if (isEditTask)
						uTask.AssignedToUserID = Convert.ToInt32(uTask.LstUsers);

                    return UpdateUserTask(uTask);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "No valid Task Assignee given.");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/MobileUpdateUserTask");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.ToString());
            }
        }


        [VersionedRoute("UpdateUserTask/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage UpdateUserTask([FromBody] UserTask uTask)
        {
            try
            {

                var lstTaskPreviousData = new List<UserTaskEmail>();

                if (uTask.TaskID > 0)
                    lstTaskPreviousData = TaskServices.GetTaskDetailsForSendingEmail(uTask.TaskID.ToString(), uTask.SiteID, uTask.ProgramID, uTask.UpdatedByUserID, true);

                var lstTask = TaskServices.UpdateUserTask(uTask.TaskID, uTask.TaskName, uTask.TaskTypeID, uTask.SiteID,
                   uTask.ProgramID, uTask.AssignedDate, uTask.AssignedToUserID, uTask.AssignedByUserID,
                   uTask.TaskDetails, uTask.CCUserIDs, uTask.DueDate, uTask.TaskStatus, uTask.UpdatedByUserID, uTask.CreatedByUserID,
                   uTask.TracerCustomID, uTask.TracerResponseID, uTask.TracerQuestionID, uTask.TracerQuestionAnswerID, uTask.EPTextID,
                   uTask.CMSStandardID, uTask.ReminderEmailRequired, uTask.TaskResolution, uTask.CompleteDate, uTask.LstUsers);

                Task.Factory.StartNew(() => TaskServices.SendTaskEmailAfterSave(lstTask, uTask.SiteID, uTask.ProgramID, uTask.UpdatedByUserID, lstTaskPreviousData, uTask.ObservationTitle));

                if (uTask.TaskID > 0)
                    Task.Factory.StartNew(() => TaskServices.DisableTaskNotificationScheduleType(DisableTaskNotificationScheduleType.TaskChanged, uTask.SiteID, uTask.UpdatedByUserID, lstTask));

                return Request.CreateResponse(HttpStatusCode.OK, lstTask);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/UpdateUserTask");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.ToString());
            }
        }

        [VersionedRoute("GetTaskDetails/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTaskDetails(int? taskId, string standardEffBeginDate, int? programId)
        {
            try
            {
                var TaskDetails = TaskServices.GetTaskDetails(taskId, standardEffBeginDate, programId);

                return Request.CreateResponse(HttpStatusCode.OK, TaskDetails);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetTaskDetails");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
        }

        [VersionedRoute("GetMaxTaskAssignedDate/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetMaxTaskAssignedDate(string taskIds)
        {
            try
            {
                var MaxAssignedDate = TaskServices.GetMaxTaskAssignedDate(taskIds);

                return Request.CreateResponse(HttpStatusCode.OK, MaxAssignedDate);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetMaxTaskAssignedDate");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
        }

        [VersionedRoute("ReAssignTask/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage ReAssignTask([FromBody] ReAssignTasks raTasks)
        {
            try
            {

                var lstPreviousTasks = TaskServices.GetTaskDetailsForSendingEmail(raTasks.LstTaskIDs.ToString(), raTasks.SiteID, raTasks.ProgramID, raTasks.UpdatedByUserID);

                TaskServices.ReAssignTask(raTasks);

                Task.Factory.StartNew(() => TaskServices.SendTaskReassignedEmail(raTasks.LstTaskIDs, raTasks.SiteID, raTasks.ProgramID, raTasks.UpdatedByUserID, lstPreviousTasks));

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/ReAssignTask");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
        }

        /// <summary>
        /// Get List of AMP Tasks
        /// </summary>
        [VersionedRoute("GetAMPTaskListByEPTextID/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetAMPTaskListByEPTextID(string standardEffDate, int siteId, int programId, int epTextId)
        {
            try
            {
                var taskList = TaskServices.GetAMPTaskListByEPTextID(standardEffDate, siteId, programId, epTextId);

                if (taskList != null)
                    return Request.CreateResponse(HttpStatusCode.OK, taskList);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tasks Found");
            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteID", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetAMPTaskListByEPTextID");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Get List of CMS Tasks
        /// </summary>
        [VersionedRoute("GetCMSTaskListByCMSStandardID/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetCMSTaskListByCMSStandardID(string standardEffDate, int siteId, int programId, int cmsStandardId)
        {
            try
            {
                var taskList = TaskServices.GetCMSTaskListByCMSStandardID(standardEffDate, siteId, programId, cmsStandardId);

                if (taskList != null)
                    return Request.CreateResponse(HttpStatusCode.OK, taskList);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Tasks Found");
            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteID", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetCMSTaskListByCMSStandardID");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Associate Tasks to Tracer Observation
        /// </summary>
        [VersionedRoute("UpdateTaskObservationAssoc/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage UpdateTaskObservationAssoc([FromBody] TaskObservationAssociation tskObsObj)
        {
            try
            {
                TaskServices.UpdateTaskObservationAssoc(tskObsObj.TracerResponseId, tskObsObj.TaskIDs);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ex.Data.Add("tracerResponseId", tskObsObj.TracerResponseId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/UpdateTaskObservationAssoc");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Get Site Email Notification settings
        /// </summary>
        [VersionedRoute("GetSiteEmailNotificationSettings/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetSiteEmailNotificationSettings(int siteId)
        {
            try
            {
                var siteEmailSettings = TaskServices.GetSiteEmailNotificationSettings(siteId);

                if (siteEmailSettings != null)
                    return Request.CreateResponse(HttpStatusCode.OK, siteEmailSettings);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found");
            }
            catch (Exception ex)
            {
                ex.Data.Add("siteId", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetSiteEmailNotificationSettings");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Update Site Email Notification Settings
        /// </summary>
        [VersionedRoute("UpdateSiteEmailNotificationSettings/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage UpdateSiteEmailNotificationSettings([FromBody]SiteEmailNotificationSetting siteEmailNotificationSetting)
        {
            try
            {
                TaskServices.UpdateSiteEmailNotificationSettings(siteEmailNotificationSetting.SiteID, siteEmailNotificationSetting.SendEmailOnTaskCreation, siteEmailNotificationSetting.SendEmailBeforeTaskDue, siteEmailNotificationSetting.DaysBeforeTaskDue, siteEmailNotificationSetting.SendEmailsAfterTaskDue, siteEmailNotificationSetting.DaysAfterTaskDue
                    , siteEmailNotificationSetting.SendRemainderEmailAfterTaskDue, siteEmailNotificationSetting.SendTaskReportToCC, siteEmailNotificationSetting.TaskReportToCCScheduleTypeID, siteEmailNotificationSetting.SendTaskReportToUsers, siteEmailNotificationSetting.TaskReportToUsersScheduleTypeID, siteEmailNotificationSetting.TaskReportRecipients
                    , siteEmailNotificationSetting.SendEmailOnAssigningEP, siteEmailNotificationSetting.TaskDueRecipientType, siteEmailNotificationSetting.UpdatedBy);

                Task.Factory.StartNew(() => TaskServices.DisableTaskNotificationScheduleType(DisableTaskNotificationScheduleType.EmailSetting, siteEmailNotificationSetting.SiteID, siteEmailNotificationSetting.UpdatedBy));

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ex.Data.Add("siteID", siteEmailNotificationSetting.SiteID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/UpdateSiteEmailNotificationSettings");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Get Task follow up remainder settings
        /// </summary>
        [VersionedRoute("GetTaskFollowUpRemainderSettings/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTaskFollowUpRemainderSettings(int siteId)
        {
            try
            {
                var taskfollowupRemainderSettings = TaskServices.GetTaskFollowUpRemainderSettings(siteId);

                if (taskfollowupRemainderSettings != null)
                    return Request.CreateResponse(HttpStatusCode.OK, taskfollowupRemainderSettings);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found");
            }
            catch (Exception ex)
            {
                ex.Data.Add("siteId", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/GetTaskFollowUpRemainderSettings");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        [VersionedRoute("ResendTaskEmail/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage ResendTaskEmail([FromBody] ResendTaskEmail resendTaskEmail)
        {
            try
            {

                Task.Factory.StartNew(() => TaskServices.ResendTaskEmail(resendTaskEmail.TaskIDs, resendTaskEmail.SiteID, resendTaskEmail.ProgramID, resendTaskEmail.UserID));

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/TaskInfo/ResendTaskEmail");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
        }


    }
}
