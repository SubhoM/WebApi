using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Web;
using Jcr.Api.Controllers;
using Jcr.Api.Models.Models;
using System.Net;

namespace UnitTestProject1
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void SaveTaskFilterMethod()
        {

            var controller = new TaskController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var usrPref = new UserPreference();

            usrPref.preferenceType = "TaskFilter";
            usrPref.PreferenceValue = "{\"page\":1,\"pageSize\":10,\"sort\":[{\"field\":\"DueDate\",\"dir\":\"asc\"}],\"group\":[],\"filter\":{\"filters\":[{\"operator\":\"contains\",\"value\":\"test\",\"field\":\"TaskName\"}],\"logic\":\"and\"}}";
            usrPref.SiteID = 18682;
            usrPref.ProgramID = 2;
            usrPref.UserID = 180364;


            // Act
            var response = controller.SaveTaskGridFilters(usrPref);

            // Assert

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

        }

        [TestMethod]
        public void GetTaskListMethodTest()
        {
            var controller = new TaskController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

           
            string standardEffDate = "07/01/2018";
            int? siteId = 14680;
            int? programId = 2;                         


            // Act
            var response = controller.GetTaskList(standardEffDate, siteId, programId, null, null);

            // Assert

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
