using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Marcia.Bot.Domain.Models;

public class BotResponse
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserRequestId { get; set; }
    public string Response { get; set; }
    public DateTime Timestamp { get; set; }

    [BsonIgnore]
    public UserRequest UserRequest { get; set; }
}
