using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jcr.Data;
namespace Jcr.Business
{
    public class ExceptionLogServices
    {

        public void ExceptionLogInsert(string exceptionText, string pageName, string methodName, int? userId, int? siteId, string transSql, string httpReferrer)
        {

            using (var db = new DBAMPContext())
            {
               int? exceptionLogId;
                exceptionLogId = db.ApiExceptionLogInsert( exceptionText,  pageName,  methodName, userId, siteId, transSql, httpReferrer, out exceptionLogId);
            }
                    }
    }
}
