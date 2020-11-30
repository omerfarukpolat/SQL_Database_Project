using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace UygulamaOdevi2.Models
{
    public class MongoSubmission
    {

        public object _id { get; set; } 

        public string prevSubmissionID { get; set; }
        
        public string submissionID { get; set; }
        
        public string title { get; set; }
        
        public string ozet { get; set; }

        public List<string> keywords { get; set; }

        public List<List<string>>  authors { get; set; }

        public Nullable<int> submittedBy { get; set; }

        public Nullable<int> correspondingAuthor { get; set; }

        public string pdf_path { get; set; }

        public string type { get; set; }

        public DateTime submissionDateTime { get; set; }

        public Nullable<int> status { get; set; }

        public Nullable<int> active { get; set; }
    }
}