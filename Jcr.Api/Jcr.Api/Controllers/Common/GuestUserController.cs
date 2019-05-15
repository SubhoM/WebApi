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
using System.IO;
using System.Text.RegularExpressions;

namespace Jcr.Api.Controllers
{
    [RoutePrefix("GuestUser")]
    public class GuestUserController : ApiController
    {
        protected GuestUserServices guestUserService;
        public GuestUserController()
        {
            guestUserService = new GuestUserServices();

        }

        [VersionedRoute("GetGuestLink/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetGuestLink(string lnk)
        {
            try
            {
                string errMsg = string.Empty;

                GuestLink guestLink = guestUserService.LookupByGuestLink(lnk);

                if (guestLink.GuestLinkGUID == Guid.Empty)
                    errMsg = "No valid guest links available. Please contact your Program Administrator";

                if (string.IsNullOrEmpty(errMsg))
                    return Request.CreateResponse(HttpStatusCode.OK, guestLink);
                else
                    return Request.CreateResponse(HttpStatusCode.Forbidden, errMsg);
            }
            catch (Exception ex)
            {

                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [VersionedRoute("GetTaskLinkInfo/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTaskLinkInfo(string lnk)
        {
            try
            {
                string errMsg = string.Empty;

                ApiGetTaskLinkDetailsReturnModel taskLink = guestUserService.LookupByTaskLink(lnk);

                return Request.CreateResponse(HttpStatusCode.OK, taskLink);

            }
            catch (Exception ex)
            {

                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [VersionedRoute("CreateTracersGuestUser/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage CreateTracersGuestUser(string userLogonId, string firstName, string lastName, int? siteId = null)
        {

            try
            {
                string errMsg;

                GuestUserInfo rtn = guestUserService.CreateTracersGuestUser(userLogonId, firstName, lastName, siteId);

                return Request.CreateResponse(HttpStatusCode.OK, rtn);

            }
            catch (Exception ex)
            {

                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");

            }
        }

        [VersionedRoute("ValidateTracerGuestUserByEmail/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage ValidateTracerGuestUserByEmail(string email, int siteID = 0)
        {
            ApiMobileValidateGuestUserReturnModel rtn = new ApiMobileValidateGuestUserReturnModel();
            try
            {

                string errMsg = string.Empty;

                rtn = guestUserService.ValidateGuestUser(email, siteID, out errMsg);


                if (string.IsNullOrEmpty(errMsg))
                    return Request.CreateResponse(HttpStatusCode.OK, rtn);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);

            }
            catch (Exception ex)
            {
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There was an error validating the user. Please contact your Program Administrator.");

            }
        }

        [VersionedRoute("IsUserRegistered/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage IsUserRegistered(string email)
        {
            try
            {

                bool rtn = guestUserService.IsUserRegistered(email);

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {

                ex.Data.Add("email", email);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GuestUser/GetGuestUserTracersBySiteProgram");
                WebExceptionHelper.LogException(ex, null);
                return null;

            }
        }

        [VersionedRoute("IsUserRegisteredByUserId/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage IsUserRegisteredByUserId(int userId)
        {
            try
            {

                bool rtn = guestUserService.IsUserRegistered(userId);

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {

                ex.Data.Add("userId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GuestUser/IsUserRegisteredByUserId");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There was an error. Please contact your Program Administrator.");
            }
        }

        [VersionedRoute("CreateEmailCode/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage CreateEmailCode(string email)
        {
            try
            {
                string rtn = guestUserService.CreatePasscode(email);

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {

                ex.Data.Add("email", email);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GuestUser/GetGuestUserTracersBySiteProgram");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [VersionedRoute("GuestLinkAccessed/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GuestLinkAccessed(int userId, Guid guestLinkGuid)
        {

            TracerService tracerService = new TracerService();
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
        [VersionedRoute("InsertGuestUserEULA/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage InsertGuestUserEULA([FromBody] UserAttribute Attribute)
        {

            try
            {

                int attributeTypeId_EULA = 304;
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
                //This method is for insert Guest User EULAT Only, attributetype code is hardcoded to 304
                _result = service.AddUserSecurityAttribute(Attribute.UserId, attributeTypeId_EULA, Attribute.AttributeValue, Attribute.AttributeActivationDate, Attribute.AttributeExpirationDate);

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
        
        [AuthorizationRequired]
        [VersionedRoute("GetTracerEULA/{id?}", 1)]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTracerEULA()
        {
            try
            {
                string html;
                String _result = "";

                using (var reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(@"~\GuestAccess\_EULA_Mobile.cshtml")))
                {
                    html = reader.ReadToEnd();
                }
                html = Regex.Replace(html, @"\r\n?|\n", "");

                _result = "{ Html: '" + html + "'}";
                if (_result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No EULA found");

                return Request.CreateResponse(HttpStatusCode.OK, _result);
            }
            catch (Exception ex)
            {
                ex.Data.Add("EULA", "EULA load error");
                ex.Data.Add("HTTPReferrer", "JCRAPI/TracerInfo/GetObservationQuestionDetail");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }
                        
        [AuthorizationRequired(ParameterName="siteId")]
        [VersionedRoute("GetGuestUserTracersBySiteProgram/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetGuestUserTracersBySiteProgram(int? userId, int? siteId, int? programId, int? statusId)
        {
            try
            {
                List<ApiGuestUserTracerDetailSelectBySiteProgramReturnModel> rtn = guestUserService.GetGuestUserTracersBySiteProgram(userId, siteId, programId, statusId);

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {
                ex.Data.Add("userId", userId);
                ex.Data.Add("siteId", siteId);
                ex.Data.Add("programId", programId);
                ex.Data.Add("statusId", statusId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GuestUser/GetGuestUserTracersBySiteProgram");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("GetGuestUserSites/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetGuestUserSites(int userId, string email)
        {
            try
            {
                List<ApiMobileGuestUserSelectSitesReturnModel> rtn = guestUserService.GetGuestUserSites(userId, email);
                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {

                ex.Data.Add("email", email);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GuestUser/GetGuestUserSites");
                WebExceptionHelper.LogException(ex, null);
                return null;

            }
        }


        [AuthorizationRequired(ParameterName = "siteId")]
        [VersionedRoute("GetGuestUserProgramsBySiteId/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetGuestUserProgramsBySiteId(int userId, int siteId, int productId)
        {
            try
            {
                List<ApiMobileGuestUserSelectProgramsBySiteReturnModel> rtn = guestUserService.GetGuestUserProgramsBySiteId(userId, siteId, productId);
                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {

                ex.Data.Add("UserId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GuestUser/GetGuestUserProgramsBySiteId");
                WebExceptionHelper.LogException(ex, null);
                return null;

            }
        }
        
    }
}