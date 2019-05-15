namespace Jcr.Api.Models.Models
{
    public class ExceptionLogInfo
    {
        public string ExceptionText { get; set; }
        public string PageName { get; set; }
        public string MethodName { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }
        public string TransSql { get; set; }
        public string HttpReferrer { get; set; }        
    }
}
