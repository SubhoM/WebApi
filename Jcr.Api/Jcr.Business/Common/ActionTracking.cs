using Jcr.Data;
using System;

namespace Jcr.Business
{
    public class ActionTracking : IActionTracking
    {
        ExceptionLogServices exceptionLog = new ExceptionLogServices();
        int IActionTracking.AddAppEventLog(int? programId, int? siteId, int? actionTypeId, int? userId)
        {
            int _result;
            using (var db = new DBAMPContext())
            {
                try
                {
                    _result = db.ApiLogTracerActionSummaryByMonth(programId, siteId, actionTypeId, userId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiLogTracerActionSummaryByMonth(" + programId + "," + siteId + "," + actionTypeId + "," + userId + ")";
                    string methodName = "JCRAPI/Business/IActionTracking/AddAppEventLog";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    return 0;
                }

            }
            return _result;
        }

        int IActionTracking.AddAppEventLogDetail(int? programId, int? siteId, int? actionTypeId, int? userId, int? productId)
        {
            int _result;
            using (var db = new DBAMPContext())
            {
                try
                {
                    _result = db.ApiAddAppEventLogDetail(programId, siteId, actionTypeId, userId, productId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiAddAppEventLogDetail(" + programId + "," + siteId + "," + actionTypeId + "," + userId + "," + productId + ")";
                    string methodName = "JCRAPI/Business/IActionTracking/AddAppEventLogDetail";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    return 0;
                }
            }
            return _result;
        }

        int IActionTracking.AddAppExceptionLog(string exceptionText, string pageName, string methodName, int? userId, int? siteId, string transSql, string httpReferrer)
        {
            int _result;
            int? _exceptionLogId = 0;
            using (var db = new DBAMPContext())
            {
                try
                {
                    _result = db.ApiExceptionLogInsert(exceptionText, pageName, methodName, userId, siteId, transSql, httpReferrer, out _exceptionLogId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "AddAppExceptionLog(" + exceptionText + "," + pageName + "," + methodName + "," + userId + "," + siteId + "," + transSql + "," + httpReferrer + "," + ")";
                    string sMethodName = "JCRAPI/Business/IActionTracking/AddAppExceptionLog";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", sMethodName, userId, siteId, sqlParam, string.Empty);

                    return 0;
                }
            }
            return _result;
        }

        void IActionTracking.AddTracking()
        {
            throw new NotImplementedException();
        }
    }
}