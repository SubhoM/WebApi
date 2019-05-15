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

using System.Configuration;
using Jcr.Api.Models.Models;

namespace Jcr.Api.Controllers
{

    /// <summary>
    /// Get UserID by user LogonId
    /// </summary>
    /// 
    [RoutePrefix("UserInfo")]
    public class UserController : ApiController
    {
        [VersionedRoute("GetUserId/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserIdByLogonId(string logonId)
        {
            UserServices service = new UserServices();
            try
            {
                int userId = service.GetUserIdByLogonId(logonId);
                if (userId != 0)
                    return Request.CreateResponse(HttpStatusCode.OK, userId);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No user found for this Id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("logonId", logonId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetUserIdByLogonId");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }




        [VersionedRoute("AuthenticateTracerUserWithoutPassword/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage AuthenticateTracerUserWithoutPassword(string logonId)
        {
            UserServices service = new UserServices();
            try
            {
                string errMsg;
                int userId = service.AuthenticateTracerUserWithoutPassword(logonId, out errMsg);

                if (userId != 0)
                    return Request.CreateResponse(HttpStatusCode.OK, userId);
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, errMsg);
            }
            catch (Exception ex)
            {
                ex.Data.Add("logonId", logonId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/AuthenticateTracerUserWithoutPassword");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
        /// <summary>
        /// Get Full user info by logonId
        /// </summary>
        /// 
        [AuthorizationRequired]
        [VersionedRoute("GetUser/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserByLogonId(string logonId)
        {
            UserServices service = new UserServices();
            try
            {
                ApiSelectUserByUserLogonIdReturnModel user = service.GetUserByLogonId(logonId);
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No user found for this Id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("logonId", logonId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetUserByLogonId");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
        [AuthorizationRequired]
        [VersionedRoute("GetUserRoleBySite/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserRoleBySite(int userId, int siteId)
        {
            UserServices service = new UserServices();
            try
            {
                ApiGetUserRoleBySiteReturnModel rtn = service.GetUserRoleBySite(userId, siteId);
                if (rtn != null)
                    return Request.CreateResponse(HttpStatusCode.OK, rtn);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Role find for this User");
            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetUserRoleBySite");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
        [VersionedRoute("GetSQuestions/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserSecurityQuestions(int userId)
        {
            UserServices service = new UserServices();
            string rtnMsg = "You are a new user and have yet to set up your security questions, please contact technical support at 877-223-6866 (Please select option 2) or via email at support@jcrinc.com.";
            try
            {
                List<ApiGetUserSecurityQuestionsReturnModel> rtn = service.GetUserSecurityQuestions(userId);
                if (rtn != null)
                    return Request.CreateResponse(HttpStatusCode.OK, rtn);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, rtnMsg);
            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetSQuestions");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [VersionedRoute("VerifyAnswer/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage VerifySecurityAnswer([FromBody] List<SecurityQuestionAnswer> answerList, int userId)
        {
            UserServices service = new UserServices();
            bool rtn;
            try
            {
                rtn = service.VerifySecurityQuestionAnswer(answerList, userId);
                if (!rtn)
                {
                    var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The answers to your security questions do not match our records.");
                    response.Headers.Add("message", "The answers to your security questions do not match our records.");

                    return response;
                }

                else
                {
                    TokenServices tokenService = new TokenServices();
                    var token = tokenService.GenerateToken(userId);
                    var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
                    response.Headers.Add("Token", token.AuthToken);
                    response.Headers.Add("UserId", userId.ToString());
                    response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
                    response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                    return response;
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetSQuestions");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
        [AuthorizationRequired]
        [VersionedRoute("GetUserSecurityAttributes/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserSecurityAttributes(int userId)
        {
            UserServices service = new UserServices();
            string rtnMsg = "You are a new user and have yet to set up your security questions, please contact technical support at 877-223-6866 (Please select option 2) or via email at support@jcrinc.com.";
            try
            {
                List<ApiGetUserSecurityAttributesReturnModel> rtn = service.GetUserSecurityAttributes(userId);
                if (rtn != null)
                    return Request.CreateResponse(HttpStatusCode.OK, rtn);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, rtnMsg);
            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetUserSecurityAttributes");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
        [AuthorizationRequired]
        [VersionedRoute("UpdateUserPassword/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateUserPassword(int userId, string password, bool? isWithRules)
        {
            try
            {
                UserServices service = new UserServices();


                int _resut;
                string rtnMessage;
                if (isWithRules != null && isWithRules == true)
                {
                    _resut = service.UpdateUserPasswordWithRestrictionRules(userId, password, out rtnMessage);
                }
                else
                {
                    _resut = service.UpdateUserPassword(userId, password, out rtnMessage);
                }


                if (_resut == 1)
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, "Update Successed");
                    response.Headers.Add("message", rtnMessage);
                    return response;
                }
                else
                {
                    var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, rtnMessage);
                    //  response.Headers.Add("message", rtnMessage);
                    return response;
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/AddUserSecurityAttribute");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }
        [AuthorizationRequired]
        [VersionedRoute("UpdateUserSecurityAnswer/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateUserSecurityAnswer(int? userId, int? attributeTypeId, string attributeValue, int? parentCodeId)
        {
            try
            {
                UserServices service = new UserServices();


                int _resut;

                _resut = service.UpdateUserSecurityAnswer(userId, attributeTypeId, attributeValue, parentCodeId);


                if (_resut != 1)
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, "Update Successed");
                    return response;

                }
                else
                {
                    var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Update Failed");
                    return response;
                }


            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/UpdateUserSecurityAnswer");
                WebExceptionHelper.LogException(ex, null);

                return null;
            }

        }



        /// <summary>
        /// Save user's preference site, program
        /// </summary>
        /// 
        [AuthorizationRequired]
        [VersionedRoute("SaveSiteProgramPreference/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveSiteProgramPreference(int? userId, int? siteId, int? programId)
        {
            try
            {
                UserServices service = new UserServices();

                int _resut;

                _resut = service.SaveSiteProgramPreference(userId, siteId, programId);


                if (_resut != 1)
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, "Save Successed");
                    return response;

                }
                else
                {
                    var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Save Failed");
                    return response;
                }


            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/SaveSiteProgramPreference");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }


        [AuthorizationRequired]
        [VersionedRoute("AddUserSecurityAttribute/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddUserSecurityAttribute([FromBody] UserAttribute Attribute)
        {

            try
            {


                UserServices service = new UserServices();
                if (Attribute.AttributeActivationDate == null)
                {
                    Attribute.AttributeActivationDate = DateTime.Now;
                }
                if (Attribute.AttributeExpirationDate == null)
                {
                    Attribute.AttributeExpirationDate = DateTime.Now;
                }

                ApiAddUserSecurityAttributeReturnModel _result = new ApiAddUserSecurityAttributeReturnModel();

                _result = service.AddUserSecurityAttribute(Attribute.UserId, Attribute.AttributeTypeId, Attribute.AttributeValue, Attribute.AttributeActivationDate, Attribute.AttributeExpirationDate);

                if (_result != null)
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");

                    return Request.CreateResponse(HttpStatusCode.OK, _result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Add User Attribute Failed");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", Attribute.UserId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/AddUserSecurityAttribute");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }




        /// <summary>
        /// Augo-suggestion user email for given site list
        /// </summary>
        /// 
        [AuthorizationRequired]
        [VersionedRoute("GetEmailListBySiteList/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetEmailListBySiteList(string search, string siteList)
        {
            UserServices service = new UserServices();
            try
            {
                List<ApiGetEmailListBySiteListReturnModel> rtn = service.GetEmailListBySiteList(search, siteList);

                if (rtn.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, rtn);
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "");
            }
            catch (Exception ex)
            {
                ex.Data.Add("siteList", siteList);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetEmailListBySiteList");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("CheckUserLoginFirstAfterProductRelease/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage CheckUserLoginFirstAfterProductRelease(int eProductId, int userId)
        {
            UserServices service = new UserServices();
            try
            {
                var status = service.CheckUserLoginFirstAfterProductRelease(eProductId, userId);
                
                return Request.CreateResponse(HttpStatusCode.OK, new { IsUserFirstLogin = status });
            }
            catch (Exception ex)
            {
                ex.Data.Add("eProductId", eProductId);
                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/CheckUserLoginFirstAfterProductRelease");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        /// <summary>
        /// Save the User Preferences for a specific Site, Program and Preference Type
        /// </summary>
        [AuthorizationRequired]
        [VersionedRoute("SaveUserPreference/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage SaveUserPreference([FromBody] UserPreference userPref)
        {
            try
            {

                UserServices.UpdateUserPreference(userPref);

                return Request.CreateResponse(HttpStatusCode.OK);


            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/SaveUserPreference");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Get the User Preference value for a specific Site, Program and Preference Type
        /// </summary>
        [AuthorizationRequired]
        [VersionedRoute("GetUserPreference/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserPreference(int userID, int siteID, int programID, string preferenceType)
        {
            try
            {
                var userPreferenceValue = UserServices.GetUserPreference(userID, siteID, programID, preferenceType);

                return Request.CreateResponse(HttpStatusCode.OK, userPreferenceValue);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/UserInfo/GetUserPreference");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }


    }
}