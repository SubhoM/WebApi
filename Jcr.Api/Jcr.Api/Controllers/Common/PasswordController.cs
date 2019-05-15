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


namespace Jcr.Api.Controllers
{
    //[AuthorizationRequired]
   // [TempAuthorizationRequired]
    [RoutePrefix("PasswordInfo")]
    public class PasswordController : ApiController
    {
        [VersionedRoute("GetSecurityQuestions/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetSecurityQuestionsById(int? questionTypeID)
        {
            PasswordServices service = new PasswordServices();
            string rtnMsg = "You are a new user and have yet to set up your security questions, please contact technical support at 877-223-6866 (Please select option 2) or via email at support@jcrinc.com.";
            try {
                var questions = service.GetSecurityQuestionsById(questionTypeID);
                if (questions != null) {
                    return Request.CreateResponse(HttpStatusCode.OK, questions);
                } else {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, rtnMsg);
                }
            } catch (Exception ex)  {
                ex.Data.Add("QuestionTypeID", questionTypeID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/PasswordInfo/GetSecurityQuestions");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [VersionedRoute("GetUserByUserLogonID/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserByUserLogonID(string loginID) {
            PasswordServices service = new PasswordServices();
            string rtnMsg = "";
            try {
                var userInfo = service.GetUserByUserLogonID(loginID);
                if (userInfo != null) {
                    return Request.CreateResponse(HttpStatusCode.OK, userInfo);
                } else {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, rtnMsg);
                }
            } catch (Exception ex) {
                ex.Data.Add("loginID", loginID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/PasswordInfo/GetUserByUserLogonID");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
    }
}
