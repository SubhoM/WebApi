using System.Collections.Generic;

namespace Jcr.Api.Models.Models {
    public class SystemTracerInfo {
        public string DialogTitle { get; set; }
        public SystemTracer Master{ get; set; }

        public List<SystemTracer> Tracers = new List<SystemTracer>();

        public SystemTracerInfo() {
            DialogTitle = "";
            Master = new SystemTracer();
        }
    }

    public class SystemTracer {
        public bool IsLocked { get; set; }
        public bool IsMaster { get; set; }
        public int TracerCustomID { get; set; }
        public string TracerCustomName { get; set; }
        public int SiteID { get; set; }
        public string HCOID { get; set; }
        public string SiteName { get; set; }
        public string HasAccess { get; set; }
        public string TracerStatus { get; set; }
        public int NumOfObservations { get; set; }
    }
}
