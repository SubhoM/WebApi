using System;

namespace Jcr.Api.Models.Models {
    public class MultisiteCopy {
        public string SelectedSites { get; set; }
        public int TracerID         { get; set; }
        public bool IsLocked        { get; set; }
        public int UserID           { get; set; }
    }
}
