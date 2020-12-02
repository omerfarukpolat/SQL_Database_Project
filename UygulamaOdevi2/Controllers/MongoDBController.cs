using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using UygulamaOdevi2.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using MongoDB.Bson.Serialization;

namespace UygulamaOdevi2.Controllers
{
    public class MongoDBController
    {
        private IMongoDatabase db;

        MongoClient client = new MongoClient("mongodb://127.0.0.1:27017/konferans?compressors=disabled&gssapiServiceName=mongodb");

        public MongoDBController(string d)
        {
            db = client.GetDatabase("KONFERANS");
            string[,] authors =
            {
                            { "1","Richar","54","af","cou" },
                            { "2","vxc","5434","af","cou" },
                            { "","ehe","223","af","t54y" },
                            { "","fds","datjr54yrgkpfjs","32","23" },
                            { "","sdfgf","54y45","af","hg" },
                        };
            var database = client.GetDatabase("KONFERANS");
            //  addSubmission("211", "weq", "qwewqe", "12", new string[] { "123", "1234", "12345" }, authors, "1231", "1231", "1231", "1231", DateTime.Now, 1, 1);
            var submissions = db.GetCollection<BsonDocument>("submissions");
            getSubmissions();

        }

        public void addSubmission(string submissionID, string title, string ozet, List<string> keywords, List<List<string>> authors, string submittedBy, string correspondingAuthor,
            string pdf_path, string type, DateTime submissionDateTime, int status, int active)

        {
            int prevSubmissionID = 0;
            string prevConfID = "";
            RDBMSController rdbms = new RDBMSController("s");
            List<SUBMISSIONS> list = rdbms.getSubmissions();
            if (list.Count == 0)
            {
                prevSubmissionID = 0;
            }
            else
            {
                prevSubmissionID = list[list.Count - 1].SubmissionID;
                prevConfID = list[list.Count - 1].ConfID;
            }
            var array = new BsonArray();
            for (int i = 0; i < authors.Count; i++)
            {
                BsonArray authorInfos = new BsonArray()
                    {
                        new BsonDocument {{ "authenticationID", authors[i][0]} },
                        new BsonDocument {{ "name", authors[i][1] } },
                        new BsonDocument {{ "email", authors[i][2] } },
                        new BsonDocument {{ "affil", authors[i][3] } },
                        new BsonDocument {{ "country", authors[i][4] } }
                    };
                array.Add(authorInfos);
            }

            var submissions = db.GetCollection<BsonDocument>("submissions");
            var document = new BsonDocument { { "prev_submission_id", prevSubmissionID },
                    { "submission_id", submissionID },
                    { "title", title},
                    { "abstract", ozet },
                    {"keywords", new BsonArray {
                       new BsonDocument {{"first", keywords[0]} },
                       new BsonDocument {{"second", keywords[1] } },
                        new BsonDocument {{"third", keywords[2] } }
                        }
                    },
                    {"authors", array},
                    { "submitted_by", submittedBy },
                    { "corresponding_author", correspondingAuthor },
                    { "pdf_path", pdf_path },
                    { "type", type },
                    { "submission_date_time", submissionDateTime },
                    { "status", status },
                    { "active", active },
                };
            submissions.InsertOne(document);

            List<USERS> users = rdbms.getUsers();
            int userID = 0;
            foreach (USERS u in users)
            {
                if (u.Username.Equals(submittedBy))
                {
                    userID = u.AuthenticationID;
                    break;
                }
            }
            //userID = 0 ise admin tarafından insert edilmiştir.
            if (list.Count > 0)
            {
                rdbms.insertSubmission(userID, submissionID, list[list.Count - 1].SubmissionID);
            }
            else
            {
                rdbms.insertSubmission(userID, submissionID, 0);
            }
        }


        public void deleteSubmission(string submissionID, string submittedBy)
        {
            //submittedBy = login olan kullanıcının kendi username'i. Sadece kendi eklediği submissionları silebilir.
            // veya eğer log in olan kullanıcı adminse istediği submissionları silebilir. 
            var submissions = db.GetCollection<BsonDocument>("submissions");
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("submission_id", submissionID);
            deleteFilter = deleteFilter & (Builders<BsonDocument>.Filter.Eq("submitted_by", submittedBy));
            submissions.DeleteOne(deleteFilter);
        }

        public List<MongoSubmission> getSubmissions()
        {
            List<MongoSubmission> list = new List<MongoSubmission>();
            var submissions = db.GetCollection<BsonDocument>("submissions");

            var documents = submissions.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {

                MongoSubmission ms = new MongoSubmission();
                ms.prevSubmissionID = doc.GetElement(1).Value.ToString();
                ms.submissionID = doc.GetElement(2).Value.ToString();
                ms.title = doc.GetElement(3).Value.ToString();
                ms.ozet = doc.GetElement(4).Value.ToString();
                ms.keywords = doc.GetElement(5).Value.ToString();
                ms.authors = doc.GetElement(6).Value.ToString();
                ms.submittedBy = doc.GetElement(7).Value.ToString();
                ms.correspondingAuthor = doc.GetElement(8).Value.ToString();
                ms.pdf_path = doc.GetElement(9).Value.ToString();
                ms.type = doc.GetElement(10).Value.ToString();
                ms.submissionDateTime = doc.GetElement(11).Value.ToString();
                ms.status = doc.GetElement(12).Value.ToInt32();
                ms.active = doc.GetElement(13).Value.ToInt32();
            }
            return list;
        }
        public void updateSubmissions(string submissionID, string title, string ozet, string[] keywords,
            string[,] authors, string submittedBy, string correspondingAuthor,
         string pdf_path, string type, DateTime submissionDateTime, int active)
        {
            int prevSubmissionID = 0;
            string prevConfID = "";
            RDBMSController rdbms = new RDBMSController("s");
            List<SUBMISSIONS> list = rdbms.getSubmissions();
            foreach (SUBMISSIONS item in list)
            {
                if (item.ConfID == submissionID)
                {
                    item.prevSubmissionID = prevSubmissionID;
                    break;
                }
            }
            var submissions = db.GetCollection<BsonDocument>("submissions");
            var filter = Builders<BsonDocument>.Filter.Eq("submission_id", submissionID);
            filter = filter & Builders<BsonDocument>.Filter.Eq("prev_submission_id", prevSubmissionID);
            var update = Builders<BsonDocument>.Update.Set("title", title)
                                                       .Set("abstract", ozet)
                                                       .Set("keywords", keywords)
                                                       .Set("authors", authors)
                                                       .Set("submitted_by", submittedBy)
                                                       .Set("corresponding_author", correspondingAuthor)
                                                       .Set("pdf_path", pdf_path)
                                                       .Set("type", type)
                                                       .Set("submission_date_time", submissionDateTime)
                                                       .Set("status", 0)
                                                       .Set("active", active);
            submissions.UpdateOne(filter, update);
        }
    }
}