using Jcr.Api.Models.Models;
using Jcr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jcr.Business
{
	public class TracerCategoryServices
	{
		ExceptionLogServices exceptionLog;
		public TracerCategoryServices()
		{
			exceptionLog = new ExceptionLogServices();
		}

		public List<ApiGetTracerCategoryBySiteIdReturnModel> GetTracerCategoryBySite(int siteId)
		{

			try
			{
				List<ApiGetTracerCategoryBySiteIdReturnModel> _dbResult;

				using (var db = new DBMEdition01Context())
				{
					_dbResult = db.ApiGetTracerCategoryBySiteId(siteId);
				}

				return _dbResult;
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("EXEC dbo.ApiGetTracerCategoryBySiteId @SiteID='{0}'", siteId);
				string sqlParam = sb.ToString();
				string methodName = "JCRAPI/Business/TracerCategoryServices/GetTracerCategoryBySite";
				exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
				return null;
			}
		}
		public List<SelectOption> GetTracerCategorDropdownyBySite(int siteId)
		{

			try
			{
				var _dbResult = this.GetTracerCategoryBySite(siteId);

				var _result = _dbResult.Select(m =>
				new SelectOption
				{
					Value = m.TracerCategoryID.ToString(),
					Name = m.TracerCategoryName,
					IsDefault = m.IsDefault
				}
				).ToList();


				return _result;
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("EXEC dbo.ApiGetTracerCategoryBySiteId @SiteID='{0}'", siteId);
				string sqlParam = sb.ToString();
				string methodName = "JCRAPI/Business/TracerCategoryService/GetTracerCategorDropdownyBySite";
				exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
				return null;
			}
		}

		public int SaveTracerCategory(TracerCategory tracerCategory)
		{

			try
			{
				ApiSaveTracerCategoryReturnModel _dbResult;

				using (var db = new DBMEdition01Context())
				{
					_dbResult = db.ApiSaveTracerCategory(tracerCategory.TracerCategoryID,
						tracerCategory.TracerCategoryName, tracerCategory.SiteID,
						tracerCategory.UpdatedByID, false).FirstOrDefault();
				}

				return _dbResult == null ? 0 : (int)_dbResult.TracerCategoryID;
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("EXEC dbo.ApiSaveTracerCategory @TracerCategoryID='{0}'", tracerCategory.TracerCategoryID);
				sb.AppendFormat(",@TracerCategoryName='{0}'", tracerCategory.TracerCategoryName);
				sb.AppendFormat(",@SiteID='{0}'", tracerCategory.SiteID);
				sb.AppendFormat(",@UpdatedByID='{0}'", tracerCategory.UpdatedByID);
				string sqlParam = sb.ToString();
				string methodName = "JCRAPI/Business/TracerCategoryServices/GetTracerCategoryBySite";
				exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerCategory.SiteID, sqlParam, string.Empty);
				return 0;
			}
		}

		public bool DeleteTracerCategory(int tracerCategoryId, int siteId, int updatedById, int? newTracerCategoryId = null)
		{

			try
			{
				var _dbResult = new ApiDeleteTracerCategoryReturnModel();

				using (var db = new DBMEdition01Context())
				{
					_dbResult = db.ApiDeleteTracerCategory(tracerCategoryId, siteId, updatedById, newTracerCategoryId).FirstOrDefault();
				}

				return _dbResult == null ? false : (bool)_dbResult.Result;
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("EXEC dbo.ApiDeleteTracerCategory @TracerCategoryID='{0}'", tracerCategoryId);
				sb.AppendFormat(",@SiteID='{0}'", siteId);
				sb.AppendFormat(",@NewTracerCategoryID='{0}'", newTracerCategoryId);
				string sqlParam = sb.ToString();
				string methodName = "JCRAPI/Business/TracerCategoryServices/DeleteTracerCategory";
				exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
				return false;
			}
		}

		public bool SetDefaultTracerCategoryForSite(int tracerCategoryId, int siteId, int updatedById)
		{

			try
			{
				var _dbResult = new ApiSetDefaultTracerCategoryForSiteReturnModel();

				using (var db = new DBMEdition01Context())
				{
					_dbResult = db.ApiSetDefaultTracerCategoryForSite(tracerCategoryId, siteId, updatedById).FirstOrDefault();
				}

				return _dbResult == null ? false : (bool)_dbResult.Result;
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("EXEC dbo.ApiSetDefaultTracerCategoryForSite @TracerCategoryID='{0}'", tracerCategoryId);
				sb.AppendFormat(",@SiteID='{0}'", siteId);
				string sqlParam = sb.ToString();
				string methodName = "JCRAPI/Business/TracerCategoryServices/SetDefaultTracerCategoryForSite";
				exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
				return false;
			}
		}

	}
}
