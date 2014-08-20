﻿using ServicePoll.Logic;
using ServicePoll.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ServicePoll.Models
{
    public class Item:ElementDb
    {
        private string _url;
        public Item(string url, string pollId)
        {
            Id = Util.GenerateIdBasedPollIdAndUrl(pollId, url);
            Url = url;
            PollId = pollId;
            OkRespondentIdList = new List<string>();
            MissedRespondents = new List<string>();
        }
        public string PollId { get; private set; }
        public List<string> OkRespondentIdList { get; private set; }
        public List<string> MissedRespondents { get; set; }
        public int CountResults { get; private set; }
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                if (!new Regex(@"^https?://").Match(_url).Success)
                {
                    _url = "http://" + _url;
                }
            }
        }
        public void AddOkResponse(string respondentId)
        {
            OkRespondentIdList.Add(respondentId);
            CountResults++;
        }
    }
}