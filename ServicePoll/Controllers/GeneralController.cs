﻿using ServicePoll.Logic;
using ServicePoll.Models;
using ServicePoll.Repository;
using ServicePoll.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using MongoDB.Bson;
using ServicePoll.Repository.Mongo;

namespace ServicePoll.Controllers
{
    [RoutePrefix("api")]
    public class GeneralController : ApiController
    {
        private const int _countTake = 2;
        private IRep<Poll> _pollRepository;
        private ItemRepository _itemRepository;
        private IssueRepository _issueRepository;
        private IRep<Result> _resultRepository;
        private AnswerRepository _answerRepository;

        public GeneralController(IRep<Poll> pollRep, ItemRepository itemRep, IssueRepository issueRep, IRep<Result> resRep, AnswerRepository ansRep)
        {
            _pollRepository = pollRep;
            _itemRepository = itemRep;
            _issueRepository = issueRep;
            _resultRepository = resRep;
            _answerRepository = ansRep;
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