using System;
using System.IO;
using System.Configuration;
using System.Drawing;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Jcr.Api.Models;
using System.Threading.Tasks;
using Jcr.Business;
using Jcr.Data;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Web;

namespace Jcr.Api.Helpers
{
    public class UploadImage
    {
       

        public static async Task<Boolean> imageRotate(Bitmap sourceImage, int siteID, int programID, int tracerID, string fileName, int timesToRotate, bool actualImage = false)
        {
            try
            {
                var container = azureContainer();
                await container.CreateIfNotExistsAsync();
                var folderName = actualImage ? "Actual/" : "Thumb/";
                CloudBlockBlob blob = container.GetBlockBlobReference(folderName + siteID + "/" + programID + "/" + tracerID + "/" + fileName);

                switch (timesToRotate)
                {
                    case 1:
                    default:
                        sourceImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 2:
                        sourceImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 3:
                        sourceImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }

                MemoryStream memoryStream = new MemoryStream();
                sourceImage.Save(memoryStream, GetImageFormat(fileName));
                memoryStream.Position = 0;
                blob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);
                blob.UploadFromStream(memoryStream);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
          
        }
        public static async Task<FileUploadResult> UploadToAzure(Stream streamImg, int userID, int siteID, int programID, int tracerID, int tracerResponseID, int tracerQuestionID, string fileName, bool rotateImage = false, int clockRotation = 0)
        {
            Bitmap sourceImage = new Bitmap(streamImg);
            double imgHeight = sourceImage.Size.Height;
            double imgWidth = sourceImage.Size.Width;

            var container = azureContainer();
            await container.CreateIfNotExistsAsync();
            
            //Actual Image Start

            CloudBlockBlob blob =  container.GetBlockBlobReference("Actual/" + siteID + "/" + programID + "/" + tracerID + "/" + fileName);
            //minimise actual image size as required. 
            double x = imgWidth / 1000;
            int newWidth = Convert.ToInt32(imgWidth / x);
            int newHeight = Convert.ToInt32(imgHeight / x);
            rotateImage = false;
            if (imgWidth <= newWidth)
            {
                blob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName); // contentType;
                if (rotateImage && clockRotation > 0)
                {

                    switch (clockRotation)
                    {
                        case 90:
                        default:
                            sourceImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 180:
                            sourceImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 270:
                            sourceImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }

                   
                    MemoryStream memoryStream = new MemoryStream();
                    sourceImage.Save(memoryStream, GetImageFormat(fileName));
                    memoryStream.Position = 0;
                    blob.UploadFromStream(memoryStream);
                }
                else
                {
                    streamImg.Position = 0;
                    blob.UploadFromStream(streamImg);
                }
                
            }
            else
            {
                imageUpload(newWidth, newHeight, blob, sourceImage, fileName, rotateImage, clockRotation);
            }

            //Actual Image End

            //Resize Image Start

            blob = container.GetBlockBlobReference("Thumb/" + siteID + "/" + programID + "/" + tracerID + "/" + fileName);

            // Getting Decreased Size
            x = imgWidth / 200;
            newWidth = Convert.ToInt32(imgWidth / x);
            newHeight = Convert.ToInt32(imgHeight / x);

            if (imgWidth <= newWidth)
            {
                blob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);
                if (rotateImage && clockRotation > 0)
                {
                    switch (clockRotation)
                    {
                        case 90:
                        default:
                            sourceImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 180:
                            sourceImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 270:
                            sourceImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }


                    MemoryStream memoryStream = new MemoryStream();
                    sourceImage.Save(memoryStream, GetImageFormat(fileName));
                    memoryStream.Position = 0;
                    blob.UploadFromStream(memoryStream);
                    
                }
                else
                {
                    streamImg.Position = 0;
                    blob.UploadFromStream(streamImg);
                }
            }
            else
            {
                imageUpload(newWidth, newHeight, blob, sourceImage, fileName, rotateImage, clockRotation);
            }
            var tempImageID = 0;

            TracerService service = new TracerService();
            tempImageID = service.TracerTempImage(siteID, programID, tracerID, tracerQuestionID, fileName, userID);

            if (tracerResponseID > 0)
            {
                service.TracerImage(tracerQuestionID, tracerResponseID, tempImageID.ToString(), userID);
            }
             
                //Resize Image End
            return new FileUploadResult
            {
                AzureFilePath = "",
                FileName = fileName,
                IsFileUploaded = true,
                Message = "Success",
                TempImageID= tempImageID
            };

        }

        public static CloudBlobContainer azureContainer() {

            //Azure settings
            var accountName = ConfigurationManager.AppSettings["storage:account:name"];
            var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("tracerimages"); // must always be lowercase
            return container;
        }

        private static void imageUpload(int newWidth, int newHeight, CloudBlockBlob blob, Bitmap sourceImage, string fileName, bool rotateImage = false, int clockRotation = 0)
        {

            using (Bitmap objBitmap = new Bitmap(newWidth, newHeight))
            {
                objBitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
                using (Graphics objGraphics = Graphics.FromImage(objBitmap))
                {
                    // Set the graphic format for better result cropping   
                    objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    objGraphics.DrawImage(sourceImage, 0, 0, newWidth, newHeight);

                    // Save the file path, note we use png format to support png file   

                    MemoryStream memoryStream = new MemoryStream();
                    if (rotateImage && clockRotation > 0)
                    {
                        switch (clockRotation)
                        {
                            case 90:
                            default:
                                objBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case 180:
                                objBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case 270:
                                objBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                        }
                       
                    }
                 
                        objBitmap.Save(memoryStream, GetImageFormat(fileName));
                        blob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);
                        memoryStream.Position = 0;
                        blob.UploadFromStream(memoryStream);
               
                }
            }

        }

        private static ImageFormat GetImageFormat(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException(
                    string.Format("Unable to determine file extension for fileName: {0}", fileName));

            switch (extension.ToLower())
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".ico":
                    return ImageFormat.Icon;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                case @".tif":
                case @".tiff":
                    return ImageFormat.Tiff;

                case @".wmf":
                    return ImageFormat.Wmf;

                default:
                    throw new NotImplementedException();
            }
        }

        public void DeleteTempBlobByTracer(int tracerCustomID, int userID)
        {
            //Azure settings
       
            CloudBlobContainer container = azureContainer(); 
            CloudBlockBlob _blockBlob;

            TracerService service = new TracerService();

            //GetTempImagestoClean
            List<ApiGetTracerImagesTempReturnModel> _result = service.GetTempImagestoClean(tracerCustomID, userID);
            if (_result.Count > 0)
            {
                //delete blob from container  
                foreach (var tempImage in _result)
                {
                    //delete Actual Image
                    _blockBlob = container.GetBlockBlobReference("Actual/" + tempImage.SiteID + "/" + tempImage.ProgramID + "/" + tempImage.TracerCustomID + "/" + tempImage.ImageName);
                    _blockBlob.Delete();
                    //delete Thumb Image
                    _blockBlob = container.GetBlockBlobReference("Thumb/" + tempImage.SiteID + "/" + tempImage.ProgramID + "/" + tempImage.TracerCustomID + "/" + tempImage.ImageName);
                    _blockBlob.Delete();
                }
            }
           
        }
        
    }
}