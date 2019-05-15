using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jcr.Data;
namespace Jcr.Business
{
    public class SiteServices
    {

        ExceptionLogServices exceptionLog = new ExceptionLogServices();
        public IEnumerable<UspSiteSelectReturnModel.ResultSetModel1> GetSiteById(int siteId)
        {
            UspSiteSelectReturnModel uspsiteselect_result;
            using (var db = new DBAMPContext())
            {               
                try
                {
                    uspsiteselect_result = db.UspSiteSelect(siteId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "UspSiteSelect(" + siteId + ")";
                    string methodName = "JCRAPI/Business/SiteServices/GetSiteById";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);

                    return null;
                }

            }

            return uspsiteselect_result.ResultSet1;

        }

        public List<ApiSelectTracerSitesByUserReturnModel> GetTracerSitesByUser(int? userId, int? siteId, bool? filteredSites, bool isGuestUser = false)
        {
            List<ApiSelectTracerSitesByUserReturnModel> _result;
            using (var db = new DBAMPContext())
            {
               
                try
                {
                    _result = db.ApiSelectTracerSitesByUser(userId, siteId, filteredSites, isGuestUser);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSelectTracerSitesByUser(" + userId+ "," + siteId + ","+ filteredSites+")";
                    string methodName = "JCRAPI/Business/SiteServices/GetTracerSitesByUser";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    return null;
                }
            }

            return _result;            
        }

        public int GetSiteIDByTracerCustomID(int tracerCustomID)
        {
            int siteID = 0;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    var _result = db.ApiSelectTracerSiteByTracerCustomId(tracerCustomID);

                    if (_result != null && _result.Count > 0)
                        siteID = (int)_result[0].SiteID;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSelectTracerSiteByTracerCustomId(" + tracerCustomID + ")";
                    string methodName = "JCRAPI/Business/SiteServices/GetTracerSitesByUser";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return 0;
                }
            }

            return siteID;
        }

        public List<ApiSelectProgramsBySiteReturnModel> GetSelectProgramsBySite(int? userId, int? siteId, DateTime? standardEffBeginDate, int? productId)
        {
            List<ApiSelectProgramsBySiteReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {
              
                try
                {
                    _result = db.ApiSelectProgramsBySite(userId, siteId, standardEffBeginDate, productId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSelectProgramsBySite(" + userId + "," + siteId + "," + standardEffBeginDate + "," + productId + ")";
                    string methodName = "JCRAPI/Business/SiteServices/GetSelectProgramsBySite";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    return null;
                }
            }

            return _result;


        }


        public static List<ApiGetAllUserSitesReturnModel> GetUserSites(int userID)
        {

            List<ApiGetAllUserSitesReturnModel> _result;

            using (var db = new Data.DBAMPContext())
            {
                ExceptionLogServices exceptionLog = new ExceptionLogServices();
                try
                {
                    _result = db.ApiGetAllUserSites(userID);

                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUserSites(" + userID + ")";
                    string methodName = "JCRAPI/Business/SiteServices/ApiGetUserSites";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, 0, sqlParam, string.Empty);

                    return null;
                }

            }

            return _result;

        }

        public static List<ApiGetProgramsBySiteReturnModel> GetProgramsBySite(int siteID)
        {

            List<ApiGetProgramsBySiteReturnModel> _result;

            using (var db = new Data.DBMEdition01Context())
            {
                ExceptionLogServices exceptionLog = new ExceptionLogServices();
                try
                {
                    _result = db.ApiGetProgramsBySite(siteID);

                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUserSites(" + siteID + ")";
                    string methodName = "JCRAPI/Business/SiteServices/GetProgramsBySite";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, siteID, 0, sqlParam, string.Empty);

                    return null;
                }

            }

            return _result;

        }

    }
}
