using CrudApp.Models;
using CrudApp.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CrudApp.Data
{
    public class MongoThingsContext
    {
        private readonly IMongoDatabase _database;
         
        public MongoThingsContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<MongoThings> MongoThings => _database.GetCollection<MongoThings>("MongoThings");
    }
}
