using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using ServicePoll.Logic;
using MongoDB.Driver.Builders;

namespace ServicePoll.Models
{
    public class MongoDb<T>
    {
        private MongoDatabase _db;
        private const string connectionString = "mongodb://localhost:27017/polls";
        public MongoDb(string connStr = connectionString)
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
        
        public IEnumerable<string> GetShuffleAllUrlsFromSiteIndex(int count)
        {
            var collect = _db.GetCollection("SiteIndex");
            var cnt = collect.Count();
            var offset = ServicePoll.Logic.Util.ThreadSafeRandom.ThisThreadsRandom.Next((int)(cnt - count));
            var tmp = collect.FindAll().SetFields("PageUrl");
            tmp.Skip = offset;
            tmp.Limit = count;
            var res = tmp.Select(x => x["PageUrl"].AsString);
            return res;
        }
    }
}