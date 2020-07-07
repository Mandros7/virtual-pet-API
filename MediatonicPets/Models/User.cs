using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MediatonicPets.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set;}

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> OwnedPets { get; set;}
    }
}