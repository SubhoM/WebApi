using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jcr.Business;
using Jcr.Data;
using Jcr.Api.ActionFilters;
using Jcr.Api.Helpers;
using Jcr.Api.Filters;
using System.Configuration;

namespace Jcr.Api.Controllers
{


    //[RoutePrefix("v1/Sites")]

    [RoutePrefix("GetCommonInfo")]
    public class SiteController : ApiController
    {

        // GET api/<controller>/5
        //[Route("siteid/{id?}")]
        // GET api/documentation
        /// <summary>
        /// Get Tracer Sites by passing UserId and SiteId
        /// </summary>
        /// <returns></returns>
        [AuthorizationRequired]
        [VersionedRoute("site/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTracerSitesByUser(int? userId, int? siteId, bool? filteredsites = false)
        {
            if (siteId == null) siteId = 0;
            SiteServices siteService = new SiteServices();

            try
            {
                var sites = siteService.GetTracerSitesByUser(userId, siteId, filteredsites);
                if (sites != null && sites.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, sites);

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User does not have access.");
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserLogonID", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetTracerSitesByUser");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        /// <summary>
        /// Return Programs by UserID/SiteID/CycleID(optional)
        /// </summary>
        [AuthorizationRequired]
        [VersionedRoute("program/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetSelectProgramsBySite(int? userId, int? siteId, DateTime? standardEffBeginDate, int? productId)
        {

            SiteServices siteService = new SiteServices();
            try
            {
                var programs = siteService.GetSelectProgramsBySite(userId, siteId, standardEffBeginDate, productId);
                if (programs != null && programs.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, programs);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No program found for this id");
            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteID", siteId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetSelectProgramsBySite");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }

        /// <summary>
        /// Return Session Timeout value in minutes
        /// </summary>
        [AuthorizationRequired]
        [VersionedRoute("SessionTime/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetSessionTimeout()
        {
            string rtn = string.Empty;


            try
            {
                if (ConfigurationManager.AppSettings["SessionTimeout"] != null)
                    rtn = ConfigurationManager.AppSettings["SessionTimeout"].ToString();

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {

                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetSessionTimeout");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }

        /// <summary>
        /// Return Tracer HELP PDF
        /// </summary>
        [AuthorizationRequired]
        [VersionedRoute("GetTracerHelpPDF/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTracerHelpPDF(bool? IsGuestUser = false)
        {
            string rtn = string.Empty;

            try
            {
                if (IsGuestUser == true)
                {
                    if (ConfigurationManager.AppSettings["TracerHelpPdfForGuest"] != null)
                        rtn = ConfigurationManager.AppSettings["TracerHelpPdfForGuest"].ToString();
                }
                else
                {
                    if (ConfigurationManager.AppSettings["TracerHelpPDF"] != null)
                        rtn = ConfigurationManager.AppSettings["TracerHelpPDF"].ToString();
                }

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetTracerHelpPDF");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        /// <summary>
        /// Return Tracer HELP Video
        /// </summary>
        [AuthorizationRequired]
        [VersionedRoute("GetTracerHelpVideo/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTracerHelpVideo(bool? IsGuestUser = false)
        {
            string rtn = string.Empty;

            try
            {
                if (IsGuestUser == true)
                {
                    if (ConfigurationManager.AppSettings["TracerHelpVideoForGuest"] != null)
                        rtn = ConfigurationManager.AppSettings["TracerHelpVideoForGuest"].ToString();
                }
                else
                {
                    if (ConfigurationManager.AppSettings["TracerHelpVideo"] != null)
                        rtn = ConfigurationManager.AppSettings["TracerHelpVideo"].ToString();
                }

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetTracerHelpVideo");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        /// <summary>
        /// Return Tracer App store version
        /// </summary>
        [VersionedRoute("GetTracerAppVersion/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetTracerAppVersion()
        {
            string rtn = string.Empty;

            try
            {
                if (ConfigurationManager.AppSettings["TracersMobileAppVersion"] != null)
                    rtn = ConfigurationManager.AppSettings["TracersMobileAppVersion"].ToString();

                return Request.CreateResponse(HttpStatusCode.OK, rtn);
            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetTracerAppVersion");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }


        /// <summary>
        /// Get Sites for User
        /// </summary>
        /// 
        [AuthorizationRequired]
        [VersionedRoute("GetUserSites/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetUserSites(int userID)
        {
            try
            {
                //List of Sites where User has either AMP or Tracers Access
                var _result = SiteServices.GetUserSites(userID).Where(m => m.IsAMPAccess == 1 || m.IsTracersAccess == 1);
                
                return Request.CreateResponse(HttpStatusCode.OK, _result);
            }
            catch (Exception ex)
            {
                ex.Data.Add("userID", userID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetUserSites");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        /// <summary>
        /// Get Programs by Site
        /// </summary>
        /// 
        [AuthorizationRequired(ParameterName = "siteID")]
        [VersionedRoute("GetProgramsBySite/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetProgramsBySite(int siteID)
        {
            try
            {
                var _result = SiteServices.GetProgramsBySite(siteID);

                return Request.CreateResponse(HttpStatusCode.OK, _result);
            }
            catch (Exception ex)
            {
                ex.Data.Add("siteID", siteID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/GetCommonInfo/GetProgramsBySite");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

    }
}
