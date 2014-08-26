using MongoDB.Driver.Builders;
using ServicePoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Repository
{
    public class AnswerRepository : RepositoryGeneric<Answer>
    {
        public AnswerRepository(MongoDb<Answer> db) : base(db) { }

        public IEnumerable<Answer> GetByIssueId(string issueId)
        {
            var q = Query<Answer>.EQ(x => x.IssueId, issueId);
            var result = _collect.Find(q);
            return result;
        }
    }
}