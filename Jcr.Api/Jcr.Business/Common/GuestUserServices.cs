using System;
using System.Collections.Generic;
using System.Linq;
using Jcr.Api.Enumerators;
using Jcr.Api.Models;
using Jcr.Data;
using Jcr.Api.Helpers;
using System.Text;
using System.Data;
using System.Net.Mail;


namespace Jcr.Business
{
    public class GuestUserServices
    {
        ExceptionLogServices exceptionLog = new ExceptionLogServices();
        public GuestUserInfo GetGuessInfoByLink(string email, string gtlink, out string rtnErr)
        {

            int errorID = 0;
            string errorMessage;
            GuestUserInfo userInfo = new GuestUserInfo();

            //step 1 Look up GuessLink, get guestLink object
            rtnErr = string.Empty;
            GuestLink guestLink = LookupByGuestLink(gtlink);
            if (guestLink.GuestLinkGUID == Guid.Empty)
                rtnErr = "No valid guest links available. Please contact your Program Administrator";
            else
            {
                //step 2 Check guest link Error or not

                if (IsErrorLink(guestLink, out errorID, out errorMessage))
                {
                    rtnErr = errorMessage;
                }
                else
                {
                    //step 3 Check Domain
                    MailAddress address = new MailAddress(email);
                    string currentHost = "@" + address.Host;


                    var isInvalidDomain = GetGuestAccessDomains(guestLink.SiteID, currentHost);
                    if (isInvalidDomain)
                    {
                        rtnErr = "You do not have access to this guest tracer. Please contact your Program Administrator.";

                    }
                    userInfo = ValidateUser(email, guestLink.SiteID);
                }
            }

            return userInfo;


        }

        public List<ApiMobileGuestUserSelectSitesReturnModel> GetGuestUserSites(int userId, string email)
        {
            List<ApiMobileGuestUserSelectSitesReturnModel> rtn;

            MailAddress address = new MailAddress(email);

            string currentHost = "@" + address.Host;
            bool isInvalidDomain;
            int numberOfInactiveDepartments;

            using (var db = new DBMEdition01Context())
            {
                try
                {
                    rtn = db.ApiMobileGuestUserSelectSites((int)userId, (int)Enums.EProduct.Tracers);
                    if (rtn.Count > 0)
                    {
                        foreach (var item in rtn.ToList())
                        {
                            isInvalidDomain = GetGuestAccessDomains(item.SiteID, currentHost);
                            if (isInvalidDomain) //Check Domain Restriction
                            {
                                rtn.Remove(item);
                            }

                            numberOfInactiveDepartments = GetDepartmentCount(item.SiteID, item.ProgramID);

                            if (numberOfInactiveDepartments == 0) //Check  Department
                            {
                                rtn.Remove(item);
                            }

                            if (item.RoleID == null)
                                item.RoleID = 7;
                        }
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileSaveTracerResponse(" + userId + ",2" + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/GetGuestUserSites";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                    return null;
                }



            }
            rtn = rtn.GroupBy(x => x.SiteID).Select(g => g.First()).ToList();
            return rtn;
        }
        public List<ApiMobileGuestUserSelectProgramsBySiteReturnModel> GetGuestUserProgramsBySiteId(int userId, int siteId, int productId)
        {
            List<ApiMobileGuestUserSelectProgramsBySiteReturnModel> rtn;
            UserServices userService = new UserServices();
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    rtn = db.ApiMobileGuestUserSelectProgramsBySite(userId, siteId, productId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileGuestUserSelectProgramsBySite(" + userId.ToString() + "," + siteId.ToString() + "," + productId.ToString() + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/GetGuestUserProgramsBySiteId";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                    return null;
                }

                //Insert User's fav site and program if dont' have any
                if (rtn.Count > 0)
                {
                    if (rtn.Select(x => x.IsPrefered = 1).Count() == 0)
                    {

                        userService.SaveSiteProgramPreference(userId, siteId, rtn.FirstOrDefault().ProgramID);
                    }
                }

            }
            return rtn;
        }
        public ApiMobileValidateTracerGuestUserReturnModel ValidateTracerGuestUser(string email, out string rtnErr)
        {
            ApiMobileValidateTracerGuestUserReturnModel rtn = new ApiMobileValidateTracerGuestUserReturnModel();
            List<ApiMobileValidateTracerGuestUserReturnModel> rtnData;
            rtnErr = string.Empty;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    rtnData = db.ApiMobileValidateTracerGuestUser(email);
                    if (rtnData != null && rtnData.Count > 0)
                    {
                        rtn = rtnData.FirstOrDefault();
                    }
                    else
                        rtnErr = "No valid guest links available. Please contact your Program Administrator";

                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileValidateTracerGuestUser(" + email + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/ValidateTracerGuestUser";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }


            }
            return rtn;
        }

        public ApiMobileValidateGuestUserReturnModel ValidateGuestUser(string userLogin, int siteID, out string errMessage)
        {
            ApiMobileValidateGuestUserReturnModel  rtn = new ApiMobileValidateGuestUserReturnModel();
            using (var db = new DBMEdition01Context())
            {
                try
                {
                    errMessage = string.Empty;

                    var rtnList = db.ApiMobileValidateGuestUser(userLogin, siteID);
                    if (rtnList != null && rtnList.Count > 0)
                        rtn = rtnList.FirstOrDefault();


                    if ((bool)rtn.IsNewUser )
                    {
                        if(siteID == 0)
                            errMessage = "No valid guest links available. Please contact your Program Administrator.";
                        else
                        {
                            MailAddress address = new MailAddress(userLogin);
                            string currentHost = "@" + address.Host;

                            var isInvalidDomain = GetGuestAccessDomains(siteID, currentHost);
                            if (isInvalidDomain)
                                errMessage = "You do not have access to this guest tracer. Please contact your Program Administrator.";
                        }
                    }
                    else
                    {

                        if (siteID > 0)
                        {
                            MailAddress address = new MailAddress(userLogin);
                            string currentHost = "@" + address.Host;

                            var isInvalidDomain = GetGuestAccessDomains(siteID, currentHost);
                            if (isInvalidDomain)
                                errMessage = "You do not have access to this guest tracer. Please contact your Program Administrator.";
                        }
                        else
                        {
                            var sites = this.GetGuestUserSites((int)rtn.UserID, userLogin);
                            if (sites.Count > 0)
                                return rtn;
                            else
                            {
                                errMessage = "No valid guest links available. Please contact your Program Administrator.";
                                return rtn;
                            }
                        }
                    }

                }
                catch(Exception ex)
                {
                    string sqlParam = "ApiMobileValidateGuestUser(" + userLogin + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/ValidateTracerGuestUser";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    errMessage = "There was an error validating the user. Please contact your Program Administrator.";
                    return null;
                }
            }
            return rtn;
        }

        public List<ApiGuestUserTracerDetailSelectBySiteProgramReturnModel> GetGuestUserTracersBySiteProgram(int? userId, int? siteId, int? programId, int? statusId)
        {
            string localUserId = userId != null ? userId.ToString() : "null";
            string localSiteId = siteId != null ? siteId.ToString() : "null";
            string localProgramId = programId != null ? programId.ToString() : "null";
            string localStatusId = statusId != null ? statusId.ToString() : "null";

            List<ApiGuestUserTracerDetailSelectBySiteProgramReturnModel> rtn;
            {

                using (var db = new DBMEdition01Context())
                {

                    try
                    {
                        rtn = db.ApiGuestUserTracerDetailSelectBySiteProgram(userId, siteId, programId, statusId);
                    }
                    catch (Exception ex)
                    {

                        string sqlParam = "ApiGuestUserTracerDetailSelectBySiteProgram(" + localUserId + "," + localSiteId + "," + localProgramId + "," + localStatusId + ")";
                        string methodName = "JCRAPI/Business/GuestUserServices/GetGuestUserTracersBySiteProgram";
                        exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);

                        return null;
                    }

                }

                return rtn;
            }
        }
        public string CreatePasscode(string email)
        {
            string rtn;

            var userData = new GuestUserInfo();

            ApiSelectUserByUserLogonIdReturnModel userInfo;
            UserServices userService = new UserServices();

            userInfo = userService.GetUserByLogonId(email);

            Random generator = new Random();
            int passCode = generator.Next(10000, 99999);
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    List<ApiUpdateSelectPasscodeReturnModel> rtnPassCode = db.ApiUpdateSelectPasscode(userInfo.UserID, userInfo.UserID, passCode);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiUpdateSelectPasscode(" + userInfo.UserID + "," + userInfo.UserID + "," + passCode + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/CreatePasscode";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userInfo.UserID, null, sqlParam, string.Empty);

                    return null;
                }
            }
            var actionTypeId = (int)Enums.ActionTypeEnum.GuestAccessPassCode;

            Email setEmail = new Email();
            setEmail.To = email;
            setEmail.Subject = "Guest Passcode";
            setEmail.Body = "Dear <strong>" + userInfo.FirstName + " " + userInfo.LastName + "</strong>,<br/><br/>Here is your new passcode. Please use this code to access your Guest Account.<br/><br/>" +
                "<b>New Passcode: " + passCode + "</b><br/>" +
                "<br/><i>Please note: This e-mail message was sent from a notification-only address that does not accept incoming e-mail. Do not reply to this message.</i>";

            EmailHelpers.SendEmail(setEmail, actionTypeId, 0, userInfo.UserID, string.Empty, userInfo.FirstName + " " + userInfo.LastName);
            rtn = "Email with new passcode successfully sent. Use the new passcode to view in-progress observations";
            return rtn;

        }

        public bool IsUserRegistered(string email)
        {
            bool rtn = false;

            GuestUserInfo guestUserInfo = new GuestUserInfo();
            ApiSelectUserByUserLogonIdReturnModel userInfo;
            UserServices userService = new UserServices();

            userInfo = userService.GetUserByLogonId(email);

            SiteServices siteService = new SiteServices();
            List<ApiSelectTracerSitesByUserReturnModel> sites;
            ApiSelectTracerSitesByUserReturnModel defaultSite;

            var siteID = 0;
            sites = siteService.GetTracerSitesByUser(userInfo.UserID, null, null);
            defaultSite = sites.Where(x => x.DefaultSelectedSite == 1).FirstOrDefault();
            if (defaultSite == null)
                defaultSite = sites.FirstOrDefault();

            if (defaultSite != null)
                siteID = defaultSite.SiteID;

            guestUserInfo = ValidateUser(email, siteID);

            if (guestUserInfo.IsRegisteredUser)
                rtn = true;

            return rtn;
        }
        public bool AuthenticateGuestUser(string email, string passCode)
        {

            bool rtn;
            GuestUserInfo guestUserInfo = new GuestUserInfo();

            var passCodeDB = string.Empty;

            ApiSelectUserByUserLogonIdReturnModel userInfo;
            UserServices userService = new UserServices();

            userInfo = userService.GetUserByLogonId(email);

            //SiteServices siteService = new SiteServices();
            //List<ApiSelectTracerSitesByUserReturnModel> sites;
            //ApiSelectTracerSitesByUserReturnModel defaultSite;

            //sites = siteService.GetTracerSitesByUser(userInfo.UserID, null, null);
            //defaultSite = sites.Where(x => x.DefaultSelectedSite == 1).FirstOrDefault();
            //guestUserInfo = ValidateUser(email, defaultSite.SiteID);

            //----------------------------------------------------------------
            //if (!guestUserInfo.IsRegisteredUser)
            //{

            int defaultPassCode = 0;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    passCodeDB = db.ApiUpdateSelectPasscode(userInfo.UserID, userInfo.UserID, defaultPassCode).FirstOrDefault().Passcode.ToString();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiUpdateSelectPasscode(" + userInfo.UserID + "," + userInfo.UserID + "," + defaultPassCode + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/AuthenticateGuestUser";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userInfo.UserID, null, sqlParam, string.Empty);

                    return false;
                }
            }

            if (!string.Equals(passCode, passCodeDB, StringComparison.InvariantCulture))
                rtn = false;
            else
                rtn = true;

            //}
            //else
            //{
            //    DataSet dsUser = commonService.SelectUserInfoSitesAffiliationAndLicensesForActiveCyclesByLogon(AppSession.EmailAddress, Crypt.Encrypt(passCode.Trim(), WebConstants.ENCRYPT_KEY));

            //    if (dsUser.Tables[0].Rows.Count > 0)
            //        isAuthenticated = true;
            //}

            return rtn;
        }

        public GuestUserInfo CreateTracersGuestUser(string userLogonId, string firstName, string lastName, int? siteId)
        {
            ApiCreateTracersGuestUserReturnModel rtnData;
            GuestUserInfo returnUser;
            int roleId = (int)Enums.RoleType.GuestUser;
            using (var db = new DBAMPContext())
            {

                try
                {
                    rtnData = db.ApiCreateTracersGuestUser(userLogonId, firstName, lastName, siteId, roleId);

                    returnUser = ParseResultforNewGuestUser(rtnData);
                    if (returnUser.UserSiteInfo.SiteID > 0)
                        returnUser.UserSiteInfo.SiteName = GetSiteFullName(returnUser.UserSiteInfo.SiteID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiCreateTracersGuestUser(" + userLogonId + "," + firstName + "," + lastName + "," + siteId + "," + roleId + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/CreateTracersGuestUser";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);

                    return null;
                }

            }

            return returnUser;
        }

        public ApiGuestLinkReadReturnModel isGuestLinkValidated(System.Guid lnk)
        {
            ApiGuestLinkReadReturnModel rtn;
            using (var db = new DBMEdition01Context())
            {


                try
                {
                    rtn = db.ApiGuestLinkRead(null, null, null, lnk, null).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGuestLinkRead(null, null, null," + lnk + ",null)";
                    string methodName = "JCRAPI/Business/GuestUserServices/isGuestLinkValidated";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }

            }

            return rtn;
        }

        public GuestLink LookupByGuestLink(string guidWithoutDashes)
        {
            string sql = string.Empty;
            string cs = string.Empty;

            //var guestLink = new GuestLink();

            ApiGuestLinkReadReturnModel glnk = new ApiGuestLinkReadReturnModel();
            GuestLink guestLink = new GuestLink();
            //string guidWithDashes = string.Format("{0}-{1}-{2}-{3}-{4}", guidWithoutDashes.Substring(0, 8),
            //    guidWithoutDashes.Substring(8, 4),
            //    guidWithoutDashes.Substring(12, 4),
            //    guidWithoutDashes.Substring(16, 4),
            //    guidWithoutDashes.Substring(20, 12));

            // This code is here to validate an edge case where a user tries to access a guestlink using the following URL:
            // http://localhost:55556/GuestAccess/UserPrompt.aspx?lnk=261bd67c03f9453e977564f03332XXXX

            Guid validGuid;
            if (Guid.TryParse(guidWithoutDashes, out validGuid))
            {
                // Nothing to do.
            }
            else
            {
                return guestLink;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                glnk = isGuestLinkValidated(new System.Guid(guidWithoutDashes));

                if (guestLink != null)
                {
                    guestLink.GuestLinkID = glnk.GuestLinkID;
                    guestLink.GuestLinkGUID = glnk.GuestLinkGUID;
                    guestLink.IsEnabled = glnk.IsEnabled;
                    guestLink.SiteID = glnk.SiteID;
                    guestLink.SiteName = glnk.SiteName;
                    guestLink.TracerCustomID = glnk.TracerCustomID;
                    guestLink.TracerCustomName = glnk.TracerCustomName;
                    guestLink.TracerStatusID = (int)glnk.TracerStatusID;
                    guestLink.ProgramID = (int)glnk.ProgramID;
                    guestLink.ProgramName = glnk.ProgramName;
                    guestLink.ExpirationDate = glnk.ExpirationDate;
                    guestLink.CreatedByID = glnk.CreatedByID;
                    guestLink.CreatedDate = glnk.CreatedDate;
                    guestLink.UpdatedByID = glnk.UpdatedByID;
                    guestLink.UpdatedDate = glnk.UpdatedDate;
                    guestLink.CycleID = (int)glnk.CycleID;
                    guestLink.IsCmsLicenseActive = glnk.IsCmsLicenseActive.HasValue ? glnk.IsCmsLicenseActive.Value : false;
                    guestLink.TracerType = glnk.TracerType;

                    if (string.IsNullOrEmpty(guestLink.ExpirationDate) == false)
                    {
                        guestLink.ExpirationDate1 = Convert.ToDateTime(guestLink.ExpirationDate);
                        if (guestLink.ExpirationDate1 < DateTime.Now)
                            guestLink.IsExpired = true;
                    }
                    GetTracerLicenseDetails(guestLink);
                }
                else
                    guestLink.GuestLinkGUID = Guid.Empty;
            }
            catch (Exception ex)
            {

            }
            return guestLink;
        }

        public void GetTracerLicenseDetails(GuestLink guestLink)
        {
            ApiGetLicenseDetailsForGuestLinkReturnModel rtn;
            using (var db = new DBMEdition01Context())
            {
                try
                {
                    rtn = db.ApiGetLicenseDetailsForGuestLink(guestLink.GuestLinkID).FirstOrDefault();
                    guestLink.IsSiteActive = (bool)rtn.IsSiteActive;
                    guestLink.TracerExpirationDate = (DateTime)rtn.ExpirationDate;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetLicenseDetailsForGuestLink(" + guestLink.GuestLinkID + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/GetTracerLicenseDetails";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);


                }
            }

        }

        public bool IsErrorLink(GuestLink guestLink, out int errorID, out string errorMessage)
        {
            errorID = 0;
            errorMessage = string.Empty;

            if (guestLink.IsExpired)
            {
                errorID = 104;
                errorMessage = guestLink.ExpirationDate1.ToString();
                return true;
            }

            if (guestLink.ExpirationDate1.HasValue && guestLink.ExpirationDate1 < DateTime.Now)
            {
                errorID = 101;
                errorMessage = guestLink.SiteName;
                return true;
            }

            if (!guestLink.IsSiteActive)
            {
                errorID = 102;
                errorMessage = guestLink.SiteName;
                return true;
            }

            guestLink.IsSetupForGuestAccess = IsFeatureEnabled(guestLink.SiteID, 1);
            if (!guestLink.IsSetupForGuestAccess)
            {
                errorID = 109;
                return true;
            }

            if (!guestLink.IsEnabled)
            {
                errorID = 105;
                errorMessage = guestLink.UpdatedDate.ToString();
                return true;
            }


            if (guestLink.TracerStatusID != (UInt32)Enums.TracerStatusID.Published)
            {
                errorID = 103;

                switch (guestLink.TracerStatusID)
                {
                    case (int)Enums.TracerStatusID.Deleted:
                        errorMessage = "Deleted ";
                        break;
                    case (int)Enums.TracerStatusID.Closed:
                        errorMessage = "Closed ";
                        break;
                    case (int)Enums.TracerStatusID.Unpublished:
                        errorMessage = "Unpublished ";
                        break;
                }
                return true;
            }

            int numberOfInactiveDepartments = GetDepartmentCount(guestLink.SiteID, guestLink.ProgramID);

            if (numberOfInactiveDepartments == 0)
            {
                errorID = 107;
                errorMessage = guestLink.SiteName;
                return true;
            }

            return false;
        }

        public bool IsFeatureEnabled(int siteID, int featureID)
        {
            bool rtn;
            using (var db = new DBAMPContext())
            {
                try
                {
                    rtn = db.ApiIsFeatureEnabled(siteID, featureID).ResultSet1.FirstOrDefault().IsEnabled;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiIsFeatureEnabled(" + siteID + "," + featureID + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/IsFeatureEnabled";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteID, sqlParam, string.Empty);

                    return false;
                }

            }

            return rtn;
        }
        public int GetDepartmentCount(int? siteId, int? programId)
        {
            int rtn;
            using (var db = new DBMEdition01Context())
            {
                try
                {
                    rtn = (int)db.ApiGetActiveDeptCount(siteId, programId).FirstOrDefault().NumberOfActiveDepartments;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetActiveDeptCount(" + siteId + "," + programId + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/GetDepartmentCount";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);

                    return 0;
                }
            }

            return rtn;
        }

        public bool GetGuestAccessDomains(int siteId, string currentHost)
        {
            bool rtn = true;
            List<String> guestSettings = new List<String>();

            List<ApiSelectGuestAccessDomainBySiteReturnModel> rtnData;

            using (var db = new DBMEdition01Context())
            {
                try
                {
                    rtnData = db.ApiSelectGuestAccessDomainBySite(siteId);

                    if (rtnData.Count > 0)
                    {
                        var domains = rtnData[0].DomainName.ToString();
                        guestSettings.AddRange(domains.Split(';'));

                        bool isDomainEnabled = rtnData[0].DomainRestriction.ToString().Trim() == "0" ? false : true;

                        if (isDomainEnabled)
                        {

                            foreach (var domain in guestSettings)
                            {
                                if (string.Equals(domain.Trim(), currentHost.Trim(), StringComparison.InvariantCultureIgnoreCase))
                                    rtn = false;
                            }
                        }
                        else
                            rtn = false;
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSelectGuestAccessDomainBySite(" + siteId + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/GetGuestAccessDomains";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);

                    return false;
                }

            }
            return rtn;
        }

        public GuestUserInfo ValidateUser(string email, int siteId)
        {
            const string FIRST_NAME = "JCRFirstName";
            const string LAST_NAME = "JCRLastName";
            GuestUserInfo returnUser;

            using (var db = new DBAMPContext())
            {
                ApiVerifyUserByUserLogonIdReturnModel rtnData;

                try
                {

                    rtnData = db.ApiVerifyUserByUserLogonId(email, siteId);

                    returnUser = ParseResultforUser(rtnData);
                    if (returnUser.UserID == 0 ||
                            (returnUser.UserID > 0 //For External users added via Schedule & Assignments screen, consider them as new users to update user & security information
                            && returnUser.FirstName.Equals(FIRST_NAME, StringComparison.InvariantCultureIgnoreCase)
                            && returnUser.LastName.Equals(LAST_NAME, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        returnUser.IsNewUser = true;
                    }
                    returnUser.EmailAddress = email;

                    if (returnUser.UserSiteInfo.SiteID == 0)
                    {
                        SiteServices siteService = new SiteServices();
                        List<ApiMobileGuestUserSelectSitesReturnModel> sites;
                        ApiMobileGuestUserSelectSitesReturnModel defaultSite;
                        using (var dbMedition = new DBMEdition01Context())
                        {
                            sites = dbMedition.ApiMobileGuestUserSelectSites(returnUser.UserID, (int)Enums.EProduct.Tracers);

                        }

                        defaultSite = sites.Where(x => x.DefaultSelectedSite == 1).FirstOrDefault();

                        returnUser.UserSiteInfo.SiteID = defaultSite.SiteID;
                        returnUser.UserSiteInfo.SiteFullName = defaultSite.SiteName;
                        returnUser.UserSiteInfo.RoleID = (int)defaultSite.RoleID;
                        returnUser.UserSiteInfo.SiteName = defaultSite.SiteName;
                        returnUser.UserSiteInfo.IsTracersAccess = false;

                    }
                    else
                        returnUser.UserSiteInfo.SiteName = GetSiteFullName(siteId);

                    if (returnUser.IsActive && IsUserRegistered(returnUser.UserID))
                        returnUser.IsRegisteredUser = true;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiVerifyUserByUserLogonId(" + email + "," + siteId + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/ValidateUser";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);

                    return null;
                }
            }

            return returnUser;
        }

        private GuestUserInfo ParseResultforUser(ApiVerifyUserByUserLogonIdReturnModel userData)
        {
            GuestUserInfo user = new GuestUserInfo();

            //User 
            ApiVerifyUserByUserLogonIdReturnModel.ResultSetModel1 userInfo = userData.ResultSet1.FirstOrDefault();
            ApiVerifyUserByUserLogonIdReturnModel.ResultSetModel2 roleInfo = userData.ResultSet2.FirstOrDefault();
            ApiVerifyUserByUserLogonIdReturnModel.ResultSetModel3 siteInfo = userData.ResultSet3.FirstOrDefault();
            ApiVerifyUserByUserLogonIdReturnModel.ResultSetModel4 eulaInfo = userData.ResultSet4.FirstOrDefault();
            ApiVerifyUserByUserLogonIdReturnModel.ResultSetModel5 gAdminInfo = userData.ResultSet5.FirstOrDefault();

            if (userInfo != null)
            {

                user.UserID = userInfo.UserID;
                user.FirstName = userInfo.FirstName ?? string.Empty;
                user.LastName = userInfo.LastName ?? string.Empty;
                user.MiddleName = userInfo.MiddleName ?? string.Empty;
                user.EmailAddress = userInfo.UserLogonID ?? string.Empty;
                user.IsActive = Convert.ToBoolean(userInfo.UserStatus);
            }
            //userSiteMap
            if (roleInfo != null)
            {

                user.UserSiteInfo.RoleID = roleInfo.RoleID;
                user.UserSiteInfo.IsTracersAccess = (bool)roleInfo.TracersAccess;
                user.IsUserSiteMapPresent = true;
            }
            //Site
            if (siteInfo != null)
            {

                user.UserSiteInfo.SiteID = siteInfo.SiteID;
                user.UserSiteInfo.SiteName = siteInfo.SiteName;
                user.UserSiteInfo.IsSiteActive = siteInfo.SiteStatus;
            }
            //User EULA - Product Status
            if (eulaInfo != null)
            {
                user.IsEulaAccepted = Convert.ToBoolean(eulaInfo.IsEulaAccepted);
            }
            //Is this person a Global Administrator
            if (gAdminInfo != null)
            {
                user.IsGlobalAdmin = Convert.ToBoolean(gAdminInfo.IsGlobalAdmin);
            }
            return user;
        }

        private GuestUserInfo ParseResultforNewGuestUser(ApiCreateTracersGuestUserReturnModel userData)
        {
            GuestUserInfo user = new GuestUserInfo();

            //User 
            ApiCreateTracersGuestUserReturnModel.ResultSetModel1 userInfo = userData.ResultSet1.FirstOrDefault();
            ApiCreateTracersGuestUserReturnModel.ResultSetModel2 roleInfo = userData.ResultSet2.FirstOrDefault();
            ApiCreateTracersGuestUserReturnModel.ResultSetModel3 siteInfo = userData.ResultSet3.FirstOrDefault();
            ApiCreateTracersGuestUserReturnModel.ResultSetModel4 eulaInfo = userData.ResultSet4.FirstOrDefault();
            ApiCreateTracersGuestUserReturnModel.ResultSetModel5 gAdminInfo = userData.ResultSet5.FirstOrDefault();

            if (userInfo != null)
            {

                user.UserID = userInfo.UserID;
                user.FirstName = userInfo.FirstName ?? string.Empty;
                user.LastName = userInfo.LastName ?? string.Empty;
                user.MiddleName = userInfo.MiddleName ?? string.Empty;
                user.EmailAddress = userInfo.UserLogonID ?? string.Empty;
                user.IsActive = Convert.ToBoolean(userInfo.UserStatus);
            }
            //userSiteMap
            if (roleInfo != null)
            {

                user.UserSiteInfo.RoleID = roleInfo.RoleID;
                user.UserSiteInfo.IsTracersAccess = (bool)roleInfo.TracersAccess;
                user.IsUserSiteMapPresent = true;
            }
            //Site
            if (siteInfo != null)
            {

                user.UserSiteInfo.SiteID = siteInfo.SiteID;
                user.UserSiteInfo.SiteName = siteInfo.SiteName;
                user.UserSiteInfo.IsSiteActive = siteInfo.SiteStatus;
            }
            //User EULA - Product Status
            if (eulaInfo != null)
            {
                user.IsEulaAccepted = Convert.ToBoolean(eulaInfo.IsEulaAccepted);
            }
            //Is this person a Global Administrator
            if (gAdminInfo != null)
            {
                user.IsGlobalAdmin = Convert.ToBoolean(gAdminInfo.IsGlobalAdmin);
            }
            return user;
        }

        public string GetSiteFullName(int siteID)
        {
            string returnValue = string.Empty;
            using (var db = new DBAMPContext())
            {

                try
                {
                    returnValue = db.ApiGetSiteFullName(siteID).ResultSet1.SingleOrDefault().SiteFullName;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetSiteFullName(" + siteID + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/GetSiteFullName";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteID, sqlParam, string.Empty);

                    return null;
                }
            }
            return returnValue;
        }

        public bool IsUserRegistered(int userID)
        {
            var isUserRegistered = false;
            using (var db = new DBAMPContext())
            {

                try
                {
                    List<ApiSiteSelectByUserIdReturnModel> rtnData;
                    rtnData = db.ApiSiteSelectByUserId(userID);
                    if (rtnData.Count > 0)
                    {
                        List<ApiSiteSelectByUserIdReturnModel> rtnfiltered = rtnData.Where(x => x.RoleID != 7).ToList();
                        if (rtnfiltered.Count > 0)
                            isUserRegistered = true;
                    }
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiSiteSelectByUserId(" + userID +")";
                    string methodName = "JCRAPI/Business/GuestUserServices/IsUserRegistered";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, null, sqlParam, string.Empty);

                    return false;
                }
            }
            return isUserRegistered;
        }

        public ApiGetTaskLinkDetailsReturnModel LookupByTaskLink(string guidWithoutDashes)
        {
            ApiGetTaskLinkDetailsReturnModel rtn;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    rtn = db.ApiGetTaskLinkDetails(new Guid(guidWithoutDashes)).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetTaskLinkDetails(" + new Guid(guidWithoutDashes) + ")";
                    string methodName = "JCRAPI/Business/GuestUserServices/LookupByTaskLink";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                    return null;
                }

            }

            return rtn;
        }
    }
}