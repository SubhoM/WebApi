using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jcr.Api.Models
{
    public class UserAttribute
    {
        public int? AttributeTypeId { get; set; }
        public int? UserId { get; set; }
        public string AttributeValue { get; set; }
        public System.DateTime? AttributeActivationDate { get; set; }
        public System.DateTime? AttributeExpirationDate { get; set; }
    }

    public class SecurityQuestionAnswer
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}