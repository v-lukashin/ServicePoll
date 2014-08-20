using ServicePoll.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Models
{
    public class SiteIndex : ElementDb
    {
        public string PageUrl { get; set; }
    }
}