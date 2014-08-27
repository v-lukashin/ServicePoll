using MongoDB.Driver;

namespace ServicePoll.Models
{
    public class MongoDb<T>
    {
        private readonly MongoDatabase _db;
        public MongoDb(string connStr)
        {
            var url = new MongoUrl(connStr);
            var cli = new MongoClient(url);
            var serv = cli.GetServer();
            _db = serv.GetDatabase(url.DatabaseName);
        }
        public MongoCollection<T> Collection
        {
            get
            {
                return _db.GetCollection<T>(typeof(T).Name);
            }
        }
        public MongoCollection<T> GetCollectionByName(string collectionName)
        {
            return _db.GetCollection<T>(collectionName);
        }
    }
}