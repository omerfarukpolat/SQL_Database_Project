using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UygulamaOdevi2.Controllers
{
    public class MongoDBController
    {
            private IMongoDatabase db;

            public MongoDBController(string database)
            {
                var client = new MongoClient();
                db = client.GetDatabase(database);
                

            }
    }
}