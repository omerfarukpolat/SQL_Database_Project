using MongoDB.Bson;
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
            MongoClient dbClient = new MongoClient(database);

            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
        }


        }
    }