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
        private readonly MongoCollection<TempUrl> _collect;
        public TempUrlRepository(MongoDb<TempUrl> db)
        {
            _collect = db.GetCollectionByName(ServicePollConfig.TempCollectionName);
        }

        public IEnumerable<string> GetShuffleUrls(int limit)
        {
            var cnt = _collect.Count();
            var maxVal = (int)(cnt - limit);

            string fieldName;
            Func<TempUrl, string> expr;
            Util.GetFieldNameAndExpression(ServicePollConfig.TempFieldName, out fieldName, out expr);

            var offset = maxVal > 0 ? Util.ThreadSafeRandom.ThisThreadsRandom.Next(maxVal) : 0;
            Console.WriteLine("Offset: {0}", offset);
            var res = _collect.FindAll()
                                .SetFields(fieldName)
                                .SetSkip(offset)
                                .SetLimit(limit)
                                .Select(expr);
            res = res.Shuffle();
            return res;
        }
    }
}