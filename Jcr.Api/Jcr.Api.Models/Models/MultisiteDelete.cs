using System;

namespace Jcr.Api.Models.Models {
    public class MultisiteDelete {
        public string SelectedSites { get; set; }
        public int TracerID { get; set; }
        public int UserID { get; set; }
    }
}