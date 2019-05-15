using System;
using System.Collections.Generic;
using System.Linq;
using Jcr.Api.Enumerators;
using Jcr.Api.Models;
using Jcr.Data;
using Jcr.Api.Helpers;
using System.Text;
using Jcr.Api.Models.Models;
using System.Configuration;

namespace Jcr.Business
{
    public class UserServices
    {
        ExceptionLogServices exceptionLog = new ExceptionLogServices();

        /// Public method to authenticate user by user name and password.

        public int Authenticate(string userLogin, string password, int? subscriptionTypeId, out string invalidMsg)
        {
            ApiValidateUserReturnModel user;
            int rtn;
            using (var db = new DBAMPContext())
            {

                try
                {

                    user = db.ApiValidateUser(userLogin, password, subscriptionTypeId, out rtn).FirstOrDefault();

                    if (rtn > 0)
                    {
                        if (rtn == 3)
                            invalidMsg = "Invalid username and/or password.";
                        else if (rtn == 4)
                            invalidMsg = "User does not have access.";
                        else
                            invalidMsg = "Other Errors";

                        return 0;
                    }
                    else
                    {
                        invalidMsg = string.Empty;

                        return user.UserID;
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiValidateUser(" + userLogin + "," + password + "," + subscriptionTypeId + ", out rtn)";
                    string methodName = "JCRAPI/Business/UserServices/Authenticate";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    invalidMsg = "Other Errors";
                    return 0;
                }
            }


        }

        public int AuthenticateGuest(string userLogin, string password, out string invalidMsg)
        {
            var userID = GetUserIdByLogonId(userLogin);

            using (var db = new DBMEdition01Context())
            {

                try
                {
                    var rtnPassCode = db.ApiUpdateSelectPasscode(userID, userID, 0).FirstOrDefault();

                    var passCodeDB = rtnPassCode.Passcode.HasValue ? rtnPassCode.Passcode.Value : 0;

                    if (rtnPassCode != null && string.Equals(password, passCodeDB.ToString(), StringComparison.InvariantCulture))
                    {
                        invalidMsg = string.Empty;
                        return userID;
                    }
                    else
                    {
                        invalidMsg = "Invalid Passcode";
                        return 0;
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiUpdateSelectPasscode(" + userID + "," + userID + "," + password + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/CreatePasscode";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, null, sqlParam, string.Empty);
                    invalidMsg = "There was an error authenticating the user. Please contact your Program Administrator.";
                    return 0;
                }
            }
        }

        public int AuthenticateTracerUserWithoutPassword(string userLogin, out string invalidMsg)
        {

            int rtn = 0;
            using (var db = new DBAMPContext())
            {
                List<ApiTracerValidateUserWithNoPasswordReturnModel> rtnData;

                try
                {
                    ApiTracerValidateUserWithNoPasswordReturnModel user;

                    rtnData = db.ApiTracerValidateUserWithNoPassword(userLogin);

                    if (rtnData.Count > 0)
                    {

                        user = rtnData.FirstOrDefault();
                        if (user.UserID != null && user.ErrorCode == null)
                        {
                            rtn = (int)user.UserID;
                            invalidMsg = string.Empty;
                        }
                        else
                        {
                            if (user.UserID == null)
                            {
                                invalidMsg = "Invalid user.";
                            }
                            else if (user.ErrorCode == 4)
                                invalidMsg = "User does not have access. Password cannot be reset.";
                            else
                                invalidMsg = "Other Errors";
                        }

                    }
                    else
                    {
                        invalidMsg = "Invalid user";
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiTracerValidateUserWithNoPassword(" + userLogin + ")";
                    string methodName = "JCRAPI/Business/UserServices/AuthenticateTracerUserWithoutPassword";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    invalidMsg = "Other Errors";
                    return 0;
                }
            }
            return rtn;

        }

        public int GetUserIdByLogonId(string userLogin)
        {
            using (var db = new DBAMPContext())
            {
                ApiSelectUserIdByUserLogonIdReturnModel user;

                try
                {

                    user = db.ApiSelectUserIdByUserLogonId(userLogin).FirstOrDefault();

                    if (user != null)
                    {
                        return user.UserID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSelectUserIdByUserLogonId(" + userLogin + ")";
                    string methodName = "JCRAPI/Business/UserServices/GetUserIdByLogonId";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return 0;
                }

            }

        }
        public ApiSelectUserByUserLogonIdReturnModel GetUserByLogonId(string userLogin)
        {
            using (var db = new DBMEdition01Context())
            {
                ApiSelectUserByUserLogonIdReturnModel user;


                try
                {
                    user = db.ApiSelectUserByUserLogonId(userLogin).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSelectUserByUserLogonId(" + userLogin + ")";
                    string methodName = "JCRAPI/Business/UserServices/GetUserByLogonId";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }
                return user;
            }

        }
        public bool VerifySecurityQuestionAnswer(List<SecurityQuestionAnswer> securityQuestionAnswers, int userID)
        {
            bool rtn = false;

            foreach (var item in securityQuestionAnswers)
            {
                rtn = ValidateSecurityQuestionAnswer(userID, item.QuestionId, item.Answer);
                if (!rtn)
                {
                    break;
                }

            }
            return rtn;
        }

        public bool ValidateSecurityQuestionAnswer(int? userId, int? questionId, string answer)
        {
            bool rtn = false;
            ApiValidateSecurityQuestionAnswerReturnModel.ResultSetModel1 _result;

            using (var db = new DBAMPContext())
            {

                try
                {
                    _result = db.ApiValidateSecurityQuestionAnswer(userId, questionId, answer).ResultSet1.FirstOrDefault();

                    if (_result.ValidateAnswer.ToUpper() == "TRUE")
                        rtn = true;
                }
                catch (Exception ex)
                {
                    string sqlParam = "ApiValidateSecurityQuestionAnswer(" + userId + "," + questionId + "," + answer + ")";
                    string methodName = "JCRAPI/Business/UserServices/ValidateSecurityQuestionAnswer";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return false;
                }
                return rtn;

            }
        }
        public List<ApiGetUserSecurityQuestionsReturnModel> GetUserSecurityQuestions(int? userId)
        {
            int codeCategoryId = (int)Jcr.Api.Enumerators.Enums.CodeCategoryEnum.SecurityQuestions;
            int questionTyeId1 = (int)Jcr.Api.Enumerators.Enums.UserSecurityAttributeType.PasswordAnswer1;
            int questionTyeId2 = (int)Jcr.Api.Enumerators.Enums.UserSecurityAttributeType.PasswordAnswer2;
            List<ApiGetUserSecurityQuestionsReturnModel> _result;

            using (var db = new DBAMPContext())
            {

                try
                {
                    _result = db.ApiGetUserSecurityQuestions(userId, codeCategoryId, questionTyeId1, questionTyeId2);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUserSecurityQuestions(" + userId + "," + codeCategoryId + "," + questionTyeId1 + "," + questionTyeId2 + ")";
                    string methodName = "JCRAPI/Business/UserServices/GetUserSecurityQuestions";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return null;
                }

            }

            return _result;
        }
        public List<ApiGetUserSecurityAttributesReturnModel> GetUserSecurityAttributes(int? userId)
        {
            int codeCategoryId = (int)Jcr.Api.Enumerators.Enums.CodeCategoryEnum.SecurityQuestions;
            int questionTyeId1 = (int)Jcr.Api.Enumerators.Enums.UserSecurityAttributeType.PasswordAnswer1;
            int questionTyeId2 = (int)Jcr.Api.Enumerators.Enums.UserSecurityAttributeType.PasswordAnswer2;
            List<ApiGetUserSecurityAttributesReturnModel> _result;

            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiGetUserSecurityAttributes(userId, codeCategoryId, questionTyeId1, questionTyeId2);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUserSecurityAttributes(" + userId + "," + codeCategoryId + "," + questionTyeId1 + "," + questionTyeId2 + ")";
                    string methodName = "JCRAPI/Business/UserServices/GetUserSecurityAttributes";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return null;
                }

            }

            return _result;
        }

        public int UpdateUserPassword(int userId, string password, out string rtnMessage)
        {
            int rtn;
            try
            {
                // validatePassword

                password = Encoding.Default.GetString(Convert.FromBase64String(password));

                if (password.Length < 6)
                {
                    rtnMessage = "A new password between 6 and 10 characters is required...";
                    rtn = 0;
                    return rtn;
                }


                char[] anyChars = { ';', '\'', '-' }; // special chars

                int index = password.IndexOfAny(anyChars);
                if (index >= 0)
                {
                    rtnMessage = "Password should not contain special character ';', '\'', '-'";
                    rtn = 0;
                }
                else
                {
                    password = CryptHelpers.Encrypt(password.Trim(), WebConstants.EncryptionKey);
                    int passwordResetInterval = GetPasswordResetInterval(userId);
                    DateTime expirationDate = DateTime.Now.AddDays(passwordResetInterval);
                    UpdateUserSecurityAttribute(userId, (int)Enums.UserSecurityAttributeType.Password, password, DateTime.Now, expirationDate);
                    UpdateUserSecurityAttribute(userId, (int)Enums.UserSecurityAttributeType.ForcePasswordReset, "FALSE", DateTime.Now, DateTime.Now);

                    rtnMessage = "Your password was successfully reset. Use the new password to login.";

                    rtn = 1;
                }
            }
            catch (Exception ex)
            {
                if (password.Length < 6)
                    rtnMessage = "A new password between 6 and 10 characters is required...";
                else
                    rtnMessage = "Password should not contain special character ';', '\'', '-'";


                rtn = 0;
            }

            return rtn;

        }

        public int UpdateUserPasswordWithRestrictionRules(int userId, string password, out string rtnMessage)
        {
            int rtn;
            try
            {
                // validatePassword

                password = Encoding.Default.GetString(Convert.FromBase64String(password));
                rtnMessage = ValidatePasswordRules(userId, password);
                if (rtnMessage.Length == 0)
                {
                    password = CryptHelpers.Encrypt(password.Trim(), WebConstants.EncryptionKey);
                    int passwordResetInterval = GetPasswordResetInterval(userId);
                    DateTime expirationDate = DateTime.Now.AddDays(passwordResetInterval);
                    UpdateUserSecurityAttribute(userId, (int)Enums.UserSecurityAttributeType.Password, password, DateTime.Now, expirationDate);
                    UpdateUserSecurityAttribute(userId, (int)Enums.UserSecurityAttributeType.ForcePasswordReset, "FALSE", DateTime.Now, DateTime.Now);

                    rtnMessage = "Your password was successfully reset. Use the new password to login.";

                    rtn = 1;
                }
                else
                {
                    rtn = 0;
                }
            }
            catch (Exception ex)
            {
                if (password.Length < 6)
                    rtnMessage = "A new password between 6 and 10 characters is required...";
                else
                    rtnMessage = "Password should not contain special character ';', '\'', '-'";


                rtn = 0;
            }

            return rtn;

        }
        private int GetPasswordResetInterval(int userId)
        {
            int result = 0;

            using (var db = new DBMEdition01Context())
            {
                try
                {
                    if (db.ApiGetPasswordResetInterval(userId).Count > 0)
                    {


                        try
                        {
                            result = (int)db.ApiGetPasswordResetInterval(userId).FirstOrDefault().Interval;
                        }
                        catch (Exception ex)
                        {

                            string sqlParam = "ApiGetPasswordResetInterval(" + userId + ")";
                            string methodName = "JCRAPI/Business/UserServices/GetPasswordResetInterval";
                            exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);


                        }
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetPasswordResetInterval(" + userId + ")";
                    string methodName = "JCRAPI/Business/UserServices/GetPasswordResetInterval";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);


                }

            }

            return result;

        }
        public static string ValidatePasswordRules(int userId, string newPassword)
        {
            //get existing site settings
            string attributeList = "";
            int siteId;
            string existingPassword;

            List<ApiSelectSiteAttributeMapReturnModel> passwordRestrictions = new List<ApiSelectSiteAttributeMapReturnModel>();

            attributeList = ((int)Enums.CodeCategoryEnum.SitePasswordResetInterval).ToString() + "," +
                            ((int)Enums.CodeCategoryEnum.SitePasswordSpecialRequirements).ToString() + "," +
                            ((int)Enums.CodeCategoryEnum.SitePasswordLength).ToString() + "," +
                            ((int)Enums.CodeCategoryEnum.SitePasswordUpperCaseRequirements).ToString() + "," +
                            ((int)Enums.CodeCategoryEnum.SitePasswordNumericRequirements).ToString();
            ExceptionLogServices exceptionLog = new ExceptionLogServices();
            using (var db = new DBAMPContext())
            {

                try
                {
                    siteId = db.ApiGetUserDefaultSiteId(userId).FirstOrDefault().DefaultSelectedSiteId;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUserDefaultSiteId(" + userId + ")";
                    string methodName = "JCRAPI/Business/UserServices/ValidatePasswordRules";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    siteId = 0;
                }
                try
                {
                    existingPassword = db.ApiGetUserPassword(userId).FirstOrDefault().AttributeValue;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUserPassword(" + userId + ")";
                    string methodName = "JCRAPI/Business/UserServices/ValidatePasswordRules";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    existingPassword = string.Empty;
                }

            }

            using (var db = new DBMEdition01Context())
            {

                try
                {
                    passwordRestrictions = db.ApiSelectSiteAttributeMap(siteId, attributeList);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSelectSiteAttributeMap(" + siteId + "," + attributeList + ")";
                    string methodName = "JCRAPI/Business/UserServices/ValidatePasswordRules";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    existingPassword = string.Empty;
                }

            }


            bool textRestrictions = false;
            string retValue = "";
            bool passwordGood = true;


            foreach (var restriction in passwordRestrictions)

            {
                int rowCode = Convert.ToInt32(restriction.AttributeTypeID.ToString());
                int rowValue = Convert.ToInt32(restriction.AttributeValueID.ToString());
                switch (rowCode)
                {
                    case (int)Enums.CodeCategoryEnum.SitePasswordLength:
                        if (newPassword.Trim().Length < rowValue)
                        {
                            retValue += "###Minimum Password Length is " + rowValue.ToString() + " Characters";
                            passwordGood = false;
                        }
                        break;
                    case (int)Enums.CodeCategoryEnum.SitePasswordResetInterval:

                        string encyptEnteredPwd = "";
                        if (newPassword.Trim().Length > 0)
                        {
                            encyptEnteredPwd = CryptHelpers.Encrypt(newPassword.Trim(), WebConstants.EncryptionKey);
                        }

                        if (encyptEnteredPwd == existingPassword)
                        {
                            retValue += "###Existing password cannot be used";
                            passwordGood = false;
                        }
                        break;
                    case (int)Enums.CodeCategoryEnum.SitePasswordSpecialRequirements:
                        if (rowValue == 1)
                        {
                            char[] anyChars = {
                                                  '!', '#', '$', '%', '&', '(', ')', '*', '+', ',', '.', '/', ':', '<',
                                                  '='
                                                  , '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~',
                                                  '"'
                                              };
                            // special chars
                            int index = newPassword.IndexOfAny(anyChars);
                            if (index < 0)
                            {
                                passwordGood = false;
                                retValue +=
                                    "###At least one Special Character is required: ! # $ % & ( ) * + , . / : < = > ? @ [ \\ ] ^ _ ` { | } ~ \"  Characters below cannot be used  ' - ; ";
                            }
                            textRestrictions = true;
                        }
                        break;
                    case (int)Enums.CodeCategoryEnum.SitePasswordUpperCaseRequirements:
                        if (rowValue == 1)
                        {
                            char[] anyChars = {
                                                  'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                                  'O'
                                                  , 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                                              };
                            // special chars
                            int index = newPassword.IndexOfAny(anyChars);
                            if (index < 0)
                            {
                                passwordGood = false;
                                retValue += "###At least one Upper Case Character is required ";
                            }
                            textRestrictions = true;
                        }
                        break;
                    case (int)Enums.CodeCategoryEnum.SitePasswordNumericRequirements:
                        if (rowValue == 1)
                        {
                            char[] anyChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }; // special chars
                            int index = newPassword.IndexOfAny(anyChars);
                            if (index < 0)
                            {
                                passwordGood = false;
                                retValue += "###At least one Numeric Character is required ";
                            }
                            textRestrictions = true;
                        }
                        break;
                }
            }
            if (textRestrictions)
            {
                char[] anyChars = {
                                      'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                                      'q',
                                      'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                                  }; // special chars
                int index = newPassword.IndexOfAny(anyChars);
                if (index < 0)
                {
                    passwordGood = false;
                    retValue += "###At least one Lower Case Character is required";
                }
            }
            if (passwordGood)
                retValue = "";
            return retValue;
        }
        public void UpdateUserSecurityAttribute(int? userId, int? attributeTypeId, string attributeValue, System.DateTime? attributeActivationDate, System.DateTime? attributeExpirationDate)
        {
            using (var db = new DBAMPContext())
            {
                int result;

                result = db.ApiUserSecurityAttributeUpdate(userId, attributeTypeId, attributeValue, attributeActivationDate, attributeExpirationDate);
            }
        }
        public ApiGetEulaStatusReturnModel CheckEULAStatus(int userId)
        {
            ApiGetEulaStatusReturnModel rtn = new ApiGetEulaStatusReturnModel();
            using (var db = new DBAMPContext())
            {


                try
                {
                    List<ApiGetEulaStatusReturnModel> user;

                    user = db.ApiGetEulaStatus(userId);

                    if (user != null)
                    {
                        ApiGetEulaStatusReturnModel firstOrDefault = user.FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            rtn = firstOrDefault;
                        }
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetEulaStatus(" + userId +")";
                    string methodName = "JCRAPI/Business/UserServices/CheckEULAStatus";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return null;
                }

            }
            return rtn;
        }
        /// <summary>
        /// Fetches user details by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            User user;
            using (var db = new DBAMPContext())
            {
                try
                {
                    user = db.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
                }
                catch (Exception ex)
                {

                    string sqlParam = " db.Users.Where(u => u.UserId ==" + userId +")";
                    string methodName = "JCRAPI/Business/UserServices/GetUserById";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return null;
                }

            }

            return user;
        }


        public ApiGetUserRoleBySiteReturnModel GetUserRoleBySite(int userId, int siteId)
        {
            ApiGetUserRoleBySiteReturnModel user;
            using (var db = new DBMEdition01Context())
            {


                try
                {
                    user = db.ApiGetUserRoleBySite(userId, siteId).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUserRoleBySite(" + userId + "," + siteId + ")";
                    string methodName = "JCRAPI/Business/UserServices/GetUserRoleBySite";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId,siteId, sqlParam, string.Empty);

                    return null;
                }
            }

            return user;
        }
        public ApiAddUserSecurityAttributeReturnModel AddUserSecurityAttribute(int? userId, int? attributeTypeId, string attributeValue, System.DateTime? attributeActivationDate, System.DateTime? attributeExpirationDate)
        {

            ApiAddUserSecurityAttributeReturnModel _result;
            using (var db = new DBAMPContext())
            {

                try
                {
                    _result = db.ApiAddUserSecurityAttribute(userId, attributeTypeId, attributeValue, attributeActivationDate, attributeExpirationDate);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiAddUserSecurityAttribute(" + userId + "," + attributeTypeId + "," + attributeValue + "," + attributeActivationDate + "," + attributeExpirationDate +")";
                    string methodName = "JCRAPI/Business/UserServices/AddUserSecurityAttribute";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return null;
                }

            }
            return _result;
        }
        public int UpdateUserSecurityAnswer(int? userId, int? attributeTypeId, string attributeValue, int? parentCodeId)
        {
            int _result;

            int? codeCategoryId = (int)Enums.CodeCategoryEnum.SecurityQuestions;
            System.DateTime? attributeActivationDate = DateTime.Now;
            int passwordResetInterval = GetPasswordResetInterval((int)userId);
            System.DateTime? attributeExpirationDate = DateTime.Now.AddDays(passwordResetInterval);

            _result = UpdateUserSecurityAttributeUpdateByParent(userId, attributeTypeId, attributeValue, attributeActivationDate, attributeExpirationDate, codeCategoryId, parentCodeId);
            return _result;
        }
        public int UpdateUserSecurityAttributeUpdateByParent(int? userId, int? attributeTypeId, string attributeValue, System.DateTime? attributeActivationDate, System.DateTime? attributeExpirationDate, int? codeCategoryId, int? parentCodeId)
        {

            int _result;
            using (var db = new DBAMPContext())
            {

                try
                {
                    _result = db.ApiUserSecurityAttributeUpdateByParent(userId, attributeTypeId, attributeValue, attributeActivationDate, attributeExpirationDate, codeCategoryId, parentCodeId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiUserSecurityAttributeUpdateByParent(" + userId + "," + attributeTypeId + "," + attributeValue + "," + attributeActivationDate + "," + codeCategoryId + "," + parentCodeId + ")";
                    string methodName = "JCRAPI/Business/UserServices/UpdateUserSecurityAttributeUpdateByParent";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return 0;
                }

            }
            return _result;
        }

        public int SaveSiteProgramPreference(int? userId, int? siteId, int? programId)
        {
            int result = 0;
            using (var db = new DBMEdition01Context())
            {
                try
                {
                    result = db.ApiSaveSiteProgramPreference(userId, siteId, programId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSaveSiteProgramPreference(" + userId + "," + siteId + "," + programId + ")";
                    string methodName = "JCRAPI/Business/UserServices/SaveSiteProgramPreference";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    return 0;
                }

            }
            return result;
        }


        //autocomplete
        public List<ApiGetEmailListBySiteListReturnModel> GetEmailListBySiteList(string search, string siteList)
        {
            List<ApiGetEmailListBySiteListReturnModel> rtn;
            using (var db = new DBAMPContext())
            {


                try
                {
                    rtn = db.ApiGetEmailListBySiteList(search, siteList);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetEmailListBySiteList(" + search + "," + siteList + ")";
                    string methodName = "JCRAPI/Business/UserServices/GetEmailListBySiteList";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }

            }

            return rtn;
        }
        public static void UpdateUserPreference(UserPreference userPref)
        {
            using (var db = new Data.DBMEdition01Context())
            {
                ExceptionLogServices exceptionLog = new ExceptionLogServices();
                try
                {
                    db.ApiUpdateUserPreference(userPref.UserID, userPref.SiteID, userPref.ProgramID, userPref.preferenceType, userPref.PreferenceValue);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiUpdateUserPreference(" + userPref.UserID + "," + userPref.SiteID + "," + userPref.ProgramID + "," + userPref.preferenceType + "," + userPref.PreferenceValue+ ")";
                    string methodName = "JCRAPI/Business/UserServices/UpdateUserPreference";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userPref.UserID, userPref.SiteID, sqlParam, string.Empty);

                }

            }
        }

        public static String GetUserPreference(int userID, int siteID, int programID, string preferenceType)
        {

            String _result = string.Empty;

            using (var db = new Data.DBMEdition01Context())
            {
                ExceptionLogServices exceptionLog = new ExceptionLogServices();
                try
                {
                    var _ret = db.ApiGetUserPreference(userID, siteID, programID, preferenceType);

                    if (_ret.Count > 0)
                    {
                        _result = _ret.FirstOrDefault().PreferenceValue;
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileSaveTracerResponse(" + userID + "," + siteID + "," + programID + "," + preferenceType + ")";
                    string methodName = "JCRAPI/Business/UserServices/MenuStateInit";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, siteID, sqlParam, string.Empty);

                    return null;
                }

            }

            return _result;

        }
        public bool CheckUserLoginFirstAfterProductRelease(int eProductId, int userId)
        {
            bool status = false;
            string ProductReleasedDate = ConfigurationManager.AppSettings["ProductReleasedDate"].ToString();

            using (var db = new DBAMPContext())
            {


                try
                {
                    var rtn = db.ApiCheckUserLoginFirstAfterProductRelease(eProductId, userId, Convert.ToDateTime(ProductReleasedDate));
                    status = rtn.First().IsFirstLogin.Value;
                }
                catch (Exception ex)
                {
                    string sqlParam = "CheckUserProductLoginStatusAfterRelease(" + eProductId + ", " + userId + ")";
                    string methodName = "JCRAPI/Business/UserServices/CheckUserLoginFirstAfterProductRelease";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return false;
                }

            }

            return status;
        }


    }
}