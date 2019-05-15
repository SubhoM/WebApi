using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jcr.Api.Controllers;
using System.Net.Http;
using System.Web.Http;
using Jcr.Api.Models.Models;
using System.Net;


namespace UnitTestProject1
{
    [TestClass]
    public class TracerTest
    {

        [TestMethod]
        public void CreateGuestUserTest()
        {
            // Arrange
            var controller = new TracerController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var gusr = new GuestUser();

            gusr.lstNewUserEmails = "kkranthi@jcrinc.com";
            gusr.siteID = 4;
            gusr.updateByID = 50331;


            // Act
            var response = controller.CreateGuestUserByEmailIds(gusr);

            // Assert

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

        }

        [TestMethod]
        public void GetInactiveUserEmailIdsTest()
        {
            // Arrange
            var controller = new TracerController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            
            var lstNewUserEmails = "srangarajan@jcrinc.com";

            //ApiValidateInactiveEmailIds(srangarajan@jcrinc.com)
            //ApiValidateInactiveEmailIds(srangarajan@jcrinc.com)

            // Act
            var response = controller.GetInactiveUserEmailIds(lstNewUserEmails);

            // Assert

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);

        }

    }
}