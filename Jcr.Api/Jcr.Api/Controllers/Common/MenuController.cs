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
    [AuthorizationRequired]
    [RoutePrefix("MenuInfo")]
    public class MenuController : ApiController
    {
        /// <summary>
        ///Menu State
        /// </summary>
        [VersionedRoute("GetMenuState/{id?}", 1)]
        [System.Web.Http.HttpGet] 
        public HttpResponseMessage GetMenuState(int? userId) {

            MenuServices service = new MenuServices();
            
            try {
                var _result = service.GetMenuState(userId);

                return Request.CreateResponse(HttpStatusCode.OK, _result);

            }
            catch (Exception ex) {
                ex.Data.Add("UserID", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/MenuInfo/GetMenuState");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Expectation Failed during Menu State SaveArg");
            }
        }


        /// <summary>
        ///Menu State
        /// </summary>
        [VersionedRoute("MenuStateSaveArg/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage MenuStateSaveArg(int? userId, string key, string value) {

            MenuServices service = new MenuServices();

            try {
                service.MenuStateSaveArg(userId, key, value);

                return Request.CreateResponse(HttpStatusCode.OK, "Menu State Save Are successed");

            }
            catch (Exception ex) {
                ex.Data.Add("UserID", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/MenuInfo/MenuStateSaveArg");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Expectation Failed during Menu State SaveArg");
            }

        }


        [VersionedRoute("Init/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Init([FromBody] Models.UserMenuState.Init userPref) {

            MenuServices service = new MenuServices();
            try {
                service.MenuStateInit(userPref);

                return Request.CreateResponse(HttpStatusCode.OK, "UserMenuState successfully inititialized.");
            }
            catch (Exception ex) {
                ex.Data.Add("UserID", userPref.UserID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/MenuInfo/MenuStateInit");
                WebExceptionHelper.LogException(ex, null);
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
    }
}