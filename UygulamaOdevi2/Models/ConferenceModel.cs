using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UygulamaOdevi2.Models {
    public class ConferenceModel {

        public string Tags { get; set; }
        public string ConfID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Submission_Deadline { get; set; }
        public string CreatorUser { get; set; }
        public string Website { get; set; }

    }
}