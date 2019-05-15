using Jcr.Api.Models.Models;
using Jcr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jcr.Api.Enumerators;
using Jcr.Api.Helpers;
using Jcr.Api.Models;
using System.Configuration;
using Jcr.Tracers.Model;

namespace Jcr.Business.Task
{
    public class TaskServices
    {

        public static List<ApiGetTaskListReturnModel> GetTaskList(string standardEffDate, int? siteId, int? programId, int? assignedToUserId, int? assignedByUserId)
        {
            List<ApiGetTaskListReturnModel> _result;
            using (var db = new Data.DBMEdition01Context())
            {
                _result = db.ApiGetTaskList(standardEffDate, siteId, programId, assignedToUserId, assignedByUserId);
            }
            return _result;
        }

        public static void DeleteTasks(string taskIDs)
        {
            using (var db = new Data.DBMEdition01Context())
            {
                db.ApiDeleteTask(taskIDs);
            }
        }

        public static List<SelectOption> GetTracerList(int? siteId, int? programId, int? statusId)
        {
            List<ApiTracerDetailSelectBySiteProgramStatusReturnModel> _dbResult;
            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiTracerDetailSelectBySiteProgramStatus(siteId, programId, statusId);
            }

            var _result = _dbResult.Select(m =>
            new SelectOption
            {
                Value = m.TracerCustomID.ToString(),
                Name = m.TracerCustomName
            }
            ).ToList();


            return _result;

        }

        public static List<SelectOption> GetChapterList(int? siteId, int? programId, int? userId, int? certificationItemId, string standardEffBeginDate, int? serviceProfileTypeId)
        {
            List<ApiGetChaptersBySiteAndProgramReturnModel> _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetChaptersBySiteAndProgram(siteId, programId, userId, certificationItemId, Convert.ToDateTime(standardEffBeginDate), serviceProfileTypeId);
            }

            var _result = _dbResult.Select(m =>
            new SelectOption
            {
                Value = m.ChapterID.ToString(),
                Name = m.ChapterTextName
            }
            ).ToList();


            return _result;

        }

        public static List<SelectOption> GetStandardByChapterList(string standardEffBeginDate, int? productType, int? programId, int? chapterId, int? siteId, int? userId, int? serviceProfileTypeId, int? certificationItemId)
        {
            List<ApiGetStandardsByChapterReturnModel> _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetStandardsByChapter(standardEffBeginDate, productType, programId, chapterId, siteId, userId, serviceProfileTypeId, certificationItemId);
            }

            var _result = _dbResult.Select(m =>
            new SelectOption
            {
                Value = m.StandardTextID.ToString(),
                Name = (m.StandardLabel + " " + m.StandardText).Length > 100 ? (m.StandardLabel + " " + m.StandardText).Substring(0, 100) + "..." : (m.StandardLabel + " " + m.StandardText),
                FullText = (m.StandardLabel + " " + m.StandardText)
            }
            ).ToList();


            return _result;

        }

        public static ApiGetChapterStandardByEpTextIdReturnModel GetChapterStandardByEPTextID(string standardEffBeginDate, int epTextID, int programID)
        {
            ApiGetChapterStandardByEpTextIdReturnModel _result;


            using (var db = new DBMEdition01Context())
            {
                _result = db.ApiGetChapterStandardByEpTextId(standardEffBeginDate, epTextID, programID).FirstOrDefault();
            }


            return _result;

        }

        public static List<ApiGetEPsByStandardReturnModel> GetEPsByStandard(int userId, int productType, int siteId, int programId, int standardTextId, string standardEffBeginDate, int serviceProfileTypeId, int certificationItemId)
        {
            List<ApiGetEPsByStandardReturnModel> _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetEPsByStandard(userId, productType, siteId, programId, standardTextId, Convert.ToDateTime(standardEffBeginDate), serviceProfileTypeId, certificationItemId);
            }


            return _dbResult;

        }

        public static List<SelectOption> GetCOPList(int? siteId, int? programId)
        {

            List<ApiGetCopDataReturnModel> _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetCopData(siteId, programId);
            }

            var _result = _dbResult.Select(m =>
            new SelectOption
            {
                Value = m.CopName,
                Name = m.CopName + " - " + m.Title
            }
            ).ToList();


            return _result;
        }



        public static List<SelectOption> GetTagsByCopAndProgramID(int? programId, string copName)
        {

            List<ApiGetTagsByCoPReturnModel> _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetTagsByCoP(programId, copName);
            }

            _dbResult.ForEach(m =>
            {

                var elements = GetElementsByCopId(programId, m.TagCode);
                m.HeaderText = string.Format("{0} - {1}", m.TagCode, elements[0].JcrElementText);
            }
            );


            var _result = _dbResult.Select(m =>
            new SelectOption
            {
                Value = m.TagCode,
                Name = formatString(m.HeaderText, 100)
            }
            ).ToList();


            return _result;

        }

        private static string formatString(string inputString, int length)
        {
            var _result = "";

            inputString = inputString.Replace(oldValue: "<br/>", newValue: " ");

            _result = length > 0 && inputString.Length > length ? inputString.Substring(0, length - 1) + "..." : inputString;

            return _result;
        }

        public static List<ApiGetElementsByTagReturnModel> GetElementsByCopId(int? programId, string tagCode)
        {

            List<ApiGetElementsByTagReturnModel> _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetElementsByTag(programId, tagCode);
            }

            _dbResult.ForEach(m =>
            {
                m.JcrElementText = formatString(m.JcrElementText, 0);
            }
            );


            return _dbResult;

        }

        public static ApiGetCopTagByCmsStandardIdReturnModel GetCopTagByCmsStandardId(int? programId, int? cmsStandardId)
        {

            ApiGetCopTagByCmsStandardIdReturnModel _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetCopTagByCmsStandardId(programId, cmsStandardId).ToList().FirstOrDefault();
            }

            _dbResult.JcrElementText = formatString(_dbResult.JcrElementText, 0);


            return _dbResult;

        }

        public static ApiGetTracerByIdReturnModel GetTracerById(int? tracerCustomId)
        {

            ApiGetTracerByIdReturnModel _dbResult;


            using (var db = new DBMEdition01Context())
            {
                _dbResult = db.ApiGetTracerById(tracerCustomId).ToList().FirstOrDefault();
            }


            return _dbResult;

        }

        public static ApiGetTaskDetailsReturnModel GetTaskDetails(int? taskId, string standardEffBeginDate, int? programId)
        {
            ApiGetTaskDetailsReturnModel _dbResult;

            using (var db = new Data.DBMEdition01Context())
            {
                _dbResult = db.ApiGetTaskDetails(taskId, Convert.ToDateTime(standardEffBeginDate), programId).FirstOrDefault();
            }

            return _dbResult;
        }

        public static string UpdateUserTask(int? taskId, string taskName, int? taskTypeId, int? siteId, int? programId, string assignedDate, int? assignedToUserId, int? assignedByUserId, string taskDetails, string ccUserIDs, string dueDate, int? taskStatus, int? updatedByUserId, int? createdByUserId, int? tracerCustomId, int? tracerResponseId, int? tracerQuestionId, int? tracerQuestionAnswerId, int? epTextId, int? cmsStandardId, bool? ReminderEmailRequired, string taskresolution, string completeDate, string lstUsers)
        {
            ApiUpdateUserTaskReturnModel _dbResult;

            using (var db = new Data.DBMEdition01Context())
            {
                _dbResult = db.ApiUpdateUserTask(taskId, taskName, taskTypeId, siteId, programId, assignedDate, assignedToUserId, assignedByUserId, taskDetails, ccUserIDs, dueDate, taskStatus, updatedByUserId, createdByUserId, tracerCustomId, tracerResponseId, tracerQuestionId, tracerQuestionAnswerId, epTextId, cmsStandardId, ReminderEmailRequired, taskresolution, completeDate, lstUsers).FirstOrDefault();
            }

            return _dbResult.tasklist;
        }

        public static string GetMaxTaskAssignedDate(string taskIds)
        {
            string MaxAssignedDate;

            using (var db = new Data.DBMEdition01Context())
            {
                MaxAssignedDate = ((DateTime)db.ApiGetMaxTaskAssignedDate(taskIds).FirstOrDefault().AssignedDate).ToShortDateString();
            }

            return MaxAssignedDate;
        }

        public static void ReAssignTask(ReAssignTasks raTasks)
        {
            using (var db = new Data.DBMEdition01Context())
            {
                db.ApiReAssignTask(raTasks.LstTaskIDs, raTasks.AssignedToUserID, raTasks.CCUserIDs, raTasks.DueDate, raTasks.UpdatedByUserID);
            }
        }



        public static List<ApiGetAmpTaskListByEpTextIdReturnModel> GetAMPTaskListByEPTextID(string standardEffDate, int siteId, int programId, int epTextId)
        {
            List<ApiGetAmpTaskListByEpTextIdReturnModel> _result;
            using (var db = new Data.DBMEdition01Context())
            {
                _result = db.ApiGetAmpTaskListByEpTextId(standardEffDate, siteId, programId, epTextId);
            }
            return _result;
        }

        public static void UpdateTaskObservationAssoc(int? tracerResponseId, string taskIDs)
        {

            using (var db = new Data.DBMEdition01Context())
            {
                db.ApiUpdateTaskObservationAssoc(tracerResponseId, taskIDs);
            }

        }



        public static List<ApiGetCmsTaskListByCmsStandardIdReturnModel> GetCMSTaskListByCMSStandardID(string standardEffDate, int siteId, int programId, int cmsStandardId)
        {
            List<ApiGetCmsTaskListByCmsStandardIdReturnModel> _result;
            using (var db = new Data.DBMEdition01Context())
            {
                _result = db.ApiGetCmsTaskListByCmsStandardId(standardEffDate, siteId, programId, cmsStandardId);
            }
            return _result;
        }

        public static ApiGetSiteEmailNotificationSettingsReturnModel GetSiteEmailNotificationSettings(int siteId)
        {
            ApiGetSiteEmailNotificationSettingsReturnModel _result;
            using (var db = new Data.DBMEdition01Context())
            {
                _result = db.ApiGetSiteEmailNotificationSettings(siteId).FirstOrDefault();
            }
            return _result;
        }

        public static void UpdateSiteEmailNotificationSettings(int? siteId, bool? sendEmailOnTaskCreation, bool? sendEmailBeforeTaskDue, int? daysBeforeTaskDue, bool? sendEmailsAfterTaskDue, int? daysAfterTaskDue, bool? sendRemainderEmailAfterTaskDue, bool? sendTaskReportToCC, int? taskReportToCCScheduleTypeId, bool? sendTaskReportToUsers, int? taskReportToUsersScheduleTypeId, string taskReportRecipients, bool? sendEmailOnAssigningEp, int? taskDueRecipientType, int? updatedByUserId)
        {
            using (var db = new Data.DBMEdition01Context())
            {
                db.ApiUpdateSiteEmailNotificationSettings(siteId, sendEmailOnTaskCreation, sendEmailBeforeTaskDue, daysBeforeTaskDue, sendEmailsAfterTaskDue, daysAfterTaskDue, sendRemainderEmailAfterTaskDue, sendTaskReportToCC, taskReportToCCScheduleTypeId, sendTaskReportToUsers, taskReportToUsersScheduleTypeId, taskReportRecipients, sendEmailOnAssigningEp, taskDueRecipientType, updatedByUserId);
            }
        }

        public static ApiGetTaskFollowUpRemainderSettingsReturnModel GetTaskFollowUpRemainderSettings(int siteId)
        {
            ApiGetTaskFollowUpRemainderSettingsReturnModel _result;
            using (var db = new Data.DBMEdition01Context())
            {
                _result = db.ApiGetTaskFollowUpRemainderSettings(siteId).FirstOrDefault();
            }
            return _result;
        }

        public static List<UserTaskEmail> GetTaskDetailsForSendingEmail(string taskIDs, int siteId, int programId, int userId, bool skipItemAssociated = false)
        {
            List<UserTaskEmail> _result;

            string tracersTaskUrl = ConfigurationManager.AppSettings["TracersTaskUrl"].ToString();

            using (var db = new Data.DBMEdition01Context())
            {
                _result = (from l in db.ApiGetTaskDetailsForSendingEmail(taskIDs, siteId, programId, userId, tracersTaskUrl, skipItemAssociated)
                           select new UserTaskEmail()
                           {
                               AssignedByEmail = l.AssignedByEmail,
                               AssignedByFirstName = l.AssignedByFirstName,
                               AssignedByLastName = l.AssignedByLastName,
                               AssignedOn = l.AssignedOn,
                               AssignedToEmail = l.AssignedToEmail,
                               AssignedToFirstName = l.AssignedToFirstName,
                               AssignedToLastName = l.AssignedToLastName,
                               CCUserIDs = l.CCUserIDs,
                               CompletionDetails = l.CompletionDetails,
                               CreateDate = l.CreateDate,
                               CreatedByEmail = l.CreatedByEmail,
                               CreatedByFirstName = l.CreatedByFirstName,
                               CreatedByLastName = l.CreatedByLastName,
                               DueDate = l.DueDate,
                               HCOID = l.HCOID,
                               ItemAssociated = l.ItemAssociated,
                               UpdatedByEmail = l.UpdatedByEmail,
                               UpdatedByFirstName = l.UpdatedByFirstName,
                               UpdatedByLastName = l.UpdatedByLastName,
                               ProgramID = l.ProgramID,
                               SiteID = l.SiteID,
                               SiteName = l.SiteName,
                               TaskDetails = l.TaskDetails,
                               TaskID = l.TaskID,
                               TaskLinkUrl = l.TaskLinkUrl,
                               TaskName = l.TaskName,
                               TaskStatus = l.TaskStatus,
                               TaskTypeID = l.TaskTypeID,
                               UpdateDate = l.UpdateDate,
                               AssignedByUserID = l.AssignedByUserID,
                               AssignedToUserID = l.AssignedToUserID,
                               CompletionDate = l.CompletionDate,
                               CreatedByUserID = l.CreatedByUserID,
                               UpdatedByUserID = l.UpdatedByUserID,
                               TaskStatusName = l.TaskStatusName,
                               CMSStandardID = l.CMSStandardID,
                               EPTextID = l.EPTextID,
                               TracerCustomID = l.TracerCustomID,
                               TracerQuestionAnswerID = l.TracerQuestionAnswerID,
                               TracerQuestionID = l.TracerQuestionID,
                               TracerResponseID = l.TracerResponseID,
                               ProgramName = l.ProgramName
                           }).ToList();
            }

            return _result;
        }

        public static void SendTaskEmailAfterSave(string TaskIDs, int siteId, int programId, int userId, List<UserTaskEmail> lstTaskPreviousData, string observationTitle = "")
        {
            List<UserTaskEmail> lstTasks;

            try
            {
                var siteEmailSetting = GetSiteEmailNotificationSettings(siteId);

                if (siteEmailSetting.SendEmailOnTaskCreation == true)
                {
                    lstTasks = GetTaskDetailsForSendingEmail(TaskIDs, siteId, programId, userId);

                    var ccUserIds = String.Join(";", lstTasks.Select(a => a.CCUserIDs).Distinct());
                    var ccUsers = GetUsers(ccUserIds, siteId);

                    EmailHelpers.SendTaskEmailAfterSave(lstTasks, lstTaskPreviousData, siteId, userId, ccUsers, observationTitle);
                }
            }
            catch (Exception ex)
            {

                string sqlParam = "";
                string methodName = "SendTaskEmail";
                new ExceptionLogServices().ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

            }
            finally
            {
                lstTasks = null;
                lstTaskPreviousData = null;
            }

        }
        public static void DisableTaskNotificationScheduleType(Enums.DisableTaskNotificationScheduleType taskChangedType, int siteId, int userId, string lstTaskIDs = "")
        {
            try
            {
                using (var db = new Data.DBMEdition01Context())
                {
                    db.ApiDisableTaskNotificationSchedules((int)taskChangedType, siteId, userId, lstTaskIDs);
                }
            }
            catch (Exception ex)
            {

                string sqlParam = "";
                string methodName = "SendTaskEmail";
                new ExceptionLogServices().ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

            }

        }

        public static void ResendTaskEmail(string TaskIDs, int siteId, int programId, int userId)
        {
            List<UserTaskEmail> lstTasks;

            try
            {
                var siteEmailSetting = GetSiteEmailNotificationSettings(siteId);

                if (siteEmailSetting.SendEmailOnTaskCreation == true)
                {
                    lstTasks = GetTaskDetailsForSendingEmail(TaskIDs, siteId, programId, userId);
                    EmailHelpers.SendTaskEmailAfterSave(lstTasks, null, siteId, userId, null, ResendEmail: true);
                }
            }
            catch (Exception ex)
            {

                string sqlParam = "";
                string methodName = "ResendTaskEmail";
                new ExceptionLogServices().ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

            }
            finally
            {
                lstTasks = null;
            }
        }

        public static List<AssignedUser> GetUsers(string lstUserIDs, int siteId)
        {
            List<ApiGetUsersReturnModel> _dbResult;

            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _dbResult = db.ApiGetUsers(lstUserIDs);
                    var _usrList = _dbResult.Select(m => new AssignedUser
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        EmailAddress = m.EmailAddress,
                        UserId = m.UserID
                    }
                    ).ToList();

                    return _usrList;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUsers(" + lstUserIDs + ")";
                    string methodName = "JCRAPI/Business/TaskService/GetUsers";
                    new ExceptionLogServices().ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
                    return null;
                }


            }

        }

        public static void SendTaskReassignedEmail(string taskIds, int siteId, int programId, int userId, List<UserTaskEmail> lstPreviousTasks)
        {
            List<UserTaskEmail> lstTasks;
            int templateId, actionTypeId;
            UserTaskEmail task;
            AssignedUser user;

            try
            {
                var siteEmailSetting = GetSiteEmailNotificationSettings(siteId);

                if (siteEmailSetting.SendEmailOnTaskCreation == true)
                {

                    //Send Email to New Assignee
                    lstTasks = TaskServices.GetTaskDetailsForSendingEmail(taskIds, siteId, programId, userId);
                    templateId = (int)Enums.TaskEmailTemplateID.TaskReassignedToNewAssignee;
                    actionTypeId = (int)Enums.ActionTypeEnum.TaskReassign;

                    task = lstTasks.First();
                    user = new AssignedUser
                    {
                        EmailAddress = task.AssignedToEmail,
                        FirstName = task.AssignedToFirstName,
                        LastName = task.AssignedToLastName,
                        UserId = task.AssignedToUserID
                    };

                    EmailHelpers.SendMultipleTaskEmailForUser(user, lstTasks, siteId, userId, templateId, actionTypeId);

                    //Send Email to Previous Assignees
                    var assignees = lstPreviousTasks.Select(a => a.AssignedToUserID).Distinct().ToList();
                    templateId = (int)Enums.TaskEmailTemplateID.TaskReassignedToPrevAssignee;
                    actionTypeId = (int)Enums.ActionTypeEnum.TaskReassign;

                    foreach (var assignee in assignees)
                    {
                        List<UserTaskEmail> assigneeTasks = lstPreviousTasks.Where(a => a.AssignedToUserID == assignee).ToList();

                        task = assigneeTasks.First();
                        user = new AssignedUser
                        {
                            EmailAddress = task.AssignedToEmail,
                            FirstName = task.AssignedToFirstName,
                            LastName = task.AssignedToLastName,
                            UserId = task.AssignedToUserID
                        };

                        EmailHelpers.SendMultipleTaskEmailForUser(user, assigneeTasks, siteId, userId, templateId, actionTypeId);

                    }

                    //Send Email to CC users
                    templateId = (int)Enums.TaskEmailTemplateID.TaskReassignedToCCUser;
                    actionTypeId = (int)Enums.ActionTypeEnum.TaskReassign;

                    var ccUserIds = String.Join(";", lstTasks.Select(a => a.CCUserIDs).Distinct());
                    var ccUsers = GetUsers(ccUserIds, siteId);

                    foreach (var ccUser in ccUsers)
                    {
                        List<UserTaskEmail> ccTasks = lstTasks.Where(a => a.CCUserIDs.Split(';').Contains(ccUser.UserId.ToString())).ToList();
                        EmailHelpers.SendMultipleTaskEmailForUser(ccUser, ccTasks, siteId, userId, templateId, actionTypeId);
                    }

                }

            }
            catch (Exception ex)
            {
                string sqlParam = "";
                string methodName = "SendReassignedTaskEmailToAssignee";
                new ExceptionLogServices().ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

            }
            finally
            {
                lstTasks = null;
                lstPreviousTasks = null;
                task = null;
                user = null;
            }
        }
    }
}
