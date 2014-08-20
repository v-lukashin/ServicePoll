using ServicePoll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using ServicePoll.Models.Abstract;

namespace ServicePoll.Repository.Mongo
{
    public class Repository<T> : IRep<T> where T : ElementDb
    {
        protected MongoCollection<T> _collect;
        protected MongoDb<T> _db;
        public Repository(MongoDb<T> db)
        {
            _collect = db.Collection;
            _db = db;
        }
        public T Get(string id)
        {
            return _collect.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _collect.FindAll();
        }

        public T Create(T value)
        {
            _collect.Insert<T>(value);
            return value;
        }

        public void Update(string id, T value)
        {
            _collect.Save(value);
        }

        public void Remove(string id)
        {
            var q = Query<T>.EQ(x => x.Id, id);
            _collect.Remove(q);
        }
    }
}