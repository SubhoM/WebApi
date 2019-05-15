using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Api.Models.Models
{
	public class TracerCategory
	{
		public int TracerCategoryID { get; set; }
		public string TracerCategoryName { get; set; }
		public int SiteID { get; set; }
		public int UpdatedByID { get; set; }
	}
}
