using ServicePoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Repository.Mongo
{
    public class SiteIndexRepository : Repository<SiteIndex>
    {
        private const string _connectionString = @"mongodb://localhost:27017/index";
        public SiteIndexRepository() : base(new MongoDb<SiteIndex>(_connectionString)) { }

        public IEnumerable<string> GetShuffleUrls(int count)
        {
            return _db.GetShuffleAllUrlsFromSiteIndex(count);
        }
    }
}