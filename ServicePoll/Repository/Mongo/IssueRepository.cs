using MongoDB.Driver.Builders;
using ServicePoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Repository.Mongo
{
    public class IssueRepository : Repository<Issue>
    {
        public IssueRepository(MongoDb<Issue> db) : base(db) { }

        public Issue GetByPollId(string pollId)
        {
            var q = Query<Issue>.EQ(x => x.PollId, pollId);
            var result = _collect.FindOne(q);
            return result;
        }
    }
}