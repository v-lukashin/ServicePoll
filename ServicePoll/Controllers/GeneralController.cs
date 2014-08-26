using MongoDB.Bson;
using ServicePoll.Config;
using ServicePoll.Models;
using ServicePoll.Repository;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ServicePoll.Controllers
{
    [RoutePrefix("api")]
    public class GeneralController : ApiController
    {
        private int _countTake;
        private RepositoryGeneric<Poll> _pollRepository;
        private ItemRepository _itemRepository;
        private IssueRepository _issueRepository;
        private RepositoryGeneric<Result> _resultRepository;
        private AnswerRepository _answerRepository;

        public GeneralController(RepositoryGeneric<Poll> pollRep, ItemRepository itemRep, IssueRepository issueRep, RepositoryGeneric<Result> resRep, AnswerRepository ansRep)
        {
            _pollRepository = pollRep;
            _itemRepository = itemRep;
            _issueRepository = issueRep;
            _resultRepository = resRep;
            _answerRepository = ansRep;
            _countTake = ServicePollConfig.CountTake;
        }

        [Route("/{pollId}/ok")]
        [HttpGet]
        public IHttpActionResult Ok(string pollId, string url, string issueId, string answerId)
        {
            Item item = null;
            var respId = GetRespondentId();
            try
            {
                item = _itemRepository.GetByPollIdAndUrl(pollId, url);
            }
            catch (Exception e) { return BadRequest(e.Message); }

            Result res = new Result
            {
                AnswerId = answerId,
                IssueId = issueId,
                IssueType = _issueRepository.Get(issueId).Type,
                RespondentId = respId,
                ItemId = item.Id
            };
            item.AddOkResponse(respId);

            _resultRepository.Create(res);
            _itemRepository.Update(item.Id, item);

            return Ok<string>(NextUrl(pollId));
        }

        [Route("{pollId}/next")]
        [HttpGet]
        public IHttpActionResult Next(string pollId)
        {
            return Ok<string>(NextUrl(pollId));
        }

        [Route("{pollId}/skip")]
        [HttpGet]
        public IHttpActionResult Skip(string pollId, string url)
        {
            var respId = GetRespondentId();
            var item = _itemRepository.GetByPollIdAndUrl(pollId, url);
            item.MissedRespondents.Add(respId);
            _itemRepository.Update(item.Id, item);
            return Ok<string>(NextUrl(pollId));
        }

        [HttpGet]
        [Route("{pId}/issue")]
        public IHttpActionResult Issue(string pId)//pollId
        {
            var issue = _issueRepository.GetByPollId(pId);//пока что один, потом много 
            var answers = _answerRepository.GetByIssueId(issue.Id).Select(x => new { x.Id, x.Name });
            var res = new { Issue = issue, Answers = answers };
            return Ok<object>(res);
        }

        [NonAction]
        private string GetRespondentId()
        {
            string res = HttpContext.Current.GetCookie("_id");
            if (string.IsNullOrEmpty(res))
            {
                res = ObjectId.GenerateNewId().ToString();
                HttpContext.Current.SetCookie("_id", res);
            }
            return res;
        }

        /// <summary>
        /// Следующий url. Если вернул null, значит для данного респондента нет больше урлов
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private string NextUrl(string pollId)//Получается сложный запрос
        {
            string respondentId = GetRespondentId();
            var poll = _pollRepository.Get(pollId);
            int limit = poll.LimitRespondents;

            var urls = _itemRepository.GetNextUrl(pollId, limit, respondentId, _countTake).Shuffle();

            var result = urls.Any() ? urls.First() : null;

            if (result == null && _itemRepository.CheckFormalEnd(pollId, limit))
            {
                poll.IsActive = false;
                _pollRepository.Update(poll.Id, poll);
            }
            return result;
        }
        [NonAction]
        public void RemovePoll(string pollId)//не удаляет результаты
        {
            _pollRepository.Remove(pollId);
            _itemRepository.RemoveItemsByPoll(pollId);
            var issueId = _issueRepository.GetByPollId(pollId).Id;
            _issueRepository.Remove(issueId);
            var answerIdList = _answerRepository.GetByIssueId(issueId).Select(x => x.Id);
            foreach (var id in answerIdList)
            {
                _answerRepository.Remove(id);
            }
        }
    }
}