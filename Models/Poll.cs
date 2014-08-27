using ServicePoll.Models.Abstract;

namespace ServicePoll.Models
{
    public class Poll : ElementDb
    {
        public Poll(string name, int limit, bool isActive = false)
        {
            Name = name;
            LimitRespondents = limit;
            IsActive = isActive;
        }
        public string Name { get; private set; }
        public int LimitRespondents { get; private set; }
        public bool IsActive { get; set; }
    }
}
