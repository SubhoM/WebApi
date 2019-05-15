using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jcr.Data;
using Jcr.Api.Enumerators;
using Jcr.Tracers.Model;
using static Jcr.Api.Enumerators.Enums;
using System.Data.SqlClient;
using Jcr.Api.Models.Models;

namespace Jcr.Business
{
    public class TracerService
    {
        ExceptionLogServices exceptionLog = new ExceptionLogServices();
        public List<ApiGetTracerCategoryNamesReturnModel> GetTracerCategoryNames(int? siteId, int? programId)
        {
            List<ApiGetTracerCategoryNamesReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiGetTracerCategoryNames(siteId, programId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetTracerCategoryNames(" + siteId + "," + programId + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetTracerCategoryNames";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);

                    return null;
                }

            }
            return _result;
        }
        public List<ApiGetDepartmentHierarchyByRankIdReturnModel> GetDepartmentHierarchy(int? siteId, int? programId, int? rankId, bool? isCategoryActive, bool? isCategoryItemActive)
        {
            List<ApiGetDepartmentHierarchyByRankIdReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiGetDepartmentHierarchyByRankId(siteId, programId, rankId, isCategoryActive, isCategoryItemActive);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetDepartmentHierarchyByRankId(" + siteId + "," + programId + "," + rankId + "," + isCategoryActive + "," + isCategoryItemActive + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetDepartmentHierarchy";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }
        public List<ApiTracerDetailSelectBySiteProgramStatusReturnModel> GetTracerDetail(int? siteId, int? programId, int? statusId)
        {
            List<ApiTracerDetailSelectBySiteProgramStatusReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiTracerDetailSelectBySiteProgramStatus(siteId, programId, statusId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiTracerDetailSelectBySiteProgramStatus(" + siteId + "," + programId + "," + statusId + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetTracerDetail";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }

        public List<ApiResponsesSelectByTracerIdReturnModel> GetTracerResponses(int? tracerId, string responseStatusCSV)
        {
            List<ApiResponsesSelectByTracerIdReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiResponsesSelectByTracerId(tracerId, responseStatusCSV);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiResponsesSelectByTracerId(" + tracerId + "," + responseStatusCSV + ")";
                    string methodName = "JCRAPI/Business/TracerService/MenuStateInit";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }



        public ApiGetTracerHeaderReturnModel GetTracerHeader(int tracerCustomId, int tracerResponseId, int siteId, int programId)
        {
            ApiGetTracerHeaderReturnModel _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiGetTracerHeader(tracerCustomId, tracerResponseId, siteId, programId).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetTracerHeader(" + tracerCustomId + "," + tracerResponseId + "," + siteId + "," + programId + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetTracerHeader";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }
        public List<ApiGetQuestionAnswerImagesReturnModel.ResultSetModel1> GetQuestionAnswerImages(int? tracerQuestionId, int? tracerResponseId, int? userId)
        {

            List<ApiGetQuestionAnswerImagesReturnModel.ResultSetModel1> _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiGetQuestionAnswerImages(tracerQuestionId, tracerResponseId, userId).ResultSet1;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetQuestionAnswerImages(" + tracerQuestionId + "," + tracerResponseId + "," + userId + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetQuestionAnswerImages";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);
                    return null;
                }
            }
            return _result;
        }
        public int DeleteObservation(int tracerCustomID, int userID)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiLogicallyDeleteObservation(tracerCustomID, userID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiLogicallyDeleteObservation(" + tracerCustomID + "," + userID + ")";
                    string methodName = "JCRAPI/Business/TracerService/DeleteObservation";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, null, sqlParam, string.Empty);
                    return 0;
                }
            }
            return _result;

        }
        public int UpdateTracerStatus(int tracerResponseID, int responseStatusID)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiMobileUpdateTracerResponseStatus(tracerResponseID, responseStatusID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiMobileUpdateTracerResponseStatus(" + tracerResponseID + "," + responseStatusID + ")";
                    string methodName = "JCRAPI/Business/TracerService/UpdateTracerStatus";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return 0;
                }
            }
            return _result;

        }
        public int PublishTracer(int? tracerId, int? updatedById)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiPublishTracer(tracerId, updatedById);

                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiPublishTracer(" + tracerId + "," + updatedById + ")";
                    string methodName = "JCRAPI/Business/TracerService/PublishTracer";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return 0;
                }
            }
            return _result;
        }
        public int UnpublishTracer(int? tracerId, int? updatedById)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {
                _result = db.ApiUnpublishTracer(tracerId, updatedById);
            }
            return _result;
        }

        public int TracerTempImage(int siteID, int programID, int tracerCustomID, int tracerQuestionID, string imageName, int userID)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiInsertTracerTempImage(siteID, programID, tracerCustomID, tracerQuestionID, imageName, userID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiInsertTracerTempImage(" + siteID + "," + programID + "," + tracerCustomID + "," + tracerQuestionID + "," + imageName + "," + userID + ")";
                    string methodName = "JCRAPI/Business/TracerService/TracerTempImage";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, siteID, sqlParam, string.Empty);
                    return 0;
                }
            }
            return _result;

        }

        public int TracerImage(int tracerQuestionID, int tracerResponseID, string tempImageID, int UserID)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiInsertQuestionImageMap(tracerQuestionID, tracerResponseID, tempImageID, UserID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiInsertQuestionImageMap(" + tracerQuestionID + "," + tracerResponseID + "," + tempImageID + "," + UserID + ")";
                    string methodName = "JCRAPI/Business/TracerService/TracerImage";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, UserID, null, sqlParam, string.Empty);
                    return 0;
                }

            }
            return _result;

        }

        public List<ApiGetTracerImagesTempReturnModel> GetTempImagestoClean(int tracerCustomID, int userID)
        {
            List<ApiGetTracerImagesTempReturnModel> _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiGetTracerImagesTemp(tracerCustomID, userID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetTracerImagesTemp(" + tracerCustomID + "," + userID + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetTempImagestoClean";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, null, sqlParam, string.Empty);
                    return null;
                }

            }
            return _result;
        }

        public int DeleteImagesTemp(int tracerCustomID, int userID)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiDeleteTracerImagesTemp(tracerCustomID, userID);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiDeleteTracerImagesTemp(" + tracerCustomID + "," + userID + ")";
                    string methodName = "JCRAPI/Business/TracerService/DeleteImagesTemp";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, null, sqlParam, string.Empty);
                    return 0;
                }
            }
            return _result;

        }
        public int DeleteImageByImageName(int tracerCustomID, int tracerQuestionId, int tracerResponseId, int userID, string imageName)
        {
            int _result;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiDeleteTracerImageByName(tracerCustomID, tracerQuestionId, tracerResponseId, userID, imageName);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiDeleteTracerImageByName(" + tracerCustomID + "," + tracerQuestionId + "," + tracerResponseId + "," + userID + "," + imageName + ")";
                    string methodName = "JCRAPI/Business/TracerService/DeleteImageByImageName";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, null, sqlParam, string.Empty);
                    return 0;
                }

            }
            return _result;

        }
        public int GetImagesCount(int tracerResponseId)
        {
            int _result = 0;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiGetTracerImagesCount(tracerResponseId);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetTracerImagesCount(" + tracerResponseId + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetImagesCount";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return 0;
                }

            }
            return _result;

        }

        public void InsertGuestLinkAccessed(int userId, Guid guestLinkGuid)
        {

            int _result = 0;
            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _result = db.ApiInsertGuestLinkAccessed(userId, guestLinkGuid);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiInsertGuestLinkAccessed(" + userId + "," + guestLinkGuid + ")";
                    string methodName = "JCRAPI/Business/TracerService/InsertGuestLinkAccessed";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, null, sqlParam, string.Empty);

                }

            }
        }

        public List<AssignedUser> GetUsersForBrowsing(string searchString, string siteList)
        {
            List<ApiGetUsersForAssignmentReturnModel> _dbResult;

            searchString = searchString == null ? "" : searchString;
           

            using (var db = new DBMEdition01Context())
            {

                try
                {
                    _dbResult = db.ApiGetUsersForAssignment(searchString, siteList);
                    var _usrList = _dbResult.Select(m => new AssignedUser
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        EmailAddress = m.EmailAddress,
                        IsExternal = m.IsExternal,
                        UserId = m.UserID,
                        UserRole = m.UserRole
                    }
                    ).OrderBy(x => x.FormattedFullName).ThenBy(x => x.UserRole).ToList();
                    return _usrList;

                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUsersForAssignment(" + searchString + "," + siteList + ")";
                    string methodName = "JCRAPI/Business/TracerService/GetUsersForBrowsing";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);
                    return null;
                }
            }
           

        }

        public List<AssignedUser> GetUsersDetails(string lstUserIDs, int siteID)
        {
            List<ApiGetUsersDetailsReturnModel> _dbResult;



            using (var db = new DBMEdition01Context())
            {
               
                try
                {
                    _dbResult = db.ApiGetUsersDetails(lstUserIDs, siteID);
                    var _usrList = _dbResult.Select(m => new AssignedUser
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        EmailAddress = m.EmailAddress,
                        IsExternal = m.IsExternal,
                        UserId = m.UserID
                    }
                    ).OrderBy(x => x.FormattedFullName).ThenBy(x => x.UserRole).ToList();

                    return _usrList;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiGetUsersDetails(" + lstUserIDs + "," + siteID +")";
                    string methodName = "JCRAPI/Business/TracerService/GetUsersDetails";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteID, sqlParam, string.Empty);
                    return null;
                }


            }


           

        }

        public List<AssignedUser> CreateGuestUserByEmailIds(string lstUserIDs, int siteID, int updateByID)
        {
            List<ApiCreateGuestUserByEmailIdsReturnModel> _dbResult;



            using (var db = new DBMEdition01Context())
            {

                
                try
                {
					lstUserIDs = lstUserIDs.Replace(",", ";");
					lstUserIDs = lstUserIDs.Replace(" ", string.Empty);

                    _dbResult = db.ApiCreateGuestUserByEmailIds(lstUserIDs, siteID, WebConstants.FIRST_NAME, WebConstants.LAST_NAME, "", (int)ExternalUserRoleType.TracerAssignmentGuest, updateByID);
                    var _usrList = _dbResult.Select(m => new AssignedUser
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        EmailAddress = m.EmailAddress,
                        IsExternal = m.IsExternal,
                        UserId = m.UserID
                    }
                    ).ToList();


                    return _usrList;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiCreateGuestUserByEmailIds(" + lstUserIDs + "," + siteID + "," + WebConstants.FIRST_NAME + "," + WebConstants.LAST_NAME + ",," + (int)ExternalUserRoleType.TracerAssignmentGuest + "," + updateByID + ")";
                    string methodName = "JCRAPI/Business/TracerService/CreateGuestUserByEmailIds";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteID, sqlParam, string.Empty);
                    return null;
                }

            }
                   

        }

        public string GetInactiveUserEmailIds(string lstEmailIDs)
        {
            List<ApiValidateInactiveEMailIdsReturnModel> _dbResult;



            using (var db = new DBAMPContext())
            {


                try
                {
                    lstEmailIDs = lstEmailIDs.Replace(" ", string.Empty);

                    _dbResult = db.ApiValidateInactiveEMailIds(lstEmailIDs);

                    string _usrList = _dbResult.Count>0 ? _dbResult.FirstOrDefault().Inactive_EmailIds : "";
                    
                    return _usrList;
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiValidateInactiveEmailIds(" + lstEmailIDs + ")";
                    string methodName = "JCRAPI/Business/TracerService/CreateGuestUserByEmailIds";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, 0, sqlParam, string.Empty);
                    return null;
                }

            }


        }

        public static ApiGetTracerQuestionInfoReturnModel GetTracerQuestionInfo(int? tracerCustomID, int? tracerQuestionId)
        {
            ApiGetTracerQuestionInfoReturnModel _result;
            using (var db = new Data.DBMEdition01Context())
            {
                _result = db.ApiGetTracerQuestionInfo(tracerCustomID, tracerQuestionId).FirstOrDefault();
            }

            return _result;
        }

        public static List<ApiGetStdDetailsByTracerQuestionReturnModel> GetStdDetailsByTracerQuestion(int? tracerCustomId, int? tracerResponseId, int? tracerQuestionId, bool? isGuestAccess)
        {
            List<ApiGetStdDetailsByTracerQuestionReturnModel> _result;
            using (var db = new Data.DBMEdition01Context())
            {
                _result = db.ApiGetStdDetailsByTracerQuestion(tracerCustomId, tracerResponseId, tracerQuestionId, isGuestAccess);
            }

            return _result;
        }

        public static string GetAllsStdsByTracerQuestion(int? tracerCustomId, int? tracerResponseId, int? tracerQuestionId, bool? isGuestAccess)
        {
            List<ApiGetStdDetailsByTracerQuestionReturnModel> _dbResult;

            string _result= string.Empty;

            using (var db = new Data.DBMEdition01Context())
            {
                _dbResult = db.ApiGetStdDetailsByTracerQuestion(tracerCustomId, tracerResponseId, tracerQuestionId, isGuestAccess);
            }

            foreach(var item in _dbResult)
            {
                _result = _result.Length > 0 ? _result + ", " + item.StandardLabel : item.StandardLabel;
            }
            
            return _result;
        }

        public List<ApiTracersBySiteProgramStatusReturnModel> GetTracersBySiteProgramStatus(int? programID, int? siteID, int? statusID, int? userID) {
            List<ApiTracersBySiteProgramStatusReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiTracersBySiteProgramStatus(siteID, programID, statusID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                // Fix for Bug 55857. Previous code had programID and siteID variables in reverse order.
                sb.AppendFormat("EXEC dbo.apiTracersBySiteProgramStatus @ProgramID='{0}', @SiteID={1}, @StatusID={2}, @UserID={3}", programID, siteID, statusID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/GetTracersBySiteProgramStatus";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiGetTracersThatCanBeCopiedReturnModel> GetTracersThatCanBeCopied(int siteID, int programID, int userID) {
            List<ApiGetTracersThatCanBeCopiedReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiGetTracersThatCanBeCopied(siteID, programID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiMultisiteCopy @SiteID='{0}', @ProgramID={1}, @UserID={2}", siteID, programID, userID);
                string sqlParam = sb.ToString();

                string methodName = "JCRAPI/Business/TracerService/GetTracersThatCanBeCopied";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiGetTracersThatCanBeDeletedReturnModel> GetTracersThatCanBeDeleted(int siteID, int programID, int userID) {
            List<ApiGetTracersThatCanBeDeletedReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiGetTracersThatCanBeDeleted(siteID, programID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiGetTracersThatCanBeDeleted @SiteID='{0}', @ProgramID={1}, @UserID={2}", siteID, programID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/GetTracersThatCanBeDeleted";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiGetTracerQuestionsReturnModel> GetTracerQuestions(int tracerID) {
            List<ApiGetTracerQuestionsReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiGetTracerQuestions(tracerID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiGetTracerQuestions @TracerID={0}", tracerID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/GetTracerQuestions";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiGetTargetSitesForCopyReturnModel> GetTargetSitesForCopy(int tracerID, int userID) {
            List<ApiGetTargetSitesForCopyReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiGetTargetSitesForCopy(tracerID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiGetTargetSitesForCopy @TracerCustomID={0}, @UserID={1}", tracerID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/GetTargetSitesForCopy";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiGetTargetSitesForDeleteReturnModel> GetTargetSitesForDelete(int tracerID, int userID) {
            List<ApiGetTargetSitesForDeleteReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiGetTargetSitesForDelete(tracerID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiGetTargetSitesForDelete @TracerCustomID='{0}', @UserID={1}", tracerID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/GetTracersThatCanBeDeleted";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiSitesNoLongerAuthorizedToAccessReturnModel> SitesNoLongerAuthorizedToAccess(int tracerID, int userID) {
            List<ApiSitesNoLongerAuthorizedToAccessReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiSitesNoLongerAuthorizedToAccess(tracerID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiSitesNoLongerAuthorizedToAccess @TracerID='{0}', @UserID={1}", tracerID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/SitesNoLongerAuthorizedToAccess";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public int OptOutOfSystemTracers(int tracerID, int userID) {
            int _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiOptOutOfSystemTracers(tracerID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiOptOutOfSystemTracers @TracerID='{0}', @UserID={1}", tracerID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/OptOutOfSystemTracers";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return 0;
            }
        }

        public List<ApiMultisiteCopyReturnModel> MultisiteCopy(string selectedSites, int tracerID, bool isLocked, int userID) {
            List<ApiMultisiteCopyReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    db.Database.CommandTimeout = 360;
                    _result = db.ApiMultisiteCopy(selectedSites, tracerID, isLocked, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                int trueFalse = isLocked ? 1 : 0;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiMultisiteCopy @SelectedSites='{0}', @TracerID={1}, @UserID={2}, @IsLocked={3}",
                    selectedSites, tracerID, userID, trueFalse);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/MultisiteCopy";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiMultisiteDeleteReturnModel> MultisiteDelete(string selectedSites, int tracerID, int userID, out bool isDeadlock) {
            List<ApiMultisiteDeleteReturnModel> _result;
            isDeadlock = false;

            try {
                using (var db = new Data.DBMEdition01Context())
                {
                    db.Database.CommandTimeout = 360;
                    _result = db.ApiMultisiteDelete(selectedSites, tracerID, userID);
                }
                return _result;
            }
            catch (SqlException ex) {
                if (ex.Number == 1205) {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("EXEC dbo.apiMultisiteDelete Sites='{0}', @TracerID={1}, @UserID={2}", selectedSites, tracerID, userID);
                    string sqlParam = sb.ToString();

                    string methodName = "JCRAPI/Business/TracerService/MultisiteDelete";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, tracerID, sqlParam, string.Empty);

                    isDeadlock = true;
                }
                return null;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiMultisiteDelete Sites='{0}', @TracerID={1}, @UserID={2}", selectedSites, tracerID, userID);
                string sqlParam = sb.ToString();

                string methodName = "JCRAPI/Business/TracerService/MultisiteDelete";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userID, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiUnlockedMasterValidationReturnModel> UnlockedMasterValidation(int tracerID, int userID) {
            List<ApiUnlockedMasterValidationReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiUnlockedMasterValidation(tracerID, userID);
                }
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiUnlockedMasterValidation @TracerID={0}, @UserID={1}", tracerID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/UnlockedMasterValidation";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public List<ApiChangeMasterSiteReturnModel> ChangeMasterSite(int tracerID, int siteID, int userID) {

            List<ApiChangeMasterSiteReturnModel> _result;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiChangeMasterSite(tracerID, siteID, userID);
                }
                MultisiteAuditLogAction(tracerID, siteID, userID, _result[0].AuditMsg);
                return _result;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiChangeMasterSite @TracerID='{0}', @SiteID='{1}', @UserID={2}", tracerID, siteID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/ChangeMasterSite";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

        public void MultisiteAuditLogAction(int tracerID, int siteID, int userID, string msg) {
            int _result = 0;
            try {
                using (var db = new Data.DBMEdition01Context()) {
                    _result = db.ApiMultisiteAuditInsert(tracerID, siteID, userID, msg);
                }
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.ApiMultisiteAuditInsert @TracerCustomID='{0}', @SiteID='{1}', @UserID={2}, @Msg='{3}'", tracerID, siteID, userID, msg);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/MultisiteAuditLogAction";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
            }
        }

        public Jcr.Api.Models.Models.SystemTracerInfo GetSystemTracerInfo(int tracerID, int userID) {

            List<ApiGetSystemTracerInfoReturnModel> results;

            Api.Models.Models.SystemTracerInfo MultisiteInfo = new Api.Models.Models.SystemTracerInfo();
            Api.Models.Models.SystemTracer MultisiteTracer;

            try {
                using (var db = new Data.DBMEdition01Context()) {
                    results = db.ApiGetSystemTracerInfo(tracerID, userID);

                    #region Populate MultisiteInfo.Master and MultisiteInfo.SelectedTracer

                    foreach (var item in results) {
                        if (item.IsMaster == true) {
                            MultisiteInfo.Master.IsLocked = item.IsLocked.GetValueOrDefault();
                            MultisiteInfo.Master.IsMaster = true;
                            MultisiteInfo.Master.TracerCustomID = item.TracerCustomID;
                            MultisiteInfo.Master.TracerCustomName = item.TracerNameChanged;
                            MultisiteInfo.Master.SiteID = item.SiteID.GetValueOrDefault();
                            MultisiteInfo.Master.HCOID = item.HCOID;
                            MultisiteInfo.Master.SiteName = item.SiteName;
                            MultisiteInfo.Master.HasAccess = item.HasAccess;
                            MultisiteInfo.Master.TracerStatus = item.TracerStatus;
                            MultisiteInfo.Master.NumOfObservations = item.NumOfObservations.GetValueOrDefault();
                        } else {
                            MultisiteTracer = new Api.Models.Models.SystemTracer();

                            MultisiteTracer.IsLocked = item.IsLocked.GetValueOrDefault();
                            MultisiteTracer.IsMaster = true;
                            MultisiteTracer.TracerCustomID = item.TracerCustomID;
                            MultisiteTracer.TracerCustomName = item.TracerNameChanged;
                            MultisiteTracer.SiteID = item.SiteID.GetValueOrDefault();
                            MultisiteTracer.HCOID = item.HCOID;
                            MultisiteTracer.SiteName = item.SiteName;
                            MultisiteTracer.HasAccess = item.HasAccess;
                            MultisiteTracer.TracerStatus = item.TracerStatus;
                            MultisiteTracer.NumOfObservations = item.NumOfObservations.GetValueOrDefault();

                            MultisiteInfo.Tracers.Add(MultisiteTracer);
                        }
                    }
                    #endregion
                }

                if (MultisiteInfo.Master.IsLocked) {
                    MultisiteInfo.DialogTitle = "Locked Tracer Details";
                } else {
                    MultisiteInfo.DialogTitle = "Unlocked Tracer Details";
                }

                return MultisiteInfo;
            }

            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC dbo.apiGetSystemTracerInfo @TracerID={0}, @UserID={1}", tracerID, userID);
                string sqlParam = sb.ToString();
                string methodName = "JCRAPI/Business/TracerService/GetSystemTracerInfo";
                exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerID, sqlParam, string.Empty);
                return null;
            }
        }

		public bool UpdateTracerCategoryForTracer(int tracerCategoryId, int tracerCustomId, int updatedById)
		{
			try
			{
				using (var db = new Data.DBMEdition01Context())
				{
					db.ApiUpdateTracerCategoryForTracer(tracerCategoryId, tracerCustomId, updatedById);
				}
				return true;
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("EXEC dbo.apiUpdateTracerCategoryForTracer @tracerCategoryId={0}, @tracerCustomId={1}, @updatedById={2}", tracerCategoryId, tracerCustomId, updatedById);
				string sqlParam = sb.ToString();
				string methodName = "JCRAPI/Business/TracerService/UpdateTracerCategoryForTracer";
				exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, tracerCustomId, sqlParam, string.Empty);
				return false;
			}
		}

	}
}
