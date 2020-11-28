using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UygulamaOdevi2.Models {
    public class UserModel {

        public string Username { get; set; }
        public string Password { get; set; }
        public string Salutation { get; set; }
        public int AuthenticationID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Affiliation { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string URL { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public string Country { get; set; }
        public string RecordCreationDate { get; set; }

    }
}