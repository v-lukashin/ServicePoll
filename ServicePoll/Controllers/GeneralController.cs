﻿using MongoDB.Bson;
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
        private readonly int _countTake;
        private readonly RepositoryGeneric<Poll> _pollRepository;
        private readonly ItemRepository _itemRepository;
        private readonly IssueRepository _issueRepository;
        private readonly RepositoryGeneric<Result> _resultRepository;
        private readonly AnswerRepository _answerRepository;

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
            var issueIds = issueId.Split(',');
            var answerIds = answerId.Split(',');
            if (issueIds.Length != answerIds.Length)
            {
                return BadRequest("Количество ответов не совпадает с количеством вопросов");
            }
            Item item;
            var respId = GetRespondentId();
            try
            {
                item = _itemRepository.GetByPollIdAndUrl(pollId, url);
            }
            catch (Exception e) { return BadRequest(e.Message); }

            for (var i = 0; i < issueIds.Length; i++)
            {
                var res = new Result
                {
                    AnswerId = answerIds[i],
                    IssueId = issueIds[i],
                    IssueType = _issueRepository.Get(issueIds[i]).Type,
                    RespondentId = respId,
                    ItemId = item.Id
                };
                _resultRepository.Create(res);    
            }
            item.AddOkResponse(respId);
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
            var issues = _issueRepository.GetByPollId(pId);//пока что один, потом много 

            var res = issues.Select(issue => new
            {
                Issue = new { issue.Id, issue.Name},
                Answers = _answerRepository.GetByIssueId(issue.Id).Select(x => new { x.Id, x.Name })
            });

            return Ok<object>(res);
        }

        [NonAction]
        private static string GetRespondentId()
        {
            var res = HttpContext.Current.GetCookie("_id");
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
            var respondentId = GetRespondentId();
            var poll = _pollRepository.Get(pollId);
            var limit = poll.LimitRespondents;

            var urls = _itemRepository.GetNextUrl(pollId, limit, respondentId, _countTake).Shuffle();

            var result = urls.Any() ? urls.First() : null;

            if (result == null && _itemRepository.CheckFormalEnd(pollId, limit))
            {
                poll.IsActive = false;
                _pollRepository.Update(poll.Id, poll);
            }
            return result;
        }
        //        [NonAction]
        //        public void RemovePoll(string pollId)//не удаляет результаты
        //        {
        //            _pollRepository.Remove(pollId);
        //            _itemRepository.RemoveItemsByPoll(pollId);
        //            var issueId = _issueRepository.GetByPollId(pollId).Id;
        //            _issueRepository.Remove(issueId);
        //            var answerIdList = _answerRepository.GetByIssueId(issueId).Select(x => x.Id);
        //            foreach (var id in answerIdList)
        //            {
        //                _answerRepository.Remove(id);
        //            }
        //        }
    }
}