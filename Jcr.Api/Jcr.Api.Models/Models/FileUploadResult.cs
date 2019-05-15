using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jcr.Api.Models
{
    public class FileUploadResult
    {
        public string AzureFilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
        public bool IsFileUploaded { get; set; }

        public string Message { get; set; }

        public int TempImageID { get; set; }
    }
}