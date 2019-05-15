using Jcr.Data;
using Jcr.Api.Enumerators;
namespace Jcr.Business
{
    public class PasswordServices
    {
        ExceptionLogServices exceptionLog = new ExceptionLogServices();

        public ApiSelectSecurityQuestionsByIdReturnModel GetSecurityQuestionsById(int? questionTyeId)
        {
            ApiSelectSecurityQuestionsByIdReturnModel _result;
            using (var db = new DBAMPContext())
            {
                try
                {
                    _result = db.ApiSelectSecurityQuestionsById(questionTyeId);
                }
                catch (System.Exception ex)
                {
                    string sqlParam = "ApiSelectSecurityQuestionsById(" + questionTyeId + ")";
                    string methodName = "JCRAPI/Business/PasswordServices/GetSecurityQuestionsById";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }

        public ApiCodeSelectByIdReturnModel GetQuestions(int questionTyeId)
        {
            var codeCategoryId = (int)Enums.CodeCategoryEnum.SecurityQuestions;


            ApiCodeSelectByIdReturnModel _result;
            using (var db = new DBAMPContext())
            {

                try
                {
                    _result = db.ApiCodeSelectById(codeCategoryId, questionTyeId);
                }
                catch (System.Exception ex)
                {

                    string sqlParam = "ApiCodeSelectById(" + codeCategoryId + "," + questionTyeId + ")";
                    string methodName = "JCRAPI/Business/PasswordServices/GetQuestions";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }

        public ApiGetUserByUserLogonIdReturnModel GetUserByUserLogonID(string loginID) {
            ApiGetUserByUserLogonIdReturnModel _result;

            using (var db = new DBAMPContext()) {
                try {
                    _result = db.ApiGetUserByUserLogonId(loginID);
                } catch (System.Exception ex) {
                    string sqlParam = "ApiGetUserByUserLogonID(" + loginID + ")";
                    string methodName = "JCRAPI/Business/PasswordServices/GetUserByUserLogonID";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }
    }
}
