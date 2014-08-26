using ServicePoll.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Models
{
    public class TempUrl : ElementDb
    {
        public string PageUrl { get; set; }
        public string Url { get; set; }
    }
}