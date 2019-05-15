using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models
{
    public class FileResult
    {
        public byte[] FileStream { get; set; }
        public string FileName { get; set; }

        public string FileID { get; set; }
        public long FileSize { get; set; }
        public string CreateDate { get; set; }
        public string FileType { get; set; }
    }
    }
