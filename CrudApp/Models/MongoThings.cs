using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrudApp.Models
{
    public class MongoThings
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
