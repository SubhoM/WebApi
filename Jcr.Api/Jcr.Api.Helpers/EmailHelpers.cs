using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;

using Jcr.Api.Models;
using Jcr.Api.Enumerators;
using System.Collections.Specialized;
using System.Threading;

using System.Diagnostics;
using static Jcr.Api.Enumerators.Enums;
using Jcr.Tracers.Model;

namespace Jcr.Api.Helpers
{

	public class CCUserTaskEmail
	{
		public int CCUserID { get; set; }
		public CCMService.EmailRecipient CCUser { get; set; }
		public List<AssignedToUserTaskEmail> Assignees { get; set; }
		public string TaskIDs { get; set; }
	}

	public static class EmailHelpers
	{
		private static MailAddressCollection emaillist(string emailaddress)
		{
			MailAddressCollection addressList = new MailAddressCollection();

			foreach (var curr_address in emailaddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
			{
				if (curr_address.Trim() != "")
				{
					MailAddress myAddress = new MailAddress(curr_address.Trim());
					addressList.Add(myAddress);
				}
			}
			return addressList;
		}


		public static void SendEmail(Email email, int actionTypeId, int siteId, int userId, string siteName, string fullName)
		{

			SmtpClient smt = new SmtpClient();
			MailMessage siteEmail = new MailMessage();
			siteEmail.IsBodyHtml = true;
			siteEmail.Body = email.Body;

			siteEmail.From = new MailAddress(ConfigurationManager.AppSettings["TracerFromEmailAddress"]);

			siteEmail.To.Add(emaillist(email.To).ToString());

			if (email.Cc != null && email.Cc.Length > 0)
			{
				siteEmail.CC.Add(emaillist(email.Cc).ToString());
			}
			if (email.Bcc != null && email.Bcc.Length > 0)
			{
				siteEmail.Bcc.Add(emaillist(email.Bcc).ToString());

			}

			//siteEmail.Bcc.Add(emaillist(ConfigurationManager.AppSettings["BCCEmailAddress"]).ToString());
			if (email.Subject != null && email.Subject.Length > 0)
			{
				siteEmail.Subject = email.Subject;

			}
			else
			{

				siteEmail.Subject = siteName + "-Tracers: " + email.Title + " Sent to you by " + fullName;

			}


			string smtpServer = null;
			//****************Send It!!!

			try
			{
				smtpServer = ConfigurationManager.AppSettings["TracerSMTPserver"];


				//if (email.AttachmentLocation != null && email.AttachmentLocation.Capacity > 0)
				//{
				//    foreach (string filelocation in email.AttachmentLocation)
				//    {
				//        if (filelocation!=null && filelocation != "")
				//        {

				//            var item = new Attachment(filelocation);
				//            siteEmail.Attachments.Add(item);
				//        }
				//    }


				//}


				string applicationCode = Enums.ApplicationCode.Tracers.ToString();

				string fnReturnValue = string.Empty;


				// Invoke the CCM Service to send the email
				CCMService.ProcessEmailClient oEmail = new CCMService.ProcessEmailClient();
				CCMService.MailDetails mailDetails = new CCMService.MailDetails();
				mailDetails.EmailTo = siteEmail.To.ToString();
				mailDetails.FromEmail = siteEmail.From.ToString();
				mailDetails.EmailCC = email.Cc;
				mailDetails.EmailBCC = email.Bcc;
				mailDetails.MailSubject = siteEmail.Subject;
				mailDetails.MailContent = siteEmail.Body;
				mailDetails.SiteID = siteId;
				mailDetails.Guid = email.Guid;

				oEmail.SendGeneralEmail(mailDetails, ref fnReturnValue, applicationCode, actionTypeId, userId);

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void SendTaskEmailAfterSave(List<UserTaskEmail> lstTasks, List<UserTaskEmail> lstTaskPreviousData, int siteId, int userId, List<AssignedUser> ccUsers, string observationTitle = "", bool ResendEmail = false)
		{
			var lstCCUserTaskEmail = new List<CCUserTaskEmail>();

			foreach (var task in lstTasks)
			{
				var lstEmailRecipients = new List<CCMService.EmailRecipient>();
				UserTaskEmail previousTaskData = null;

				try
				{
					int templateId = 0;
					int actionTypeId = 0;

					if (lstTaskPreviousData != null)
						previousTaskData = lstTaskPreviousData.FirstOrDefault(a => a.TaskID == task.TaskID);


					switch ((Enums.TaskStatus)task.TaskStatus)
					{
						case Enums.TaskStatus.Open:

							if ((TaskType)task.TaskTypeID == TaskType.TracerObservation)
							{
								task.ItemAssociated += !string.IsNullOrEmpty(observationTitle) ? " - " + observationTitle : observationTitle;
							}

							if (previousTaskData == null) //New Task
							{
								templateId = (int)Enums.TaskEmailTemplateID.NewTask;
								actionTypeId = (int)Enums.ActionTypeEnum.TaskNew;

								lstEmailRecipients.Clear();

								//Sending email to Assignee
								lstEmailRecipients.Add(new CCMService.EmailRecipient { EmailAddress = task.AssignedToEmail, FullName = task.AssignedToFullName });

								SendTaskEmailThroughCCM(task, lstEmailRecipients, siteId, userId, templateId, actionTypeId);

								if (ResendEmail == false)
								{
									//Send email to CC Users
									if (ccUsers != null)
									{
										foreach (var user in ccUsers.Where(a => task.CCUserIDs.Split(';').Contains(a.UserId.ToString())).ToList())
										{
											var ccUserObj = lstCCUserTaskEmail.FirstOrDefault(a => a.CCUserID == user.UserId);

											if (ccUserObj == null)
											{
												ccUserObj = new CCUserTaskEmail();
												ccUserObj.Assignees = new List<AssignedToUserTaskEmail>();
												lstCCUserTaskEmail.Add(ccUserObj);

												ccUserObj.CCUserID = user.UserId;
												ccUserObj.CCUser = new CCMService.EmailRecipient { EmailAddress = user.EmailAddress, FullName = user.FullName };
												ccUserObj.TaskIDs = "";
											}

											ccUserObj.TaskIDs += "," + task.TaskID.ToString();
											ccUserObj.Assignees.Add(new AssignedToUserTaskEmail()
											{
												AssignedToFirstName = task.AssignedToFirstName,
												AssignedToLastName = task.AssignedToLastName,
												AssignedToEmail = task.AssignedToEmail
											});
										}

									}
								}
							}
							else
							{
								var newCCUserIds = new string[0];

								if(task.AssignedToUserID != previousTaskData.AssignedToUserID) // Task Reassigned to another user, since the Edit task will now allow only 1 user in Assignee Field 5/15/2018 Bug: 54390
								{
									//Send Email to New Assignee
									templateId = (int)Enums.TaskEmailTemplateID.TaskReassignedToNewAssignee;
									actionTypeId = (int)Enums.ActionTypeEnum.TaskReassign;

									var assignUser = new AssignedUser
									{
										EmailAddress = task.AssignedToEmail,
										FirstName = task.AssignedToFirstName,
										LastName = task.AssignedToLastName,
										UserId = task.AssignedToUserID
									};

									SendSingleTaskEmailForUser(assignUser, task, siteId, userId, templateId, actionTypeId);

									//Send Email to Previous Assignees
									templateId = (int)Enums.TaskEmailTemplateID.TaskReassignedToPrevAssignee;
									actionTypeId = (int)Enums.ActionTypeEnum.TaskReassign;

									var prevUser = new AssignedUser
									{
										EmailAddress = previousTaskData.AssignedToEmail,
										FirstName = previousTaskData.AssignedToFirstName,
										LastName = previousTaskData.AssignedToLastName,
										UserId = previousTaskData.AssignedToUserID
									};

									SendSingleTaskEmailForUser(prevUser, task, siteId, userId, templateId, actionTypeId);

									//Send Email to CC users
									templateId = (int)Enums.TaskEmailTemplateID.TaskReassignedToCCUser;
									actionTypeId = (int)Enums.ActionTypeEnum.TaskReassign;

									var ccuser = ccUsers.Where(a => task.CCUserIDs.Split(';').Contains(a.UserId.ToString())).ToList();

									foreach (var item in ccuser)
									{
										SendSingleTaskEmailForUser(item, task, siteId, userId, templateId, actionTypeId);
									}
								}
								else if (IsTaskValueChanged(previousTaskData, task, ref newCCUserIds) == true) //Updated Task and if any values changed
								{

									templateId = (int)Enums.TaskEmailTemplateID.EditTask;
									actionTypeId = (int)Enums.ActionTypeEnum.TaskEdit;

									lstEmailRecipients.Clear();

									//Sending email to Assignee
									lstEmailRecipients.Add(new CCMService.EmailRecipient { EmailAddress = task.AssignedToEmail, FullName = task.AssignedToFullName });

									SendTaskEmailThroughCCM(task, lstEmailRecipients, siteId, userId, templateId, actionTypeId);

									//Send email to CC Users
									if (ccUsers != null)
									{
										foreach (var user in ccUsers.Where(a => newCCUserIds.Contains(a.UserId.ToString())).ToList())
										{
											var ccUserObj = lstCCUserTaskEmail.FirstOrDefault(a => a.CCUserID == user.UserId);

											if (ccUserObj == null)
											{
												ccUserObj = new CCUserTaskEmail();
												ccUserObj.Assignees = new List<AssignedToUserTaskEmail>();
												lstCCUserTaskEmail.Add(ccUserObj);

												ccUserObj.CCUserID = user.UserId;
												ccUserObj.CCUser = new CCMService.EmailRecipient { EmailAddress = user.EmailAddress, FullName = user.FullName };
												ccUserObj.TaskIDs = "";
											}

											ccUserObj.TaskIDs += "," + task.TaskID.ToString();
											ccUserObj.Assignees.Add(new AssignedToUserTaskEmail()
											{
												AssignedToFirstName = task.AssignedToFirstName,
												AssignedToLastName = task.AssignedToLastName,
												AssignedToEmail = task.AssignedToEmail
											});
										}
									}
								}
							}

							break;
						case Enums.TaskStatus.Complete:
							templateId = (int)Enums.TaskEmailTemplateID.CompletedTask;
							actionTypeId = (int)Enums.ActionTypeEnum.TaskCompleted;

							lstEmailRecipients.Clear();

							//Sending email to Assignee
							lstEmailRecipients.Add(new CCMService.EmailRecipient { EmailAddress = task.AssignedToEmail, FullName = task.AssignedToFullName });

							//Sending email to Assigner if not same.
							if (task.AssignedToUserID != task.AssignedByUserID)
								lstEmailRecipients.Add(new CCMService.EmailRecipient { EmailAddress = task.AssignedByEmail, FullName = task.AssignedByFullName });

							//If LastUpdatedBy (Before marking Completed) is different from Assigner or Assignee, send email
							if (previousTaskData.UpdatedByUserID != task.AssignedByUserID && previousTaskData.UpdatedByUserID != task.AssignedToUserID)
							{
								lstEmailRecipients.Add(new CCMService.EmailRecipient { EmailAddress = previousTaskData.UpdatedByEmail, FullName = previousTaskData.UpdatedByFullName });
							}

							SendTaskEmailThroughCCM(task, lstEmailRecipients, siteId, userId, templateId, actionTypeId);

							break;
						case Enums.TaskStatus.Deleted:
							continue; //skip the task since it is removed/deleted.
					}

				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					lstEmailRecipients = null;
				}

			}


			//Send email to CC Users
			try
			{
				if (lstCCUserTaskEmail.Count > 0)
				{
					var templateId = (int)Enums.TaskEmailTemplateID.NewTaskToCC;
					var actionTypeId = (int)Enums.ActionTypeEnum.TaskNew;
					var task = lstTasks.First();

					foreach (var ccuser in lstCCUserTaskEmail)
					{
						SendTaskEmailForCCUsersThroughCCM(task, ccuser.TaskIDs.Substring(1), ccuser.CCUser, ccuser.Assignees, siteId, userId, templateId, actionTypeId);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				lstCCUserTaskEmail = null;
			}


		}

		private static bool IsTaskValueChanged(UserTaskEmail previousTaskData, UserTaskEmail task, ref string[] newCCUserIds)
		{

			if (!previousTaskData.TaskName.Equals(task.TaskName))
				return true;

			if (!previousTaskData.AssignedOn.Equals(task.AssignedOn))
				return true;

			if (!previousTaskData.DueDate.Equals(task.DueDate))
				return true;

			if (!previousTaskData.AssignedToUserID.Equals(task.AssignedToUserID))
				return true;

			var pCCUsers = previousTaskData.CCUserIDs.Split(';').ToList().OrderBy(a => a).ToArray();
			var CCUsers = task.CCUserIDs.Split(';').ToList().OrderBy(a => a).ToArray();

			var result = (pCCUsers.Length == CCUsers.Length && pCCUsers.Intersect(CCUsers).Count() == pCCUsers.Length);

			if (!result)
			{
				newCCUserIds = CCUsers.Except(pCCUsers).ToArray();
				return true;
			}

			if (!previousTaskData.TaskDetails.Equals(task.TaskDetails))
				return true;

			if (!previousTaskData.CompletionDetails.Equals(task.CompletionDetails))
				return true;

			if (!previousTaskData.CMSStandardID.Equals(task.CMSStandardID))
				return true;

			if (!previousTaskData.EPTextID.Equals(task.EPTextID))
				return true;

			if (!previousTaskData.TracerCustomID.Equals(task.TracerCustomID))
				return true;

			if (!previousTaskData.TracerQuestionAnswerID.Equals(task.TracerQuestionAnswerID))
				return true;

			if (!previousTaskData.TracerQuestionID.Equals(task.TracerQuestionID))
				return true;

			if (!previousTaskData.TracerResponseID.Equals(task.TracerResponseID))
				return true;

			return false;

		}

		private static void SendTaskEmailThroughCCM(UserTaskEmail task, List<CCMService.EmailRecipient> lstEmailRecipients, int siteId, int userId, int templateId, int actionTypeId)
		{
			string FromEmailAddress = "";

			switch ((TaskType)task.TaskTypeID)
			{
				case TaskType.Generic:
				case TaskType.AMPEP:
				case TaskType.CMSStandard:
					FromEmailAddress = ConfigurationManager.AppSettings["AMPFromEmailAddress"];
					break;
				case TaskType.Tracer:
				case TaskType.TracerObservation:
				case TaskType.TracerObservationQuestion:
					FromEmailAddress = ConfigurationManager.AppSettings["TracerFromEmailAddress"];
					break;
			}

			CCMService.TaskDetail taskDetail = GetCCMTaskDetail(task);

			lstEmailRecipients.ForEach(a => a.FullName = a.FullName == "JCRFirstName JCRLastName" ? "Guest User" : a.FullName); //This will be replaced in Email Salutations

			CCMService.ProcessEmailClient oEmail = new CCMService.ProcessEmailClient();
			CCMService.TaskMailDetail taskMailDetail = new CCMService.TaskMailDetail();

			taskMailDetail.FromEmail = FromEmailAddress;
			taskMailDetail.SiteID = siteId;
			taskMailDetail.TemplateID = templateId;
			taskMailDetail.UserID = userId;
			taskMailDetail.ActionTypeID = actionTypeId;
			taskMailDetail.ApplicationCode = ApplicationCode.TracerswithAMP.ToString();
			taskMailDetail.EmailRecipients = lstEmailRecipients;

			oEmail.SendTaskEmailAsync(taskMailDetail, taskDetail);
		}

		private static void SendTaskEmailForCCUsersThroughCCM(UserTaskEmail task, string taskIDs, CCMService.EmailRecipient emailRecipients, List<AssignedToUserTaskEmail> lstTaskAssignees, int siteId, int userId, int templateId, int actionTypeId)
		{
			string FromEmailAddress = "";

			switch ((TaskType)task.TaskTypeID)
			{
				case TaskType.Generic:
				case TaskType.AMPEP:
				case TaskType.CMSStandard:
					FromEmailAddress = ConfigurationManager.AppSettings["AMPFromEmailAddress"];
					break;
				case TaskType.Tracer:
				case TaskType.TracerObservation:
				case TaskType.TracerObservationQuestion:
					FromEmailAddress = ConfigurationManager.AppSettings["TracerFromEmailAddress"];
					break;
			}

			CCMService.TaskDetail taskDetail = GetCCMTaskDetail(task);

			var assignees = (from l in lstTaskAssignees
							 select new
							 {
								 Assignee = l.AssignedToFullName == "JCRFirstName JCRLastName" ? l.AssignedToEmail : l.AssignedToFormattedName // this will be replaced in Email Task Detail
							 }).Select(a => a.Assignee).ToList<string>();

			taskDetail.AssignedTo = string.Join("; ", assignees);
			taskDetail.TaskIDs = taskIDs;

			if (emailRecipients.FullName == "JCRFirstName JCRLastName")
				emailRecipients.FullName = "Guest User"; //This will be replaced in Email Salutations

			CCMService.ProcessEmailClient oEmail = new CCMService.ProcessEmailClient();
			CCMService.TaskMailDetail taskMailDetail = new CCMService.TaskMailDetail();

			taskMailDetail.FromEmail = FromEmailAddress;
			taskMailDetail.SiteID = siteId;
			taskMailDetail.TemplateID = templateId;
			taskMailDetail.UserID = userId;
			taskMailDetail.ActionTypeID = actionTypeId;
			taskMailDetail.ApplicationCode = ApplicationCode.TracerswithAMP.ToString();

			taskMailDetail.EmailRecipients = new List<CCMService.EmailRecipient>();
			taskMailDetail.EmailRecipients.Add(emailRecipients);

			oEmail.SendTaskEmailAsync(taskMailDetail, taskDetail);
		}

		public static void SendSingleTaskEmailForUser(AssignedUser user, UserTaskEmail tasks, int siteId, int userId, int templateId, int actionTypeId)
		{
			var lstTasks = new List<UserTaskEmail>();

			try
			{
				lstTasks.Add(tasks);
				SendMultipleTaskEmailForUser(user, lstTasks, siteId, userId, templateId, actionTypeId);
			}
			catch (Exception)
			{
				throw;
			}

		}

		public static void SendMultipleTaskEmailForUser(AssignedUser user, List<UserTaskEmail> lstTasks, int siteId, int userId, int templateId, int actionTypeId)
		{
			try
			{
				//Sending email to Assignee
				var emailRecipient = new CCMService.EmailRecipient { EmailAddress = user.EmailAddress, FullName = user.FullName };

				SendMultipleTaskEmailThroughCCM(emailRecipient, lstTasks, siteId, userId, templateId, actionTypeId);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				lstTasks = null;
			}

		}

		private static void SendMultipleTaskEmailThroughCCM(CCMService.EmailRecipient emailRecipient, List<UserTaskEmail> lstTasks, int siteId, int userId, int templateId, int actionTypeId)
		{
			string FromEmailAddress = ConfigurationManager.AppSettings["AMPFromEmailAddress"];

			List<CCMService.TaskDetail> lstTaskDetails = new List<CCMService.TaskDetail>();
			string taskIds = string.Join(",", lstTasks.Select(a => a.TaskID));

			foreach (var task in lstTasks)
			{
				lstTaskDetails.Add(GetCCMTaskDetail(task));
			}

			if (emailRecipient.FullName == "JCRFirstName JCRLastName")
				emailRecipient.FullName = "Guest User"; //This will be replaced in Email Salutations

			CCMService.ProcessEmailClient oEmail = new CCMService.ProcessEmailClient();
			CCMService.TaskMailDetail taskMailDetail = new CCMService.TaskMailDetail();

			taskMailDetail.FromEmail = FromEmailAddress;
			taskMailDetail.SiteID = siteId;
			taskMailDetail.TemplateID = templateId;
			taskMailDetail.UserID = userId;
			taskMailDetail.ActionTypeID = actionTypeId;
			taskMailDetail.ApplicationCode = ApplicationCode.TracerswithAMP.ToString();

			taskMailDetail.EmailRecipients = new List<CCMService.EmailRecipient>();
			taskMailDetail.EmailRecipients.Add(emailRecipient);

			oEmail.SendTasksEmail(taskMailDetail, lstTaskDetails);
		}

		private static CCMService.TaskDetail GetCCMTaskDetail(UserTaskEmail task)
		{
			return new CCMService.TaskDetail
			{
				AssignedTo = task.AssignedToFullName == "JCRFirstName JCRLastName" ? task.AssignedToEmail : task.AssignedToFormattedName, // this will be replaced in Email Task Detail
				AssignedBy = task.AssignedByFormattedName,
				AssignedOn = task.AssignedOn,
				CompletionDate = task.CompletionDate,
				CompletionDetails = task.CompletionDetails,
				DueDate = task.DueDate,
				HCOID = task.HCOID.ToString(),
				ItemAssociated = task.ItemAssociated,
				SiteName = task.SiteName,
				TaskDetails = task.TaskDetails,
				TaskIDs = task.TaskID.ToString(),
				TaskLinkUrl = task.TaskLinkUrl,
				TaskName = task.TaskName,
				TaskStatus = task.TaskStatusName,
				UserFullName = "",
				UpdatedBy = task.UpdatedByFormattedName,
				ProgramName = task.ProgramName
			};
		}

		#region SendSMTPEmail
		public static void SendSMTPEmail(string to, string cc, string bcc, string replyTo,
			string subject, string body, string attachment, bool isError, string sMTPServer,
			string emailUserName, string emailPassword, string emailErrorTo,
			string emailTestsTo, string emailFrom, int userId, int siteId)
		{
			MailMessage message = new MailMessage();

			if (isError)
				message.To.Add(emailErrorTo);
			else
			{
				if (emailTestsTo.Length > 0)
				{
					string[] addresses = emailTestsTo.TrimEnd(';').Split(';');
					foreach (string semail in addresses)
					{
						message.To.Add(semail);
					}
				}
				else
				{
					string[] toEmails = to.TrimEnd(';').Split(';');
					foreach (string email in toEmails)
					{
						message.To.Add(email);
					}

					if (cc.Length > 0)
					{
						string[] ccEmails = cc.TrimEnd(';').Split(';');
						foreach (string email in ccEmails)
						{
							message.CC.Add(email);
						}
					}

					if (bcc.Length > 0)
					{
						string[] bccEmails = bcc.TrimEnd(';').Split(';');
						foreach (string email in bccEmails)
						{
							message.Bcc.Add(email);
						}
					}
				}
			}

			if (replyTo.Length > 0)
				message.ReplyToList.Add(new MailAddress(replyTo));

			message.From = new MailAddress(emailFrom);
			message.Subject = subject;
			message.IsBodyHtml = true;
			message.Body = body;

			if (attachment.Length > 0)
			{
				var item = new Attachment(attachment);
				message.Attachments.Add(item);
			}

			if (sMTPServer.Length > 0)
			{
				try
				{
					SmtpClient client = new SmtpClient(sMTPServer);
					client.UseDefaultCredentials = false;
					//if (emailUserName.Length > 0)
					//    client.Credentials = new NetworkCredential(emailUserName, emailPassword);

					//Send mail using CCM implementation
					string applicationCode = Enums.ApplicationCode.Tracers.ToString();
					int actionTypeId = (int)Enums.ActionTypeEnum.TracersEmailed;
					string fnReturnValue = string.Empty;

					// Invoke the CCM Service to send the email
					CCMService.ProcessEmailClient oEmail = new CCMService.ProcessEmailClient();
					CCMService.MailDetails mailDetails = new CCMService.MailDetails();
					mailDetails.EmailTo = message.To.ToString();
					mailDetails.FromEmail = emailFrom;
					mailDetails.EmailCC = message.CC.ToString();
					mailDetails.EmailBCC = message.Bcc.ToString();
					mailDetails.MailSubject = message.Subject;
					mailDetails.MailContent = message.Body;
					mailDetails.SiteID = siteId;
					oEmail.SendGeneralEmail(mailDetails, ref fnReturnValue, applicationCode, actionTypeId, userId);
				}
				catch (Exception)
				{
					if (!isError)
						throw;
				}
			}
		}
		#endregion
	}


}
