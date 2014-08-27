using ServicePoll.Models.Abstract;

namespace ServicePoll.Models
{
    public class Result:ElementDb
    {
        public string ItemId { get; set; }
        public string IssueId { get; set; }
        public string AnswerId { get; set; }
        public string RespondentId { get; set; }
        public IssueType IssueType { get; set; }
    }
}