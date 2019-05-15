using System;
using System.Text;
using System.Web;
using System.Diagnostics;
using Jcr.Business;
using System.Data.Entity.Core.Objects;


namespace Jcr.Api.Helpers
{
    public class ExceptionHelper
    {
        public string SaveException(Exception ex)
        {
            string exceptionText = "";

            try
            {
                

                ExceptionLog exceptionLog = new ExceptionLog();
                var stackTrace = new StackTrace(ex);

                int SiteId = ex.Data["SiteId"] != null ? Convert.ToInt32(ex.Data["SiteId"]) : 0;

                int UserId = ex.Data["UserId"] != null ? Convert.ToInt32(ex.Data["UserId"]) : 0;


                //String together all exceptions (if more than 1)
                //exceptionText = GetExceptionDetails(ex);
                exceptionText = GetExceptionText(ex);

                //Stringbuilder to contain list of methods in the Exception Stack
                StringBuilder sbMethodName = new StringBuilder();

                //To get the the Event Name and all Method Names
                foreach (StackFrame frame in stackTrace.GetFrames())
                {
                    var method = frame.GetMethod();
                    sbMethodName.Append("JCRAPI: ");
                    sbMethodName.Append(method.ToString());
                    sbMethodName.Append("\n");
                }

                //Build object
                exceptionLog.MethodName = String.Empty;

                if (sbMethodName != null)
                    exceptionLog.MethodName = sbMethodName.ToString();

                exceptionLog.ExceptionText = exceptionText;

                exceptionLog.PageName = string.Empty;

                exceptionLog.HttpReferrer = ex.Data["HttpReferrer"] != null ? ex.Data["HttpReferrer"].ToString() : "";




                exceptionLog.CreateDate = DateTime.Now;

                int ExceptionLogId = 0;

                ObjectParameter exceptionLogID = new ObjectParameter("ExceptionLogId", ExceptionLogId);

                //Log to database
                ExceptionLogServices ExceptionService=new ExceptionLogServices();
                
                ExceptionService.ExceptionLogInsert(exceptionLog.ExceptionText, exceptionLog.PageName, exceptionLog.MethodName, UserId, SiteId, exceptionLog.TransSql, exceptionLog.HttpReferrer);



            }
            catch (Exception ex2)
            {
                //Do nothing. Maybe email admin?
                string exMessage = ex2.ToString();
            }

            return exceptionText;
        }


     
        private string GetExceptionDetails(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            GetExceptionDetails(ex, sb);
            return sb.ToString().Replace("\n", "<br/>");
        }

        private string GetExceptionText(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<b>Source:</b> {0}\n<b>Message:</b> {1}\n<b>Stack Trace:</b>\n{2}\n\n",
                ex.Source, ex.Message, ex.StackTrace));

            sb.Append("\n");
            if (ex.InnerException != null)
                GetExceptionDetails(ex.InnerException, sb);

            return sb.ToString().Replace("\n", "<br/>");
        }

        private void GetExceptionDetails(Exception ex, StringBuilder sb)
        {
            sb.Append(string.Format("<b>Source:</b> {0}\n<b>Message:</b> {1}\n<b>Stack Trace:</b>\n{2}\n\n",
                ex.Source, ex.Message, ex.StackTrace));
            if (ex.Data.Count > 0)
            {
                sb.Append("<b>Additional Info:</b>\n");
                foreach (object obj2 in ex.Data.Keys)
                {
                    sb.Append(string.Format("{0} = {1}\n", obj2.ToString(),
                        (ex.Data[obj2] != null) ? ex.Data[obj2].ToString() : "{<i>null</i>}"));
                }
            }
            sb.Append("\n");
            if (ex.InnerException != null)
                GetExceptionDetails(ex.InnerException, sb);
        }
    }

    public class ExceptionLog
    {
        int exceptionLogID;
        string exceptionText;
        private string pageName;
        private string methodName;
        private int userId;
        private int siteId;
        private string transSql;
        private string httpReferrer;
        DateTime createDate;

        public int ExceptionLogID
        {
            get { return exceptionLogID; }
            set { exceptionLogID = value; }
        }
        public string ExceptionText
        {
            get { return exceptionText; }
            set { exceptionText = value; }
        }

        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        public string TransSql
        {
            get { return transSql; }
            set { transSql = value; }
        }

        public string HttpReferrer
        {
            get { return httpReferrer; }
            set { httpReferrer = value; }
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
    }
}