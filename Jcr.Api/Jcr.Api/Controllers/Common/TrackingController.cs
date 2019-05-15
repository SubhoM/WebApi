using Jcr.Api.ActionFilters;
using Jcr.Api.Helpers;
using Jcr.Api.Models.Models;
using Jcr.Business;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jcr.Api.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("TrackingInfo")]
    public class TrackingController : ApiController
    {
        protected TrackingRepository trackingService;
        public TrackingController()
        {
            trackingService = new TrackingRepository();
        }

        [VersionedRoute("AddAppEventLog/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddAppEventLog(int programId, int siteId, int actionTypeId, int userId, int productId)
        {
            try
            {
                int _result;

                //Update AppEventLog table
                _result = trackingService.AddAppEventLog(programId, siteId, actionTypeId, userId);

                if (_result != 1)
                {
                    //Update AppEventLogDetail table
                    _result = trackingService.AddAppEventLogDetail(programId, siteId, actionTypeId, userId, productId);
                    if (_result != 1)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Event log updated");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Event log update failed");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Event log update failed");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteId", siteId);
                ex.Data.Add("UserId", userId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TrackingInfo/AddAppEventLog");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [VersionedRoute("AddAppExceptionLog/{id?}", 1)]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddAppExceptionLog([FromBody]ExceptionLogInfo exceptionInfo)
        {
            try
            {
                int _result;

                //Update AppEventLog table
                _result = trackingService.AddAppExceptionLog(exceptionInfo.ExceptionText, exceptionInfo.PageName, exceptionInfo.MethodName, exceptionInfo.UserId, exceptionInfo.SiteId, exceptionInfo.TransSql, exceptionInfo.HttpReferrer);

                if (_result != 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Exception log updated");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Exception log update failed");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("SiteId", exceptionInfo.SiteId);
                ex.Data.Add("UserId", exceptionInfo.UserId);
                ex.Data.Add("HTTPReferrer", "JCRAPI/TrackingInfo/AddAppExceptionLog");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }
    }
}
