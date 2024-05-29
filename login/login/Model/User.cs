using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginJwt.Model;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }
    [BsonRequired]
    public string Name { get; set; } = null!;
    [BsonRequired]
    public string Password { get; set; } = null!;
}