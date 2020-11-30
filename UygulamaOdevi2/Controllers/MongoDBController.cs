using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using UygulamaOdevi2.Models;

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
                    addSubmission("211", "weq", "qwewqe", "12", new string[] { "123", "1234", "12345" },authors , "1231", "1231", "1231", "1231", DateTime.Now, 1, 1);
                    var submissions = database.GetCollection<BsonDocument>("submissions");
                    var documents = submissions.Find(new BsonDocument()).ToList();
                    foreach (BsonDocument doc in documents)
                    {
                        Debug.WriteLine(doc.ToString());
                    }
            }

        public void addSubmission(string prevSubmissionID, string submissionID, string title, string ozet, string[] keywords,string[,] authors, string submittedBy, string correspondingAuthor,
            string pdf_path, string type, DateTime sumissionDateTime, int status, int active)
    
        {
        
            
            var array = new BsonArray();
            for(int i = 0; i < authors.GetLength(0); i++)
            {
                BsonArray authorInfos = new BsonArray()
                {
                    new BsonDocument {{ "authenticationID", authors[i,0]} },
                    new BsonDocument {{ "name", authors[i,1] } },
                    new BsonDocument {{ "email", authors[i,2] } },
                    new BsonDocument {{ "affil", authors[i,3] } },
                    new BsonDocument {{ "country", authors[i,4] } }
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
                { "submission_date_time", sumissionDateTime },
                { "status", status },
                { "active", active },
            };
            submissions.InsertOne(document);
        }

        public void deleteSubmission(string submissionID) 
        {
            
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("submission_id", submissionID);

            var submissions = db.GetCollection<BsonDocument>("submissions");
            submissions.DeleteOne(deleteFilter);
        }

        public List<MongoSubmission> getSubmissions(MongoSubmission ms)
        {
            List<MongoSubmission> list = new List<MongoSubmission>();

            MongoSubmission ms = new MongoSubmission();



            return list;
        }



    }
    }