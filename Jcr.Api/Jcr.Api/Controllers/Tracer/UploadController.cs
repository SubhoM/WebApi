using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Configuration;
using Jcr.Api.Helpers;
using Jcr.Api.Models;
using System.Web;
using System.IO;
using Jcr.Business;
using Jcr.Data;
using Jcr.Api.ActionFilters;
using System.Drawing;

namespace Jcr.Api.Controllers.Tracer
{
    [AuthorizationRequired]
    [RoutePrefix("Files")]
    public class UploadController : ApiController
    {
        protected TrackingRepository TRepository;
        protected TracerService tracerService;
      
        public UploadController()
        {
            tracerService = new TracerService();
            TRepository = new TrackingRepository();
        }

        [VersionedRoute("post/image", 1)]
        [HttpPost]
        public async Task<FileUploadResult> UploadFile(int userID, int siteID, int programID, int tracerID, int tracerResponseID, int tracerQuestionID, string fileName, bool rotateImage = false)
        {
            //if (!Request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}
            var httpRequest = HttpContext.Current.Request;
            var fileResult = new FileUploadResult();
            foreach (string file in httpRequest.Files)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                var postedFile = httpRequest.Files[file];
                if (postedFile != null && postedFile.ContentLength > 0)
                {

                    fileResult = await UploadImage.UploadToAzure(postedFile.InputStream, userID, siteID, programID, tracerID, tracerResponseID, tracerQuestionID, Guid.NewGuid().ToString() + "_" + fileName, rotateImage, 0);

                }
                //TRepository.AddTracking();
            }
            return fileResult;
        }

    
        [VersionedRoute("get/image", 1)]
        public async Task<HttpResponseMessage> GetImage(int siteID, int programID, int tracerID, string fileName, bool actualImage = false)
        {
            try
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
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "file not found");
                }

                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
                Stream blobStream = await Blob.OpenReadAsync();
                
                message.Content = new StreamContent(blobStream);
                message.Content.Headers.ContentLength = Blob.Properties.Length;
                message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Blob.Properties.ContentType);
                message.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName,
                    Size = Blob.Properties.Length
                };

                return message;

            }
            catch (Exception ex)

            {

                return new HttpResponseMessage

                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                };

            }

        }

        [VersionedRoute("get/rotateimage", 1)]
        [HttpGet]
        public async Task<HttpResponseMessage> RotateImage(int siteID, int programID, int tracerID, string fileName, int timesToRotate)
        {
            try
            {
                //Azure settings
                var accountName = ConfigurationManager.AppSettings["storage:account:name"];
                var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
                var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                var uri = ConfigurationManager.AppSettings["azure:filepath"] + "Actual/" + siteID + "/" + programID + "/" + tracerID + "/" + fileName;

                var Blob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(uri));
                var isExist = await Blob.ExistsAsync();

                if (!isExist)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "file not found");
                }

              
                Stream blobStream = await Blob.OpenReadAsync();
                using (Bitmap sourceImage = new Bitmap(blobStream))
                {
                    await UploadImage.imageRotate(sourceImage, siteID, programID, tracerID, fileName, timesToRotate, true);

                }
             
                //end rotate actual image end

                //rotate thumb image

                uri = ConfigurationManager.AppSettings["azure:filepath"] + "Thumb/" + siteID + "/" + programID + "/" + tracerID + "/" + fileName;

                 Blob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(uri));
                 isExist = await Blob.ExistsAsync();

                if (!isExist)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "file not found");
                }


                blobStream = await Blob.OpenReadAsync();
                using (Bitmap sourceImage = new Bitmap(blobStream))
                {
                    await UploadImage.imageRotate(sourceImage, siteID, programID, tracerID, fileName, timesToRotate, false);
                }

                //end rotate thumb image

                return Request.CreateResponse(HttpStatusCode.OK, "File Rotated");

            }
            catch (Exception ex)

            {

                return new HttpResponseMessage

                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                };

            }

        }


        [VersionedRoute("get/tempimagedelete", 1)]
        [HttpGet]
        public async Task<HttpResponseMessage> TempImageClean(int tracerCustomID, int userID)
        {
            try
            {

              //  TracerServices service = new TracerServices();

                //GetTempImagestoClean
                List<ApiGetTracerImagesTempReturnModel> _result = tracerService.GetTempImagestoClean(tracerCustomID, userID);
                if (_result.Count > 0)
                {
                    //Azure settings
                    var container = UploadImage.azureContainer();
                    ICloudBlob _blockBlob;
                    //delete blob from container  
                    foreach (var tempImage in _result)
                    {
                        //delete Actual Image
                        _blockBlob = await container.GetBlobReferenceFromServerAsync("Actual/" + tempImage.SiteID + "/" + tempImage.ProgramID + "/" + tempImage.TracerCustomID + "/" + tempImage.ImageName);
                        _blockBlob.Delete();
                        //delete Thumb Image
                         _blockBlob = await container.GetBlobReferenceFromServerAsync("Thumb/" + tempImage.SiteID + "/" + tempImage.ProgramID + "/" + tempImage.TracerCustomID + "/" + tempImage.ImageName);
                        _blockBlob.Delete();
                    }

                    tracerService.DeleteImagesTemp(tracerCustomID, userID);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Files Deleted");
                
            }
            catch (Exception ex)
            {
                ex.Data.Add("tracerCustomId", tracerCustomID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/Image/Delete");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }
        [VersionedRoute("get/deleteimagebyname", 1)]
        [HttpGet]
        public async Task<HttpResponseMessage> DeleteImageByName(int siteID, int programID, int tracerCustomID, int tracerQuestionID, int tracerResponseID, int userID, string imageName)
        {
            try
            {

             //   TracerServices service = new TracerServices();


                var container = UploadImage.azureContainer();
                ICloudBlob _blockBlob;
                    //delete blob from container  
                  
                        //delete Actual Image
                        _blockBlob = await container.GetBlobReferenceFromServerAsync("Actual/" + siteID + "/" + programID + "/" + tracerCustomID + "/" + imageName);
                        _blockBlob.Delete();
                        //delete Thumb Image
                        _blockBlob = await container.GetBlobReferenceFromServerAsync("Thumb/" + siteID + "/" + programID + "/" + tracerCustomID + "/" + imageName);
                        _blockBlob.Delete();


                tracerService.DeleteImageByImageName(tracerCustomID, tracerQuestionID, tracerResponseID, userID, imageName);
             
                return Request.CreateResponse(HttpStatusCode.OK, "File Deleted");

            }
            catch (Exception ex)
            {
                ex.Data.Add("tracerCustomId", tracerCustomID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/Image/Delete");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }

        }

 
        [VersionedRoute("get/imagescount", 1)]
        [HttpGet]
        public HttpResponseMessage GetImagesCount(int tracerResponseID)
        {

          //  TracerServices service = new TracerServices();
            try
            {
                var imagesCount = tracerService.GetImagesCount(tracerResponseID);
             
              return Request.CreateResponse(HttpStatusCode.OK, imagesCount);
               
            }
            catch (Exception ex)
            {
                ex.Data.Add("TracerReponseID", tracerResponseID);
                ex.Data.Add("HTTPReferrer", "JCRAPI/Image/GetImagesCount");
                WebExceptionHelper.LogException(ex, null);
                return null;
            }


        }

    }
}
