using ServicePoll.Models.Abstract;

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