using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jcr.Data;
using Jcr.Api.Enumerators;

namespace Jcr.Business
{
    public class MenuServices
    {
        ExceptionLogServices exceptionLog = new ExceptionLogServices();
        public ApiMenuStateGetReturnModel GetMenuState(int? userId) {
            using (var db = new DBMEdition01Context()) {

                ApiMenuStateGetReturnModel result;

                try
                {
                  
                    result = db.ApiMenuStateGet(userId).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMenuStateGet(" + userId +")";
                    string methodName = "JCRAPI/Business/MenuServices/GetMenuState";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return null;
                }
                return result;
            }
        }

        public void MenuStateSaveArg(int? userId, string key, string value)
        {
            using (var db = new DBMEdition01Context())
            {
                int result;
               
                try
                {
                    result = db.ApiMenuStateSaveArg(userId, key, value);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMenuStateSaveArg(" + userId + "," + key + "," + value+ ")";
                    string methodName = "JCRAPI/Business/MenuServices/MenuStateSaveArg";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                }

            }
        }

        public void MenuStateInit(Api.Models.UserMenuState.Init userPref) {
            using (var db = new DBMEdition01Context()) {
                int result;
               
                try
                {
                    result = db.ApiMenuStateInit(userPref.UserID, userPref.SiteID, userPref.RoleID, userPref.AccessToEdition,
                   userPref.AccessToAMP, userPref.AccessToTracers, userPref.AccessToERAMP, userPref.AccessToERTracers);

                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMenuStateInit(" + userPref.UserID + "," + userPref.SiteID + "," + userPref.RoleID + "," + userPref.AccessToEdition + "," + userPref.AccessToAMP + "," + userPref.AccessToTracers + "," + userPref.AccessToERAMP + "," + userPref.AccessToERTracers + ")";
                    string methodName = "JCRAPI/Business/MenuServices/MenuStateInit";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userPref.UserID, userPref.SiteID, sqlParam, string.Empty);
                    
                }

            }
        }
    }
}
