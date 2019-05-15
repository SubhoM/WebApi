using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jcr.Api.Models
{
    public class TracerQuestionAnswer
    {

        public int? ObservationId { get; set; }
        public int? UserId { get; set; }
        public int? TracerQuestionId { get; set; }
        public int? TracerQuestionAnswerId { get; set; }
        public bool? IsResponseRequired { get; set; }
        public int? Numerator { get; set; }
        public int? Denominator { get; set; }
        public int? QuestionAnswer { get; set; }
        public int? QuestionNoteID { get; set; }
        public string QuestionNote { get; set; }
    }
    public class TracerQuestionAnswerGroup
    {       
        public int? TracerQuestionId { get; set; }
        public int? TracerQuestionAnswerId { get; set; }
      
    }
}