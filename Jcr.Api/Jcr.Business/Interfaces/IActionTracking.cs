namespace Jcr.Business
{
    public interface IActionTracking
    {
        void AddTracking();

        int AddAppEventLog(int? programId, int? siteId, int? actionTypeId, int? userId);

        int AddAppEventLogDetail(int? programId, int? siteId, int? actionTypeId, int? userId, int? productId);

        int AddAppExceptionLog(string exceptionText, string pageName, string methodName, int? userId, int? siteId, string transSql, string httpReferrer);
    }
}