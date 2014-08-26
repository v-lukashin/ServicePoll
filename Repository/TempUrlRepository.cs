using MongoDB.Driver;
using ServicePoll.Config;
using ServicePoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicePoll.Repository
{
    public class TempUrlRepository
    {
        private string _nameUrlField;
        private MongoCollection<TempUrl> _collect;
        public TempUrlRepository(MongoDb<TempUrl> db)
        {
            _nameUrlField = ServicePollConfig.TempFieldName;
            _collect = db.GetCollectionByName(ServicePollConfig.TempCollectionName);
        }

        public IEnumerable<string> GetShuffleUrls(int limit)
        {
            var cnt = _collect.Count();
            var offset = ServicePoll.Models.Util.ThreadSafeRandom.ThisThreadsRandom.Next((int)(cnt - limit));
            Console.WriteLine("Offset: {0}", offset);
            var res = _collect.FindAll()
                                .SetFields("PageUrl")
                                .SetSkip(offset)
                                .SetLimit(limit)
                                .Select(x => x.PageUrl);
            return res;
        }
    }
}