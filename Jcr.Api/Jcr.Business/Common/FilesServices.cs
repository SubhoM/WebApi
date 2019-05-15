using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;

using Jcr.Api.Models;
using Jcr.Data;

namespace Jcr.Business
{
    public class FilesServices
    {
        public static string SaveAttachmentFile(string fileName, byte[] p2, string appCode)
        {
            UsmCcmInsertFileReturnModel rtn;
            ExceptionLogServices exceptionLog = new ExceptionLogServices();

            using (var db = new DBFileTableContext())
            {
                //rtn = db.usmc(fileName, p2,).FirstOrDefault().stream_id.ToString();
                //   rtn = _result.ToString();

                try
                {
                    rtn = db.UsmCcmInsertFile(fileName, p2, appCode).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "UsmCcmInsertFile(" + fileName + "," + p2 + "," + appCode + ")";
                    string methodName = "JCRAPI/Business/FilesServices/SaveAttachmentFile";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }
            }

            return rtn.stream_ID.ToString();

        }

        public static Guid? GetFileStreamIDbyFileDisplayName(string FileDisplayName)
        {
            var ampDbEntityContainer = new DBMEdition01Context();

            var result = ampDbEntityContainer.ApiGetFileStreamIDbyFileDisplayName(FileDisplayName).FirstOrDefault().FileStreamID;

            return result;
        }
    }
}
