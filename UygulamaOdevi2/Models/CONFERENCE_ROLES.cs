using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UygulamaOdevi2.Models
{
    public class CONFERENCE_ROLES
    {
        public string ConfName { get; set; }
        public Nullable<int> ConfID_ROLE { get; set; }
        public string AuthenticationID { get; set; }
        public virtual USERS USERS { get; set; }
    }
}