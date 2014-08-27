using MongoDB.Driver.Builders;
using ServicePoll.Models;
using System.Collections.Generic;
using System.Linq;

namespace ServicePoll.Repository
{
    public class ItemRepository : RepositoryGeneric<Item>
    {
        public ItemRepository(MongoDb<Item> db) : base(db) { }

        public Item GetByPollIdAndUrl(string pollId, string url)
        {
            //Ищем по сгенерированному ID
            var id = Util.GenerateIdBasedPollIdAndUrl(pollId, url);
            var result = Collect.FindOneById(id);
            //Если Item не найден пробуем найти через условие 
            if (result == null)
            {
                var q = Query.And(
                    Query<Item>.EQ(x => x.PollId, pollId),
                    Query<Item>.EQ(x => x.Url, url)
                    );
                result = Collect.FindOne(q);
            }
            return result;
        }

        public IEnumerable<string> GetNextUrl(string pollId, int limit, string respondentId, int countTake)
        {
            var q = Query.And(
                Query<Item>.EQ(x => x.PollId, pollId),
                Query<Item>.LT(x => x.CountResults, limit),
                Query<Item>.NE(x => x.OkRespondentIdList, respondentId),
                Query<Item>.NE(x => x.MissedRespondents, respondentId)
                );
            var result = Collect.Find(q).SetSortOrder("CountResults:-1").SetLimit(countTake).Select(x => x.Url);
            return result;
        }

        public bool CheckFormalEnd(string pollId, int limit)
        {
            var q = Query.And(
                Query<Item>.EQ(x => x.PollId, pollId),
                Query<Item>.LT(x => x.CountResults, limit)
                );
            var res = !Collect.Find(q).Any();
            return res;
        }
        public void RemoveItemsByPoll(string pollId)
        {
            var q = Query<Item>.EQ(x => x.PollId, pollId);
            Collect.Remove(q);
        }
    }
}