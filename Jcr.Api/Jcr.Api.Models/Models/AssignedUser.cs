using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Tracers.Model
{
    public class AssignedUser
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsExternal { get; set; }
        public string FullName { get { return String.Concat(FirstName, " ", LastName); } }

        public string FormattedFullName { get { return String.Concat(LastName!="" ? LastName + ", " + FirstName : FirstName); } }
        public string UserRole { get; set; }
    }
}
