using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.Models
{
   public class SelectOption
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string FullText { get; set; }
		public bool IsDefault { get; set; }
    }
}
