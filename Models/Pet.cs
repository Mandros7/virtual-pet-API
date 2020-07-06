using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MediatonicPets.Models
{
    public enum PetTypes {
        dog = 0,
        cat = 1,
        turtle = 2,
        fish = 3
    }
    public class Pet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set;}
        
        [BsonElement("LastUpdate")]
        public DateTime LastUpdate { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("OwnerID")]
        public string OwnerID {get; set;}

        [BsonElement("Happiness")]
        public float Happiness { get; set; }

        [BsonElement("Hungriness")]
        public float Hungriness { get; set; }

        public float HappinessRate { get; set;}

        public float HungrinessRate { get; set;}

        public float StrokeHapiness { get; set;}

        public float FeedHungriness { get; set;}

    }
}
