using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Text;
using System.Data;
using System.Configuration;
using System.Xml.Linq;
using System.Collections.Generic;
using Jcr.Api.Models;

namespace Jcr.Api.Helpers
{
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

        
        public static void SendEmail(Email email, string SelectedSiteName, string FullName)
        {

            SmtpClient smt = new SmtpClient();
            MailMessage siteEmail = new MailMessage();
            siteEmail.IsBodyHtml = true;
            siteEmail.Body = email.Body;

            siteEmail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmailAddress"]);

            siteEmail.To.Add(emaillist(email.To).ToString());

            if (email.Cc != null && email.Cc.Length > 0)
            {
                siteEmail.CC.Add(emaillist(email.Cc).ToString());
            }
            if (email.Bcc != null && email.Bcc.Length > 0)
            {
                siteEmail.Bcc.Add(emaillist(email.Bcc).ToString());

            }
            siteEmail.Bcc.Add(emaillist(ConfigurationManager.AppSettings["BCCEmailAddress"]).ToString());

            if (email.Subject != null && email.Subject.Length > 0)
            {
                siteEmail.Subject = email.Subject;

            }
            else
            {

                siteEmail.Subject = SelectedSiteName + "-Tracers: " + email.Title + " Sent to you by " + FullName;

            }


            string smtpServer = null;
            //****************Send It!!!

            try
            {
                smtpServer = ConfigurationManager.AppSettings["SMTPserver"];


                if (email.AttachmentLocation != null && email.AttachmentLocation.Capacity > 0)
                {
                    foreach (string filelocation in email.AttachmentLocation)
                    {
                        if (filelocation != "")
                        {

                            var item = new Attachment(filelocation);
                            siteEmail.Attachments.Add(item);
                        }
                    }


                }


                smt.Host = smtpServer;

                smt.Send(siteEmail);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}