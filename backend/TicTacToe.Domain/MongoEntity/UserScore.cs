using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicTacToe.Domain.MongoEntity;

public class UserScore
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = default!;
    
    public int Score { get; set; }
    
    public Guid UserIdPostgres { get; set; } 
}