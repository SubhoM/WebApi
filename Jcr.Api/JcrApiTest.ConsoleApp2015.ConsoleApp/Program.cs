using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;


namespace JcrApiTest2015.ConsoleApp
{
    class Program
    {
        //---------Test sites---------------------------
        // DEVSQL 02 services.dev.devjcrinc.com
        // DEVSQL 01 services.devb.devjcrinc.com
        // DEVSQL 02 localhost:53421/
        //-----------------------------------------------
          static string baseAddress = "http://localhost:53421/";
        //static string baseAddress = "http://services.dev.devjcrinc.com/";
        //static string baseAddress = "http://services.devb.devjcrinc.com/";
        //static string baseAddress = " http://services.main.devjcrinc.com/";
        //  static string baseAddress = "https://services.uat.devjcrinc.com/";

        static void Main(string[] args)                                 
        {
            using (var client = new HttpClient())
            {

                var Name = "Ksultz@jcrinc.com";
                var Password = "1234";

                // PostDataWihtoutToken();
                //var Password = "123";
              
                var byteArray = new UTF8Encoding().GetBytes(Name + ":" + Password);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var formData = new List<KeyValuePair<string, string>>();

                var tokenResponse = client.PostAsync(baseAddress + "Authenticate/get/token", new FormUrlEncodedContent(formData)).Result;
                


                if (tokenResponse.IsSuccessStatusCode)
                {

                    var token = tokenResponse.Headers.GetValues("Token").FirstOrDefault();
                    // var userId = tokenResponse.Headers.GetValues("UserId").FirstOrDefault();
                    if (!string.IsNullOrEmpty(token))
                    {
                        //---GET---
                           GetData(token);

                        //---POST---
                        //   PostData(token);
                    }
                    else
                    {
                        //Error
                    }
                }
                else
                {
                    var errMsg = tokenResponse.Headers.GetValues("UnauthorizedMessage").FirstOrDefault();
                }
            }
        }
        private static void PostData(string token)
        {
            using (var client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("UserId", "56");
                //**********Insert EULA Status *************
                //var formData = new List<KeyValuePair<string, string>>();
                //formData.Add(new KeyValuePair<string, string>("UserId", "100106"));
                //formData.Add(new KeyValuePair<string, string>("AttributeTypeId", "304"));
                //formData.Add(new KeyValuePair<string, string>("AttributeValue", DateTime.Now.ToString()));
                //formData.Add(new KeyValuePair<string, string>("AttributeActivationDate", DateTime.Now.ToString()));
                //formData.Add(new KeyValuePair<string, string>("AttributeExpirationDate", ""));

                //  var tokenResponse = client.PostAsync(baseAddress + "UserInfo/AddUserSecurityAttribute", new FormUrlEncodedContent(formData)).Result;

                //**********Save Observation*************
                //var formData = new List<KeyValuePair<string, string>>();

                //formData.Add(new KeyValuePair<string, string>("TracerId", "90240"));
                //formData.Add(new KeyValuePair<string, string>("ObservationId", "1596207"));
                //formData.Add(new KeyValuePair<string, string>("UserId", "147626 "));
                //formData.Add(new KeyValuePair<string, string>("Title", "Test inactive building 10/19 1"));
                //formData.Add(new KeyValuePair<string, string>("ObservationDate", "10/19/2017 12:00:00 AM"));
                //formData.Add(new KeyValuePair<string, string>("DepartmentId", "0/64851"));  ////departmentValue + "/" + buildingValue + "/" + campusValue;
                //formData.Add(new KeyValuePair<string, string>("SurveyTeam", ""));
                //formData.Add(new KeyValuePair<string, string>("MedicalStaffInvolved", ""));
                //formData.Add(new KeyValuePair<string, string>("Location", ""));
                //formData.Add(new KeyValuePair<string, string>("IsCalledByGuestAccess", "false"));
                //formData.Add(new KeyValuePair<string, string>("ResponseStatusID", "7"));
                //formData.Add(new KeyValuePair<string, string>("ObservationStatusId", "7"));



                //formData.Add(new KeyValuePair<string, string>("TracerId", "88069"));
                //formData.Add(new KeyValuePair<string, string>("ObservationId", ""));
                //formData.Add(new KeyValuePair<string, string>("UserId", "56"));
                //formData.Add(new KeyValuePair<string, string>("Title", "EC Rounds 2017 -Grace H 5G- 07/11/2017 11:07.695  AM"));
                //formData.Add(new KeyValuePair<string, string>("ObservationDate", "9/8/2017 11:13:04 AM "));
                //formData.Add(new KeyValuePair<string, string>("DepartmentId", "20440/20441/20442"));  ////departmentValue + "/" + buildingValue + "/" + campusValue;
                //formData.Add(new KeyValuePair<string, string>("SurveyTeam", ""));
                //formData.Add(new KeyValuePair<string, string>("MedicalStaffInvolved", ""));
                //formData.Add(new KeyValuePair<string, string>("Location", ""));
                //formData.Add(new KeyValuePair<string, string>("IsCalledByGuestAccess", "fale"));


                //formData.Add(new KeyValuePair<string, string>("TracerId", "89351"));
                //formData.Add(new KeyValuePair<string, string>("ObservationId", "1534076"));
                //formData.Add(new KeyValuePair<string, string>("UserId", "147626"));
                //formData.Add(new KeyValuePair<string, string>("Title", "Min's Tracer with question in new section only - Navdeep1 Sohal - 10/2/2017 1:52:24 PM"));
                //formData.Add(new KeyValuePair<string, string>("ObservationDate", "10/02/2017 11:11:04 AM "));
                //formData.Add(new KeyValuePair<string, string>("DepartmentId", "46689/27953"));  ////departmentValue + "/" + buildingValue + "/" + campusValue;
                //formData.Add(new KeyValuePair<string, string>("SurveyTeam", "1"));
                //formData.Add(new KeyValuePair<string, string>("MedicalStaffInvolved", "2"));
                //formData.Add(new KeyValuePair<string, string>("Location", "3"));
                //formData.Add(new KeyValuePair<string, string>("IsCalledByGuestAccess", "fale"));




                //  var tokenResponse = client.PostAsync(baseAddress + "TracerInfo/MobileSaveObservation", new FormUrlEncodedContent(formData)).Result;

                //  **********Save Question Answer*************
                //var formData = new List<KeyValuePair<string, string>>();
                //formData.Add(new KeyValuePair<string, string>("ObservationId", "1202211"));
                //formData.Add(new KeyValuePair<string, string>("UserId", "68335"));
                //formData.Add(new KeyValuePair<string, string>("TracerQuestionId", "3081266"));
                //formData.Add(new KeyValuePair<string, string>("TracerQuestionAnswerId", "22950017"));
                //formData.Add(new KeyValuePair<string, string>("Numerator", "1"));
                //formData.Add(new KeyValuePair<string, string>("Denominator", "1"));
                //formData.Add(new KeyValuePair<string, string>("QuestionAnswer", "11"));
                //formData.Add(new KeyValuePair<string, string>("QuestionNoteID", "895940"));
                //formData.Add(new KeyValuePair<string, string>("QuestionNote", "Test from Client - 9.21.2017.01"));


                //var tokenResponse = client.PostAsync(baseAddress + "TracerInfo/MobileUpdateQuestionAnswer", new FormUrlEncodedContent(formData)).Result;

                //**********Publish Tracer*************

                // var formData = new List<KeyValuePair<string, string>>();
                // var tokenResponse = client.PostAsync(baseAddress + "TracerInfo/PublishTracer?tracerID=4599990&updatedByID=41008", new FormUrlEncodedContent(formData)).Result;

                //**********Unpublish Tracer*************                
                // var formData = new List<KeyValuePair<string, string>>();
                //  var tokenResponse = client.PostAsync(baseAddress + "TracerInfo/UnpublishTracer?tracerID=4599990&updatedByID=41008", new FormUrlEncodedContent(formData)).Result;

                ////**********verify User Security Question Answers*************                
                //List<SecurityQuestionAnswer> answers = new List<SecurityQuestionAnswer>();
                //SecurityQuestionAnswer answer1 = new SecurityQuestionAnswer();
                //answer1.QuestionId = 313;
                //answer1.Answer = "color";
                //answers.Add(answer1);
                //SecurityQuestionAnswer answer2 = new SecurityQuestionAnswer();
                //answer2.QuestionId = 316;
                //answer2.Answer = "movie";
                //answers.Add(answer2);
                //// var content = new FormUrlEncodedContent(answers);
                //var tokenResponse = client.PostAsJsonAsync<List<SecurityQuestionAnswer>>(baseAddress + "UserInfo/VerifyAnswer?userId=56", answers).Result;

                //309, 310
                ////**********Save User Security Question Answers*************                

                //var formData = new List<KeyValuePair<string, string>>();
                //var tokenResponse = client.PostAsync(baseAddress + "UserInfo/UpdateUserSecurityAnswer?userId=260144&attributeTypeId=313&attributeValue=RED&parentCodeId=309", new FormUrlEncodedContent(formData)).Result;



                //**********Update password*************
                // var formData = new List<KeyValuePair<string, string>>();
                //var NewPassword = "abceee";
                //var byteArray = new UTF8Encoding().GetBytes(NewPassword);
                //NewPassword = Convert.ToBase64String(byteArray);
                ////var tokenResponse = client.PostAsync(baseAddress + "UserInfo/UpdateUserPassword?userId=56&password=" + NewPassword, new FormUrlEncodedContent(formData)).Result;
                // var tokenResponse = client.PostAsync(baseAddress + "UserInfo/UpdateUserPassword?userId=264299&password=.", new FormUrlEncodedContent(formData)).Result;

                //**********Update password with restriction rules*************
                //var formData = new List<KeyValuePair<string, string>>();
                //var NewPassword = "Software2";
                //var byteArray = new UTF8Encoding().GetBytes(NewPassword);
                //NewPassword = Convert.ToBase64String(byteArray);
                //var tokenResponse = client.PostAsync(baseAddress + "UserInfo/UpdateUserPassword?userId=264299&password=" + NewPassword + "&isWithRules=true", new FormUrlEncodedContent(formData)).Result;


                ////// var tokenResponse = client.PostAsync(baseAddress + "UserInfo/UpdateUserPassword?userId=56&password=.", new FormUrlEncodedContent(formData)).Result;

                // **********Send Email*************
                var formData = new List<KeyValuePair<string, string>>();

                formData.Add(new KeyValuePair<string, string>("Attachment", ""));
                formData.Add(new KeyValuePair<string, string>("AttachmentLocation", ""));
                formData.Add(new KeyValuePair<string, string>("Bcc", ""));
                formData.Add(new KeyValuePair<string, string>("Body", ""));
                formData.Add(new KeyValuePair<string, string>("Cc", ""));
                formData.Add(new KeyValuePair<string, string>("Comments", ""));
                formData.Add(new KeyValuePair<string, string>("From", ""));
                formData.Add(new KeyValuePair<string, string>("Guid", ""));
                formData.Add(new KeyValuePair<string, string>("MultipleAttachment", "false"));
                formData.Add(new KeyValuePair<string, string>("ReportName", ""));
                formData.Add(new KeyValuePair<string, string>("Subject", "Tracer Observation Detail Report - Hospital"));
                formData.Add(new KeyValuePair<string, string>("Title", "10/10/2017"));
                formData.Add(new KeyValuePair<string, string>("To", "ghan@jcrinc.com"));

               var tokenResponse = client.PostAsync(baseAddress + "TracerInfo/SendTracerEmail?AttachmentName=Observation+Detail+Report+-+Hospital&userId=56&siteId=21600&programId=2&TracerId=1675208&siteName=JCR+Demo+Site+A+(jcrz.com)&fullName=Navdeep1+Sohal&programName=Hospital&pIsGuestUser=0&pIsErrorOnly=0&formType", new FormUrlEncodedContent(formData)).Result;
                //var tokenResponse = client.PostAsync(baseAddress + "TracerInfo/SendTracerEmail?AttachmentName=Observation+Detail+Report+-+Hospital&userId=56&siteId=21600&programId=2&TracerId=1164&siteName=JCR+Demo+Site+A+(jcrz.com)&fullName=Navdeep1+Sohal&programName=Hospital&pIsGuestUser=0&pIsErrorOnly=0&formType", new FormUrlEncodedContent(formData)).Result;

                //**********MenuStateSaveArgr*************
                //  var formData = new List<KeyValuePair<string, string>>();
                //   var tokenResponse = client.PostAsync(baseAddress + "MenuInfo/MenuStateSaveArg?userId=56&key=&value=", new FormUrlEncodedContent(formData)).Result;

                //**********************upload image *****************


                //var formData = new List<KeyValuePair<string, string>>();
                //var tokenResponse = client.PostAsync(baseAddress + "Files/Post/Image?userID=147626&siteID=41476&programID=2&tracerID=89351&tracerResponseID=1533836&tracerQuestionID=1533836&fileName=IMG_20170918_150037.jpg", new FormUrlEncodedContent(formData)).Result;
                // **********Send Tracer Mobile Customer Support Email*************
                //var formData = new List<KeyValuePair<string, string>>();

                //formData.Add(new KeyValuePair<string, string>("Email", "ghanAsUser@jcrinc.com"));
                //formData.Add(new KeyValuePair<string, string>("UserName", "Grace H"));
                //formData.Add(new KeyValuePair<string, string>("UserID", "147626"));
                //formData.Add(new KeyValuePair<string, string>("SiteID", "12379"));
                //formData.Add(new KeyValuePair<string, string>("HCOID", "0"));
                //formData.Add(new KeyValuePair<string, string>("ProgramName", "Hospital"));
                //formData.Add(new KeyValuePair<string, string>("Body", "Test Body"));
                //formData.Add(new KeyValuePair<string, string>("Subject", "Test Subject"));

                //var tokenResponse = client.PostAsync(baseAddress + "TracerInfo/SendTracerCustomerSupportEmail", new FormUrlEncodedContent(formData)).Result;

                //**********Guest Link via LINK STEP 1A:  Insert New User First&Last Name *************   


                //**********verify User Security Question Answers*************                
                //        List<TracerQuestionAnswerGroup> list = new List<TracerQuestionAnswerGroup>();
                //        TracerQuestionAnswerGroup list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384492;
                //        list1.TracerQuestionAnswerId = 31696314;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384493;
                //        list1.TracerQuestionAnswerId = 31696315;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384494;
                //        list1.TracerQuestionAnswerId = 31696316;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384495;
                //        list1.TracerQuestionAnswerId = 31696317;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384496;
                //        list1.TracerQuestionAnswerId = 31696318;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384497;
                //        list1.TracerQuestionAnswerId = 31696319;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384498;
                //        list1.TracerQuestionAnswerId = 31696320;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384499;
                //        list1.TracerQuestionAnswerId = 1696321;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384500;
                //        list1.TracerQuestionAnswerId = 31696322;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384501;
                //        list1.TracerQuestionId = 31696323;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384502;
                //        list1.TracerQuestionId = 31696324;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384503;
                //        list1.TracerQuestionId = 31696325;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384504;
                //        list1.TracerQuestionId = 31696326;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384505;
                //        list1.TracerQuestionId = 31696327;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384506;
                //        list1.TracerQuestionId = 31696328;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384507;
                //        list1.TracerQuestionId = 31696329;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384508;
                //        list1.TracerQuestionId = 31696330;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384509;
                //        list1.TracerQuestionId = 31696331;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384510;
                //        list1.TracerQuestionId = 31696332;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384511;
                //        list1.TracerQuestionId = 31696333;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384512;
                //        list1.TracerQuestionId = 31696334;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384513;
                //        list1.TracerQuestionId = 31696335;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384480;
                //        list1.TracerQuestionId = 31696302;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384481;
                //        list1.TracerQuestionId = 316963032;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384483;
                //        list1.TracerQuestionId = 316963035;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384484;
                //        list1.TracerQuestionId = 316963036;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384485;
                //        list1.TracerQuestionId = 316963037;
                //        list.Add(list1);
                //        list1 = new TracerQuestionAnswerGroup();
                //        list1.TracerQuestionId = 3384486;
                //        list1.TracerQuestionId = 316963038;
                //        list.Add(list1);
                //        var tokenResponse = client.PostAsJsonAsync<List<TracerQuestionAnswerGroup>>(baseAddress + "TracerInfo/MobileGroupUpdateToNotApplicable?ObservationId=1675062&UserId=154528", list).Result;

                //        if (tokenResponse.IsSuccessStatusCode)
                //        {
                //            var t = tokenResponse.Content.ReadAsStringAsync().Result;
                //        }
                //        else
                //        {
                //            var error = tokenResponse.Content.ReadAsStringAsync().Result;
                //        }
                //    }
                //}

                //private static void PostDataWihtoutToken()
                //{
                //    using (var client = new HttpClient())
                //    {

                //        //**********verify User Security Question Answers*************                
                //        List<SecurityQuestionAnswer> answers = new List<SecurityQuestionAnswer>();
                //        SecurityQuestionAnswer answer1 = new SecurityQuestionAnswer();
                //        answer1.QuestionId = 313;
                //        answer1.Answer = "color";
                //        answers.Add(answer1);
                //        SecurityQuestionAnswer answer2 = new SecurityQuestionAnswer();
                //        answer2.QuestionId = 316;
                //        answer2.Answer = "movie";
                //        answers.Add(answer2);
                //        // var content = new FormUrlEncodedContent(answers);
                //        var tokenResponse = client.PostAsJsonAsync<List<SecurityQuestionAnswer>>(baseAddress + "UserInfo/VerifyAnswer?userId=56", answers).Result;


                //        if (tokenResponse.IsSuccessStatusCode)
                //        {
                //            var t = tokenResponse.Content.ReadAsStringAsync().Result;

                //        }
                //        else
                //        {
                //            var error = tokenResponse.Content.ReadAsStringAsync().Result;
                //        }
            }
        }
        private static void GetData(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri(baseAddress); 

                httpClient.DefaultRequestHeaders.Add("token", token);
                httpClient.DefaultRequestHeaders.Add("UserId", "56");
                // client.DefaultRequestHeaders.Add("UserId", "56");
                //**********Get EULA 123882**********
                // HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/GetTracerEULA?UserId=56").Result;
                //**********Get Sites by User**********
                //HttpResponseMessage response1 = httpClient.GetAsync("GetCommonInfo/site?userId=147626&siteId=&filteredsites=").Result;
                // HttpResponseMessage response1 = httpClient.GetAsync("GetCommonInfo/site?userId=262584&siteId=&filteredsites=").Result;
                //HttpResponseMessage response1 = httpClient.GetAsync("GetCommonInfo/site?userId=97921&siteId=0&filteredsites=false").Result;
                //**********Get TRACER Programs by User, site, and cycle**********
                // HttpResponseMessage response1 = httpClient.GetAsync("GetCommonInfo/program?siteId=12379&standardEffBeginDate&productId=2").Result;
                //**********Get Tracer By SiteID**********
                // HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/TracerDetail?siteId=4&programId=2").Result;
                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/TracerDetail?siteId=0&programId=0&statusId=1").Result;
                //**********Get Observation Category Name (page layout)**********
                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/GetTracerCategoryNames?siteId=12379&programId=2").Result;

                //**********Get TracerObservations (page layout)**********
                // HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/MobileTracerObservations?tracerId=73745&responseStatusCSV=7,8").Result;


                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/TracerHeader?tracerCustomId=90233&tracerResponseId=1596289&siteId=12379&programId=2&UserId=147626").Result;
                //**********Get Department Hierarchy (Campus, Department, Building)**********
                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/GetDepartmentHierarchy?siteId=15804&programId=2&rankId=1&isCategoryActive=&isCategoryItemActive=").Result;

                //**********Get Obversation by Tracer CustomerID**********
                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/Obversation?TracerCustomId=73745&statusID=&deviceTypeId=2").Result;
                //**********Get Questions**********
                // HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/Questions?tracerId=16239").Result;
                //**********Get Question Detail**********
                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/MobileTracerQuestion?tracerCustomId= 5684&tracerQuestionId= 323152&tracerResponseId=0&userId= 10298&deviceTypeId=2").Result;
                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/MobileTracerQuestion?tracerCustomId=89158&tracerQuestionId=3384489&tracerResponseId=1596069&userId=147626").Result;


                //**********Get Question Detail with validation message**********
                //HttpResponseMessage response1 = httpClient.GetAsync("TracerInfo/MobileQuestionsWithValidationMessage?tracerCustomId=89386&tracerResponseId=1534159&IsGuestAccess=").Result;


                //**********Get Security Questions 309 group 1, 310 group 2*************                
                HttpResponseMessage response1 = httpClient.GetAsync("PasswordInfo/GetSecurityQuestions?questionTypeID=309").Result;

                //**********Get user profile by userID*************                
                //  HttpResponseMessage response1 = httpClient.GetAsync("UserInfo/GetUser?logonId=newadmin@jcrinc.com").Result; 

                //**********Get SAVED user Security Questions*************                
                //  HttpResponseMessage response1 = httpClient.GetAsync("UserInfo/GetSQuestions?userId=264318").Result; 


                ////**********Mark's Menu*************                
                //HttpResponseMessage response1 = httpClient.GetAsync("MenuInfo/GetMenuState?userId=100106").Result;

                //******Get User Role by Site
                //  HttpResponseMessage response1 = httpClient.GetAsync("UserInfo/GetUserRoleBySite?userId=264301&siteId=12379").Result;

                //**********Create Guest User PassCode*************                
                //HttpResponseMessage response1 = httpClient.GetAsync("GuestUser/CreateEmailCode?email=ksultz@jcrinc.com").Result;

                //**********Guest Link via LINK STEP 1 *************                
                //Existing user
                //HttpResponseMessage response1 = httpClient.GetAsync("GuestUser/GuestAccessWithLink?email=ksultz@jcrinc.com&lnk=22c3c9958d7f4758850c8ccf221895ed").Result;
                //New User
                //HttpResponseMessage response1 = httpClient.GetAsync("GuestUser/GuestAccessWithLink?email=testA1031@jcrinc.com&lnk=dbb66f502da4410bbf48fe7048272f29").Result;
                //**********Guest Link via LINK STEP 1A:  Insert New User First&Last Name *************         
                // Go to POST DATA

                //HttpResponseMessage response1 = httpClient.GetAsync("GuestUser/ValidateTracerGuestUserByEmail?email=ksultz@jcrinc.com").Result;




                //**********Guest User's Tracer *************                

                // HttpResponseMessage response1 = httpClient.GetAsync("GuestUser/GetGuestUserTracersBySiteProgram?userId=264346&siteId=12379&programId=2&statusId=7").Result;

                //Guest User - available sites
                // HttpResponseMessage response1 = httpClient.GetAsync("GuestUser/GetGuestUserSites?userId=266532&email=ksultz@1.com").Result;

                if (response1.IsSuccessStatusCode)

                {
                    System.Console.WriteLine(response1.Content.ReadAsStringAsync().Result);
                }
                System.Console.WriteLine("URL response 1: " + response1.Content.ReadAsStringAsync().Result);

            }
        }



        private static void GetData2()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string baseAddress = "http://localhost:53421/";
                httpClient.BaseAddress = new Uri(baseAddress);
                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);
                //  httpClient.DefaultRequestHeaders.Add("token", token.AccessToken);
                // httpClient.DefaultRequestHeaders.Add("userName", "sitecore");
                // userLogonID, int? hcoId, int? siteId)
                //HttpResponseMessage response1 = httpClient.GetAsync("Licenses/Get?userLogonID=denise.lord@va.gov&hcoId=&siteId=").Result;
                HttpResponseMessage response1 = httpClient.GetAsync("GuestUser/GuestAccess?email=dzde@tt.com&lnk=dbb66f502da4410bbf48fe7048272f29").Result;
                
                if (response1.IsSuccessStatusCode)
                {
                    System.Console.WriteLine("Success");
                }
                System.Console.WriteLine("URL response 1: " + response1.Content.ReadAsStringAsync().Result);


              
            }
        }


    }
    public class SecurityQuestionAnswer
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class TracerQuestionAnswerGroup
    {
        public int? TracerQuestionId { get; set; }
        public int? TracerQuestionAnswerId { get; set; }

    }
}