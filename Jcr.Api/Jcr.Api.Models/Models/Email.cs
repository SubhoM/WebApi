using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jcr.Api.Models
{
    public class Email
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }

        public string Subject { get; set; }
        public string Comments { get; set; }

        public string Body { get; set; }

        public string Attachment { get; set; }

        public string From { get; set; }
        public string Title { get; set; }
        public bool MultipleAttachment { get; set; }
        public string ReportName { get; set; }

        public string Guid { get; set; }

        public List<string> AttachmentLocation { get; set; }
    }
}