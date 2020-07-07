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

        public float HAPPINESS_MAX = 100.0f;
        public float HAPPINESS_MIN = 0.0f;
        public float HUNGRINESS_MAX = 100.0f;
        public float HUNGRINESS_MIN = 0.0f;

        public void UpdateMetrics () {
            TimeSpan elapsed = DateTime.Now - this.LastUpdate;
            float newHappiness = this.Happiness + this.HappinessRate * elapsed.Minutes;
            this.Happiness = (newHappiness < HAPPINESS_MIN) ? HAPPINESS_MIN : newHappiness;
            float newHungriness = this.Hungriness + this.HungrinessRate * elapsed.Minutes;
            this.Hungriness = (newHungriness > HUNGRINESS_MAX) ? HUNGRINESS_MAX : newHungriness;
            this.LastUpdate = DateTime.Now;
        }

        public void Stroke () {
            float newHappiness = this.Happiness + this.StrokeHapiness;
            this.Happiness = (newHappiness > HAPPINESS_MAX) ? HAPPINESS_MAX : newHappiness;
        }
        public void Feed () {
            float newHungriness = this.Hungriness + this.FeedHungriness;
            this.Hungriness = (newHungriness < HUNGRINESS_MIN) ? HUNGRINESS_MIN : newHungriness;
        }
        

    }
}
