using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ServicePoll.Models
{
    public class MongoDb<T>
    {
        private MongoDatabase _db;
        public MongoDb(string connStr)
        {
            MongoUrl url = new MongoUrl(connStr);
            MongoClient cli = new MongoClient(url);
            MongoServer serv = cli.GetServer();
            _db = serv.GetDatabase(url.DatabaseName);
        }
        public MongoCollection<T> Collection
        {
            get
            {
                return _db.GetCollection<T>(typeof(T).Name);
            }
        }
        public MongoCollection<T> GetCollectionByName(string collectionName)
        {
            return _db.GetCollection<T>(collectionName);
        }
    }
}