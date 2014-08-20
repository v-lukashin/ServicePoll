using MongoDB.Bson;
using ServicePoll.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePoll.Models.Abstract
{
    public abstract class ElementDb
    {
        private string _id;
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id)) _id = ObjectId.GenerateNewId().ToString(); 
                return _id;
            }
            set { _id = value; }
        }
    }
}