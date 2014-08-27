using MongoDB.Driver.Builders;
using ServicePoll.Models;

namespace ServicePoll.Repository
{
    public class IssueRepository : RepositoryGeneric<Issue>
    {
        public IssueRepository(MongoDb<Issue> db) : base(db) { }

        public Issue GetByPollId(string pollId)
        {
            var q = Query<Issue>.EQ(x => x.PollId, pollId);
            var result = Collect.FindOne(q);
            return result;
        }
    }
}