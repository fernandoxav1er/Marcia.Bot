using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Marcia.Bot.Domain.Models;

public class UserRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}
