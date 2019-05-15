using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using Jcr.Api.Models;
using Jcr.Api.Enumerators;
using Jcr.Api.Helpers;
using Jcr.Data;
using System.ComponentModel;
using System.Configuration;
//using Microsoft.Reporting.WebForms;

namespace Jcr.Business
{
    public class CustomerSupportEmailServices
    {

        public void TracerMobileCustomerSupportEmail(CustomerSupport customerSupport)
        {
            //Insert record to database
            int rowCount = InsertCustomerSupport(customerSupport);


            //Load Email Template          
            string TMobileCustomerSupportEmailTo = ConfigurationManager.AppSettings["TracerMobileCustomerSupportEmailTo"].ToString();
            string TMobileCustomerSupportEmailFrom = ConfigurationManager.AppSettings["TracerMobileCustomerSupportEmailFrom"].ToString();
            string smtpServer = ConfigurationManager.AppSettings["TracerSMTPserver"];
            string subject = "Customer Support Request From Tracers Mobile App";
            var nvc = new NameValueCollection(6);

            nvc["USEREMAIL"] = customerSupport.Email;
            nvc["USERFULLNAME"] = customerSupport.UserName;
            nvc["USERID"] = customerSupport.UserID;
            nvc["SITEID"] = customerSupport.SiteID;
            nvc["HCOID"] = (customerSupport.HCOID == 0) ? "n/a" : customerSupport.HCOID.ToString();
            nvc["SUBMITTIME"] = DateTime.Now.ToString(); //customerSupport.SubmitTime;
            nvc["SUBJECT"] = customerSupport.Subject;
            nvc["PROGRAM"] = customerSupport.ProgramName;
            nvc["BODY"] = customerSupport.Body;

            //Create body from template
            string body = EmailServices.LoadEmailTemplate("Support.htm", nvc);

            //Send Email
            //(string to, string cc, string bcc, string replyTo,
            //string subject, string body, string attachment, bool isError, string sMTPServer,
            //string emailUserName, string emailPassword, string emailErrorTo,
            //string emailTestsTo, string emailFrom, int userId, int siteId)
           
            EmailHelpers.SendSMTPEmail(
                          TMobileCustomerSupportEmailTo,
                          string.Empty,
                          string.Empty,
                          customerSupport.Email,
                          subject,
                          body,
                          string.Empty,
                          false,
                          smtpServer,
                          string.Empty,
                          string.Empty,
                          string.Empty,
                          string.Empty,
                          TMobileCustomerSupportEmailFrom,                          
                          Convert.ToInt32(customerSupport.UserID),
                          Convert.ToInt32(customerSupport.SiteID)
                          );

           

        }
        public int InsertCustomerSupport(CustomerSupport customerSupport)
        {
            ExceptionLogServices exceptionLog = new ExceptionLogServices();
            int _result;
            DateTime submitTime = DateTime.Now;
            //=============User for Expection Log=======
            string userId = customerSupport.UserID;
            string siteId = customerSupport.SiteID;
            string HCOID = (customerSupport.HCOID == 0) ? "null" : customerSupport.HCOID.ToString();
            string subject = customerSupport.Subject;
            string body = customerSupport.Body;
            string productId = customerSupport.EProductID.ToString();
            string programId = customerSupport.ProgramID.ToString();
            //==========================================
            using (var db = new DBAMPContext())
            {
               
                try
                {
                    _result = db.ApiInsertCustomerSupport(Convert.ToInt32(customerSupport.UserID),
                       Convert.ToInt32(customerSupport.SiteID),
                       (customerSupport.HCOID == 0) ? null : customerSupport.HCOID,
                       submitTime,
                       customerSupport.Subject,
                       customerSupport.Body,
                       customerSupport.EProductID,
                       customerSupport.ProgramID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiInsertCustomerSupport(" + userId + "," + siteId + "," + HCOID + "," + submitTime.ToString() + "," + subject + "," + body + "," + productId + "," + programId+")";
                    string methodName = "JCRAPI/Business/CustomerSupportEmailServices/InsertCustomerSupport";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, Convert.ToInt32(userId), Convert.ToInt32(siteId), sqlParam, string.Empty);

                    return 0;
                }
            }

            return _result;
        }
    }
}
