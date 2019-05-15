using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;


namespace Jcr.Api.Helpers

{
    public static class WebExceptionHelper
    {
        public static void LogException(Exception ex, NameValueCollection nvc)
        {
            //We don't need to log this exception...it occurs when
            //you redirect and transfer from within a Try..Catch
            if (ex is ThreadAbortException)
                return;





            ExceptionHelper exceptionHelper = null;
            string exceptionText = "";
            try
            {
                //Add session and path details
                AddAdditionalInformation(ex, nvc);

                //Log to database
                exceptionHelper = new ExceptionHelper();
                exceptionText = exceptionHelper.SaveException(ex);
                HttpContext.Current.Session["Exception"] = exceptionText;
            }
            catch
            {
                //Do nothing. Maybe email admin?
            }
            finally
            {
                exceptionHelper = null;
            }


        }

     
        public static void AddAdditionalInformation(Exception ex, NameValueCollection nvc)
        {
            try
            {


                if (nvc != null)
                {
                    foreach (string key in nvc.AllKeys)
                    {
                        ex.Data.Add(key, nvc.Get(key));
                    }
                }
            }
            catch (Exception ex2)
            {
                string exMessage = ex2.ToString();
            }
        }
    }


    
}