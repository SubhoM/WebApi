using Jcr.Api.ActionFilters;
using Jcr.Api.Helpers;
using Jcr.Api.Models.Models;
using Jcr.Business;
using Jcr.Business.MockSurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Jcr.Api.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("MockSurveyInfo")]
    public class MockSurveyController : ApiController
    {

        [AuthorizationRequired]
        [VersionedRoute("GetMSWorkFlows/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetMSWorkFlows()
        {
            try
            {
                var _lstMSWorkFlows = MSServices.GetMSWorkFlows();

                var _wrkFlwOptions = _lstMSWorkFlows.Select(m =>
                                    new SelectOption
                                    {
                                        Name = m.WorkFlowName,
                                        Value = m.MockSurveyWorkFlowID.ToString(),
                                        FullText = m.BoilerPlate
                                    }
                                    ).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, _wrkFlwOptions);

            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/MockSurveyInfo/GetMSWorkFlows");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

        [AuthorizationRequired]
        [VersionedRoute("GetMSCorporateSettings/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetMSCorporateSettings(int userID)
        {
            try
            {
                var MSCorpSettings = MSServices.GetAllCorpSettings(userID);

                return Request.CreateResponse(HttpStatusCode.OK, MSCorpSettings);
            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/MockSurveyInfo/GetMSCorporateSettings");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("UpdateMSWorkFlow/{id?}", 1)]
        [HttpPost]
        public HttpResponseMessage UpdateMSWorkFlow([FromBody] MSWrkFlWUpdate _msWrkFlWUpdate)
        {
            try
            {
                MSServices.UpdateMSWorkFlow(_msWrkFlWUpdate.mockSurveyWorkFlowId, _msWrkFlWUpdate.userId, _msWrkFlWUpdate.siteIDs);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ex.Data.Add("HTTPReferrer", "JCRAPI/MockSurveyInfo/GetMSCorporateSettings");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }
        }

        [AuthorizationRequired]
        [VersionedRoute("GetMockSurveyWorkFlowHelpFileStreamID/{id?}", 1)]
        [HttpGet]
        public HttpResponseMessage GetMockSurveyWorkFlowHelpFileStreamID()
        {
            var fileStreamID = FilesServices.GetFileStreamIDbyFileDisplayName("Mock Survey WorkFlow Manual");
            
            return Request.CreateResponse(HttpStatusCode.OK, fileStreamID);
        }
    }

}