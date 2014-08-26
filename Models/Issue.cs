using ServicePoll.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Models
{
    public class Issue:ElementDb
    {
        public Issue(string name, string pollId, IssueType type)
        {
            Name = name;
            PollId = pollId;
            Type = type;
        }
        public string Name { get; set; }
        public string PollId { get; set; }
        public IssueType Type { get; set; }
    }
}