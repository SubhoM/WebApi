using Jcr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Business
{
    public class TrackingRepository
    {
        public IActionTracking ServiceProxy { get; set; }

        public TrackingRepository()
        {
            ServiceProxy = new ActionTracking();
        }

        public void AddTracking()
        {
            ServiceProxy.AddTracking();
        }

        public int AddAppEventLog(int? programId, int? siteId, int? actionTypeId, int? userId)
        {
            return ServiceProxy.AddAppEventLog(programId, siteId, actionTypeId, userId);
        }

        public int AddAppEventLogDetail(int? programId, int? siteId, int? actionTypeId, int? userId, int? productId)
        {
            return ServiceProxy.AddAppEventLogDetail(programId, siteId, actionTypeId, userId, productId);
        }

        public int AddAppExceptionLog(string exceptionText, string pageName, string methodName, int? userId, int? siteId, string transSql, string httpReferrer)
        {
            return ServiceProxy.AddAppExceptionLog(exceptionText, pageName, methodName, userId, siteId, transSql, httpReferrer);
        }
    }
}
