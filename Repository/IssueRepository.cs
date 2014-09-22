using MongoDB.Driver.Builders;
using ServicePoll.Models;
using System.Collections.Generic;

namespace ServicePoll.Repository
{
    public class IssueRepository : RepositoryGeneric<Issue>
    {
        public IssueRepository(MongoDb<Issue> db) : base(db) { }

        public IEnumerable<Issue> GetByPollId(string pollId)
        {
            var q = Query<Issue>.EQ(x => x.PollId, pollId);
            var result = Collect.Find(q);
            return result;
        }
    }
}