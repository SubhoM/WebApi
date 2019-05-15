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
	[RoutePrefix("TracerCategoryInfo")]
	public class TracerCategoryController : ApiController
	{

		[AuthorizationRequired]
		[Route("GetTracerCategoryDropdownBySite/{siteId?}")]
		[HttpGet]
		public HttpResponseMessage GetTracerCategoryDropdownBySite(int siteId)
		{
			try
			{
				var service = new TracerCategoryServices();
				var _result = service.GetTracerCategorDropdownyBySite(siteId);

				return Request.CreateResponse(HttpStatusCode.OK, _result);

			}
			catch (Exception ex)
			{
				ex.Data.Add("HTTPReferrer", "JCRAPI/TracerCategoryInfo/GetTracerCategoryDropdownBySite");
				WebExceptionHelper.LogException(ex, null);
				return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Empty);
			}

		}

		[AuthorizationRequired]
		[Route("GetTracerCategoryBySite/{siteId?}")]
		[HttpGet]
		public HttpResponseMessage GetTracerCategoryBySite(int siteId)
		{
			try
			{
				var service = new TracerCategoryServices();
				var _result = service.GetTracerCategoryBySite(siteId);

				return Request.CreateResponse(HttpStatusCode.OK, _result);

			}
			catch (Exception ex)
			{
				ex.Data.Add("HTTPReferrer", "JCRAPI/TracerCategoryInfo/GetTracerCategoryBySite");
				WebExceptionHelper.LogException(ex, null);
				return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Empty);
			}

		}

		[AuthorizationRequired]
		[Route("SaveTracerCategory")]
		[HttpPost]
		public HttpResponseMessage SaveTracerCategory([FromBody]TracerCategory tracerCategory)
		{
			try
			{
				var service = new TracerCategoryServices();
				var _result = service.SaveTracerCategory(tracerCategory);

				return Request.CreateResponse(HttpStatusCode.OK, _result);

			}
			catch (Exception ex)
			{
				ex.Data.Add("HTTPReferrer", "JCRAPI/TracerCategoryInfo/SaveTracerCategory");
				WebExceptionHelper.LogException(ex, null);
				return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Empty);
			}

		}

		[AuthorizationRequired]
		[Route("DeleteTracerCategory")]
		[HttpPost]
		public HttpResponseMessage DeleteTracerCategory(int tracerCategoryId, int siteId, int updatedById, int? newTracerCategoryId = null)
		{
			try
			{
				var service = new TracerCategoryServices();
				var _result = service.DeleteTracerCategory(tracerCategoryId, siteId, updatedById, newTracerCategoryId);

				return Request.CreateResponse(HttpStatusCode.OK, _result);

			}
			catch (Exception ex)
			{
				ex.Data.Add("HTTPReferrer", "JCRAPI/TracerCategoryInfo/DeleteTracerCategory");
				WebExceptionHelper.LogException(ex, null);
				return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Empty);
			}

		}

		[AuthorizationRequired]
		[Route("SetDefaultTracerCategoryForSite")]
		[HttpPost]
		public HttpResponseMessage SetDefaultTracerCategoryForSite(int tracerCategoryId, int siteId, int updatedById)
		{
			try
			{
				var service = new TracerCategoryServices();
				var _result = service.SetDefaultTracerCategoryForSite(tracerCategoryId, siteId, updatedById);

				return Request.CreateResponse(HttpStatusCode.OK, _result);

			}
			catch (Exception ex)
			{
				ex.Data.Add("HTTPReferrer", "JCRAPI/TracerCategoryInfo/SetDefaultTracerCategoryForSite");
				WebExceptionHelper.LogException(ex, null);
				return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Empty);
			}

		}

	}
}
