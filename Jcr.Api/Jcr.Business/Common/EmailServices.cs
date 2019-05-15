using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using Jcr.Api.Models;
using Jcr.Api.Enumerators;
using Jcr.Api.Helpers;
using Jcr.Data;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Web.Mvc;

//using Microsoft.Reporting.WebForms;

namespace Jcr.Business
{
    public class EmailServices
    {
        ExceptionLogServices exceptionLog = new ExceptionLogServices();
        public string SavePDF(string Title, byte[] fileContent, string pformatType = "pdf")
        {
            string Attachment = "";
            try
            {

                var fileLocation = AppDomain.CurrentDomain.BaseDirectory + "ReportExportFiles";
                string dtNow = DateTime.Now.ToString("MM-dd-yyyy_hhmmssfff_tt");
                string filename = string.Format("{0}_{1}.{2}", Title, dtNow.ToString(), pformatType);
                Attachment = Path.Combine(fileLocation, filename);
                using (var fs = new FileStream(Attachment, FileMode.Create))
                {
                   
                    try
                    {
                        fs.Write(fileContent, 0, fileContent.Length);
                    }
                    catch (Exception ex)
                    {

                        string sqlParam = "Write(" + fileContent + ",0," +  fileContent.Length + ")";
                        string methodName = "JCRAPI/Business/EmailServices/SavePDF";
                        exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, null, sqlParam, string.Empty);

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Attachment;
        }

        //public byte[] ObservationPrintForm(int pTracerResponseID, ref int customSections, int pIsErrorOnly = 0, int pIsGuestUser = 0, string pformatType = "pdf")
        public byte[] ObservationPrintForm(int pTracerCustomID, ref int? customSections, int? userId, int? siteId, int? programId, string siteName, string programName, int pIsErrorOnly = 0, int pIsGuestUser = 0, string pformatType = "pdf")
        {
            byte[] fileContents = null;
            string rdlcName = String.Empty;
            string dsName = String.Empty;
            string tracerName = String.Empty;
            string tracerTypeID;

            ReportParameterCollection reportParameterCollection = null;


            try
            {
                // Setup ReportViewer 
                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;

                string reportTitle = "Tracer Observation Detail Report";
                int IsErrorOnly = pIsErrorOnly;

                // Get header and detail data sets
                DataView dvReportHeader = null;
                DataView dvReportDetail = null;


                // Get header data from caller
                List<ApiReportTracerObservationHeaderReturnModel> _result;
                List<ApiReportTracerObservationDetailReturnModel.ResultSetModel1> _detail;
                List<ApiReportTracerObservationDetailReturnModel.ResultSetModel2> _ImageDetail;
                using (var db = new DBMEdition01Context())
                {

                    
                    try
                    {
                        _result = db.ApiReportTracerObservationHeader(pTracerCustomID, siteId, programId);
                    }
                    catch (Exception ex)
                    {

                        string sqlParam = "ApiReportTracerObservationHeader(" + pTracerCustomID + "," + siteId + "," + programId + ")";
                        string methodName = "JCRAPI/Business/EmailServices/ObservationPrintForm";
                        exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, null, siteId, sqlParam, string.Empty);
                        return null;
                    }

                    DataTable ListAsDataTable = BuildDataTable<ApiReportTracerObservationHeaderReturnModel>(_result);
                    dvReportHeader = ListAsDataTable.DefaultView;

                    tracerName = dvReportHeader[0]["TracerCustomName"].ToString();

                    if (tracerName.ToUpper() == "NULL")
                        tracerName = string.Empty;

                    if (dvReportHeader[0]["SurveyTeam"].ToString().ToUpper() == "NULL") dvReportHeader[0]["SurveyTeam"] = string.Empty;

                    if (dvReportHeader[0]["MedicalStaffInvolved"].ToString().ToUpper() == "NULL") dvReportHeader[0]["MedicalStaffInvolved"] = string.Empty;
                    if (dvReportHeader[0]["StaffInterviewed"].ToString().ToUpper() == "NULL") dvReportHeader[0]["StaffInterviewed"] = string.Empty;
                    if (dvReportHeader[0]["Location"].ToString().ToUpper() == "NULL") dvReportHeader[0]["Location"] = string.Empty;
                    if (dvReportHeader[0]["MedicalRecordNumber"].ToString().ToUpper() == "NULL") dvReportHeader[0]["MedicalRecordNumber"] = string.Empty;
                    if (dvReportHeader[0]["EquipmentObserved"].ToString().ToUpper() == "NULL") dvReportHeader[0]["EquipmentObserved"] = string.Empty;
                    if (dvReportHeader[0]["ContractedService"].ToString().ToUpper() == "NULL") dvReportHeader[0]["ContractedService"] = string.Empty;

                    tracerTypeID = dvReportHeader[0]["TracerTypeID"].ToString();

                    _detail = db.ApiReportTracerObservationDetail(pTracerCustomID, programId, null, pIsErrorOnly, out customSections).ResultSet1;

                    DataTable dtReportDetail = BuildDataTable<ApiReportTracerObservationDetailReturnModel.ResultSetModel1>(_detail);
                    dvReportDetail = dtReportDetail.DefaultView; 
                     
                    _ImageDetail = db.ApiReportTracerObservationDetail(pTracerCustomID, programId, null, pIsErrorOnly, out customSections).ResultSet2;
                    DataTable dtReportImageDetail = new DataTable();
                    if (_ImageDetail.Count > 0)
                        dtReportImageDetail = BuildDataTable<ApiReportTracerObservationDetailReturnModel.ResultSetModel2>(_ImageDetail);
                    
                    //-----------------------------------------------------------------
                    // Next get detail data
                    DataSet ds = new DataSet();
                    var qImages = new List<QuestionImage>();

                    dtReportDetail.Columns.Add(new DataColumn("imagepath1", typeof(string)));
                    dtReportDetail.Columns.Add(new DataColumn("imagepath2", typeof(string)));
                    dtReportDetail.Columns.Add(new DataColumn("imagepath3", typeof(string)));
                    dtReportDetail.Columns.Add(new DataColumn("imagepath4", typeof(string)));
                    dtReportDetail.Columns.Add(new DataColumn("imagepath5", typeof(string)));
                    dvReportDetail = new DataView(dtReportDetail);

                    if (dtReportImageDetail.Rows.Count > 0)
                    {
                        qImages = (from DataRow dr in dtReportImageDetail.Rows
                                   select new QuestionImage()
                                   {
                                       TracerQuestionID = Convert.ToInt32(dr["TracerQuestionID"]),
                                       SiteID = Convert.ToInt32(dr["SiteID"]),
                                       ProgramID = Convert.ToInt32(dr["ProgramID"]),
                                       TracerCustomID = Convert.ToInt32(dr["TracerCustomID"]),
                                       FileName = dr["FileName"].ToString()
                                   }).ToList();
                        //  qImages = dtReportImageDetail.Rows.Cast<QuestionImage>().ToList();
                        dtReportImageDetail.Columns.Add(new DataColumn("AzureFilePath", typeof(string)));

                        foreach (DataRowView rowView in dvReportDetail)
                        {
                            string imageDirectoryPath = System.Web.HttpContext.Current.Request.MapPath("~/Images");
                            DataRow row = rowView.Row;
                            var AssociateImages = qImages.Where(image => image.TracerQuestionID == Convert.ToInt32(row["TracerQuestionAnswerID"])).ToList();
                            String originalPath = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority;

                            int counter = 1;
                            foreach (var image in AssociateImages)
                            {
                                if (counter <= 5)
                                {                      
                                    row["imagepath" + counter] = originalPath + string.Format("/Transfer/GetImage?userID={0}&siteID={1}&programID={2}&tracerID={3}&fileName={4}", userId, image.SiteID, image.ProgramID, image.TracerCustomID, image.FileName);                                 
                                }
                                counter++;
                            }
                        }
                    }


                    int IsGuestUser = pIsGuestUser;
                    int NumValue = 0, DenValue = 0, NACount = 0, TotalCount = 0;
                    foreach (DataRowView rowView in dvReportDetail)
                    {
                        DataRow row = rowView.Row;
                        TotalCount = TotalCount + 1;
                        NumValue = NumValue + Convert.ToInt32(row["NumValid"]);
                        DenValue = DenValue + Convert.ToInt32(row["DenValid"]);
                        if (row["QuestionAnswer"].ToString() == "11")
                        {
                            NACount = NACount + 1;
                        }

                    }
                    //<strong> </strong>
                    string NAEntries = "<B>Total Numerator : <SPAN style='color:navy'>" + NumValue + ", </SPAN>Total Denominator : <SPAN style='color:navy'>" + DenValue + ", </SPAN>Not Applicable Count : <SPAN style='color:navy'>" + NACount + " (out of Total : " + TotalCount + ")</SPAN></B>";

                    if (DenValue == 0) { DenValue = 1; }
                    double CompliancePercentage = ((double)NumValue / DenValue) * 100;

                    //Get Compus/building
                    List<ApiGetCategoryNamesReturnModel> lstcategoryNames;
                    lstcategoryNames = db.ApiGetCategoryNames(siteId, programId);

                    DataTable categoryNamesTable = BuildDataTable<ApiGetCategoryNamesReturnModel>(lstcategoryNames);
                    DataView categoryNames = categoryNamesTable.DefaultView;


                    string campusName = GetCategoryTitle(categoryNames, Enums.CategoryHierarchy.Campus);
                    string buildingName = GetCategoryTitle(categoryNames, Enums.CategoryHierarchy.Building);



                    rdlcName = "rptReportTracerObservationDetail.rdlc";
                    ReportParameter p1 = new ReportParameter("ReportTitle", reportTitle);
                    ReportParameter p2 = new ReportParameter("Copyright", "© " + DateTime.Now.Year.ToString() + WebConstants.Tracer_Copyright.ToString());
                    ReportParameter p3 = new ReportParameter("SiteName", siteName);
                    ReportParameter p4 = new ReportParameter("ProgramName", programName);
                    ReportParameter p5 = new ReportParameter("IsErrorOnly", IsErrorOnly.ToString());
                    ReportParameter p6 = new ReportParameter("IsGuestUser", IsGuestUser.ToString());
                    ReportParameter p7 = new ReportParameter("Building", buildingName);
                    ReportParameter p8 = new ReportParameter("Campus", campusName);
                    ReportParameter p9 = new ReportParameter("CompliancePercentage", string.Format("{0:0.0}", CompliancePercentage));
                    ReportParameter p10 = new ReportParameter("NAEntries", NAEntries);
                    ReportParameter p11 = new ReportParameter("CustomSections", customSections.ToString());
                    ReportParameter p12 = new ReportParameter("TracerName", tracerName.ToString());
                    reportParameterCollection = new ReportParameterCollection { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 };

                    reportParameterCollection.Add(new ReportParameter("TracerTypeID", tracerTypeID));

                    ReportDataSource rdsHeader = new ReportDataSource("dsReportTracerObservationHeader", dvReportHeader);
                    ReportDataSource rdsDetail = new ReportDataSource("dsReportTracerObservationDetail", dvReportDetail);
                    // Setup Data sources for report
                    reportViewer.LocalReport.DataSources.Clear();
                    reportViewer.LocalReport.EnableExternalImages = true;
                    reportViewer.LocalReport.ReportPath = System.Web.HttpContext.Current.Request.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath) + @"Reports\" + rdlcName.ToString();
                    reportViewer.LocalReport.DataSources.Add(rdsHeader);
                    reportViewer.LocalReport.DataSources.Add(rdsDetail);
                    reportViewer.LocalReport.SetParameters(reportParameterCollection);
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;
                    string format = "pdf";      // PDF is default
                    switch (pformatType)
                    {
                        case "pdf":
                        default:
                            {
                                format = "pdf";
                                break;
                            }

                        case "xlsx":
                            {
                                format = "EXCELOPENXML";
                                break;
                            }

                        case "docx":
                            {
                                format = "WORDOPENXML";
                                break;
                            }
                    }


                    fileContents = reportViewer.LocalReport.Render(format, null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileContents;
        }

        public byte[] TracerEmptyPrintForm(int pTracerCustomID, ref int customSections, int? userId, int? siteId, int? programId, string siteName, string programName, int pIsGuestUser = 0, string pformatType = "pdf")
        {
            byte[] fileContents = null;
            string rdlcName = String.Empty;
            string dsName = String.Empty;
            string tracerName = String.Empty;
            int? outCustomSection;

            ReportParameterCollection reportParameterCollection = null;

            try
            {
                // Setup ReportViewer 
                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;

                string reportTitle = "Tracer Observation Form";

                List<ApiReportTracerObservationReturnModel> _result = new List<ApiReportTracerObservationReturnModel>();

                // Get header and detail data sets
                using (var db = new DBMEdition01Context())
                {
                    
                    try
                    {
                        _result = db.ApiReportTracerObservation(pTracerCustomID.ToString(), out outCustomSection);
                    }
                    catch (Exception ex)
                    {

                        string sqlParam = "ApiReportTracerObservation(" + pTracerCustomID.ToString() + ", out outCustomSection"  + ")";
                        string methodName = "JCRAPI/Business/EmailServices/TracerEmptyPrintForm";
                        exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, userId, siteId, sqlParam, string.Empty);
                        return null;
                    }
                    // populate list
                    DataTable ListAsDataTable = BuildDataTable<ApiReportTracerObservationReturnModel>(_result);
                    DataView dvReportResult = ListAsDataTable.DefaultView;



                    // Get header data from caller
                    tracerName = dvReportResult[0]["TracerCustomName"].ToString();

                    if (tracerName.ToUpper() == "NULL")
                        tracerName = string.Empty;

                    List<ApiGetCategoryNamesReturnModel> lstcategoryNames;
                    lstcategoryNames = db.ApiGetCategoryNames(siteId, programId);

                    DataTable categoryNamesTable = BuildDataTable<ApiGetCategoryNamesReturnModel>(lstcategoryNames);
                    DataView categoryNames = ListAsDataTable.DefaultView;


                    string campusName = GetCategoryTitle(categoryNames, Enums.CategoryHierarchy.Campus);
                    string buildingName = GetCategoryTitle(categoryNames, Enums.CategoryHierarchy.Campus);

                    rdlcName = "rptReportTracerObservation.rdlc";
                    ReportParameter p1 = new ReportParameter("CustomTracerIDFilter", pTracerCustomID.ToString());
                    ReportParameter p2 = new ReportParameter("ReportTitle", reportTitle);
                    ReportParameter p3 = new ReportParameter("Copyright", "© " + DateTime.Now.Year.ToString() + WebConstants.Tracer_Copyright.ToString());
                    ReportParameter p4 = new ReportParameter("SiteName", siteName);
                    ReportParameter p5 = new ReportParameter("ProgramName", programName);
                    ReportParameter p6 = new ReportParameter("Building", buildingName);
                    ReportParameter p7 = new ReportParameter("Campus", campusName);
                    ReportParameter p8 = new ReportParameter("CustomSections", customSections.ToString());
                    ReportParameter p9 = new ReportParameter("TracerName", tracerName.ToString());

                    reportParameterCollection = new ReportParameterCollection { p1, p2, p3, p4, p5, p6, p7, p8, p9 };

                    ReportDataSource rds = new ReportDataSource("dsReportTracerObservation", dvReportResult);

                    // Setup Data sources for report
                    reportViewer.LocalReport.DataSources.Clear();

                    reportViewer.LocalReport.ReportPath = System.Web.HttpContext.Current.Request.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath) + @"Reports\" + rdlcName.ToString();
                    reportViewer.LocalReport.DataSources.Add(rds);


                    reportViewer.LocalReport.SetParameters(reportParameterCollection);
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    string format = "pdf";      // PDF is default
                    switch (pformatType)
                    {
                        case "pdf":
                        default:
                            {
                                format = "pdf";
                                break;
                            }

                        case "xlsx":
                            {
                                format = "EXCELOPENXML";
                                break;
                            }

                        case "docx":
                            {
                                format = "WORDOPENXML";
                                break;
                            }
                    }



                    fileContents = reportViewer.LocalReport.Render(format, null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fileContents;
        }
        public static DataTable BuildDataTable<T>(IList<T> lst)
        {
            //create DataTable Structure
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            int i = 0;
            try
            {
                foreach (T item in lst)
                {
                    i++;
                    if (item != null)
                    {
                        DataRow row = tbl.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (prop.GetValue(item) == null)
                                row[prop.Name] = DBNull.Value;
                            else
                                row[prop.Name] = prop.GetValue(item);
                        }
                        tbl.Rows.Add(row);
                    }

                }
            }
            catch (Exception ex)
            {
                int h = i;

            }
            return tbl;
        }

        private static DataTable CreateTable<T>()
        {
            //T –> ClassName
            Type entType = typeof(T);
            //set the datatable name as class name
            DataTable tbl = new DataTable(entType.Name);
            //get the property list
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            int count = 0;
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column
                try
                {
                    //tbl.Columns.Add(prop.Name, prop.PropertyType);
                    tbl.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    count++;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    int i = count;
                }
            }
            return tbl;
        }

        public bool SendEmailAttachemnt(Email email, int actionTypeId, byte[] fileContent, int userId, int siteId, string siteName, string fullName, string reportName)
        {
            bool returnStatus = true;

            try
            {
                // var reportName =  "Tracer By CMS Tag Report";

                string dtNow = DateTime.Now.ToString("MM-dd-yyyy_hhmmssfff_tt");

                Random rn = new Random(siteId + userId);
                var seedVal = rn.Next(99999999);

                // Generate a unique file name. 
                string filename = string.Format("{0}_{1}{2}.pdf", reportName, dtNow.ToString(), seedVal.ToString());
                
                // Create email body from email template file
                var nvc = new NameValueCollection(3);
                nvc["TONAME"] = fullName;
                nvc["MESSAGE"] = "Please find the report for observation <b>" + email.Title + "</b> attached.";
                nvc["COMMENTS"] = !string.IsNullOrEmpty(email.Comments) ? "Comments:" + email.Comments : "";

                // Create body from template
                email.Body = LoadEmailTemplate("EmailReport.htm", nvc);

                string fileGuid = FilesServices.SaveAttachmentFile(filename, fileContent, Enums.ApplicationCode.Tracers.ToString()); //, 
                email.Guid = fileGuid;

                //string path = email.AttachmentLocation[0];
                //if (path != null)
                //{
                //    int pos = path.LastIndexOf("\\") + 1;
                //    string fileName = path.Substring(pos, path.Length - pos);
                //    string fileGuid = FilesServices.SaveAttachmentFile(fileName, fileContent, Enums.ApplicationCode.Tracers.ToString()); //, 
                //    email.Guid = fileGuid;
                //}


                EmailHelpers.SendEmail(email, actionTypeId, siteId, userId, siteName, fullName);


            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
                returnStatus = false;

            }

            return returnStatus;
        }

        public static string LoadEmailTemplate(string templateFileName, NameValueCollection nvc)
        {
            return LoadEmailTemplate(
                System.Web.HttpContext.Current.Server.MapPath(@"~\EmailTemplates\"),
                templateFileName,
                nvc);
        }
        public string GetCategoryTitle(DataView dv, Enums.CategoryHierarchy categoryHierarchy)
        {
            if (dv == null)
                return string.Empty;

            dv.RowFilter = "Ranking =" + ((int)categoryHierarchy).ToString() + "AND IsActive = 1";

            if (dv.Count > 0)
                return dv[0]["Title"].ToString();
            else
                return string.Empty;
        }
        private static string LoadEmailTemplate(string templateFolder, string templateFileName, NameValueCollection nvc)
        {
            string html;
            using (var reader = new StreamReader(templateFolder + templateFileName))
            {
                html = reader.ReadToEnd();
                // reader.Close();
            }

            if (nvc != null)
            {
                foreach (string key in nvc.AllKeys)
                {
                    html = html.Replace("%%" + key + "%%", nvc.Get(key));
                }
            }

            return html;
        }

    
    }
}

