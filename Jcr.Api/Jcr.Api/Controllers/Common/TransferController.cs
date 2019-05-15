using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using Jcr.Api.Controllers.Tracer;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Jcr.Business;

namespace Jcr.Api.Controllers
{
    public class TransferController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public async Task<FileResult> GetImage(int userID, int siteID, int programID, int tracerID, string fileName, bool actualImage = false)
        {
            try
            {
                TokenServices tokenService = new TokenServices();
                if (tokenService.CheckUserToken(userID))
                {
                    //Azure settings
                    var accountName = ConfigurationManager.AppSettings["storage:account:name"];
                    var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
                    var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    var folderName = actualImage ? "Actual/" : "Thumb/";

                    var uri = ConfigurationManager.AppSettings["azure:filepath"] + folderName + siteID + "/" + programID + "/" + tracerID + "/" + fileName;

                    var Blob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(uri));
                    var isExist = await Blob.ExistsAsync();

                    if (!isExist)
                    {
                        // "file not found"
                        return null;
                    }
                    else {
                        Stream blobStream = await Blob.OpenReadAsync();
                        byte[] streamdata = new BinaryReader(blobStream).ReadBytes((int)Blob.Properties.Length);

                        var contentType = Blob.Properties.ContentType.ToString();
                        contentType = contentType.Contains("octet") ? MimeMapping.GetMimeMapping(fileName) : contentType;

                        return File(streamdata, contentType, fileName);
                    }                   

                }
                else
                    return null;
               
            }
            catch (Exception ex)
            {
                var Content = new StringContent(ex.Message);
                return null;
            }
        }
    }

}


