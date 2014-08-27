using ServicePoll.Models;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using ServicePoll.Models.Abstract;

namespace ServicePoll.Repository
{
    public class RepositoryGeneric<T> where T : ElementDb
    {
        protected MongoCollection<T> Collect;
        protected MongoDb<T> Db;
        public RepositoryGeneric(MongoDb<T> db)
        {
            Collect = db.Collection;
            Db = db;
        }
        public T Get(string id)
        {
            return Collect.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Collect.FindAll();
        }

        public T Create(T value)
        {
            Collect.Insert<T>(value);
            return value;
        }

        public void Update(string id, T value)
        {
            Collect.Save(value);
        }

        public void Remove(string id)
        {
            var q = Query<T>.EQ(x => x.Id, id);
            Collect.Remove(q);
        }
    }
}