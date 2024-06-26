﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace login.Model;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.Int64)]
    public long Id { get; set; }
    [BsonRequired]
    public string Name { get; set; } = null!;
    [BsonRequired]
    public string Password { get; set; } = null!;
    [BsonRequired]
    public string Email { get; set; } = null!;
}