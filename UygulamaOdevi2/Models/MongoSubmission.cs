using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;

namespace UygulamaOdevi2.Models
{
    [Serializable]
    public class MongoSubmission
    {
        [BsonId]
        public object _id { get; set; }

        [BsonElement("prev_submission_id")]
        public string prevSubmissionID { get; set; }

        [BsonElement("submission_id")]
        public string submissionID { get; set; }

        [BsonElement("title")]
        public string title { get; set; }

        [BsonElement("abstract")]
        public string ozet { get; set; }

        [BsonElement("keywords")]
        public string keywords { get; set; }

        [BsonElement("authors")]
        public string authors { get; set; }

        [BsonElement("submitted_by")]
        public string submittedBy { get; set; }

        [BsonElement("corresponding_author")]
        public string correspondingAuthor { get; set; }

        [BsonElement("pdf_path")]
        public string pdf_path { get; set; }

        [BsonElement("type")]
        public string type { get; set; }

        [BsonElement("submission_date_time")]
        public string submissionDateTime { get; set; }

        [BsonElement("status")]
        public Nullable<int> status { get; set; }

        [BsonElement("active")]
        public Nullable<int> active { get; set; }
    }
}