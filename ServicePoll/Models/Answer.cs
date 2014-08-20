using ServicePoll.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Models
{
    public class Answer:ElementDb
    {
        public Answer(string name, string issueId)
        {
            Name = name;
            IssueId = issueId;
        }
        public string Name { get; set; }
        public string IssueId { get; set; }
    }
}