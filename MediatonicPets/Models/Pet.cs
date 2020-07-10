using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MediatonicPets.Models
{
    /// <summary>
    /// Enum <c>PetTypes</c> is used to define existing types of pets, and to avoid creation of unexisting types
    /// </summary>
    public enum PetTypes {
        dog = 0,
        cat = 1,
        fish = 2
    }

    /// <summary>
    /// Class <c>Pet</c> contains the generic model for a Pet, and its common methods
    /// This class could be abstracted and let more concrete types of pet inherit from it
    /// </summary>
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

        public float StrokeHappiness { get; set;}

        public float FeedHungriness { get; set;}

        public float HAPPINESS_MAX = 100.0f;
        public float HAPPINESS_MIN = 0.0f;
        public float HUNGRINESS_MAX = 100.0f;
        public float HUNGRINESS_MIN = 0.0f;


        /// <summary>
        /// Method <c>UpdateMetrics</c> updates the model based on the time elapsed since the last time it was persisted.
        /// *Metric rates are considered as per minute*
        /// This method removes the necessity to periodically update the metrics and directly ties processing costs 
        /// to the actual usage of the API, with unused pets only using storage on the DB.
        /// </summary>
        public void UpdateMetrics () {
            TimeSpan elapsed = DateTime.UtcNow - this.LastUpdate; //Detect time since last update
            float newHappiness = this.Happiness + this.HappinessRate * (float)elapsed.TotalMinutes;
            this.Happiness = (newHappiness < HAPPINESS_MIN) ? HAPPINESS_MIN : newHappiness;
            float newHungriness = this.Hungriness + this.HungrinessRate * (float)elapsed.TotalMinutes;
            this.Hungriness = (newHungriness > HUNGRINESS_MAX) ? HUNGRINESS_MAX : newHungriness;
            this.LastUpdate = DateTime.UtcNow;
        }
        /// <summary>
        /// Method <c>Stroke</c> applies the StrokeHapiness modifier (which should have a positive value).
        /// It also ensures hapiness is never out of bounds!
        /// </summary>
        public void Stroke () {
            float newHappiness = this.Happiness + this.StrokeHappiness;
            this.Happiness = (newHappiness > HAPPINESS_MAX) ? HAPPINESS_MAX : newHappiness;
        }
        /// <summary>
        /// Method <c>Feed</c> applies the FeedHungriness modifier (which should have a negative value).
        /// It also ensures hungriness is never out of bounds
        /// </summary>
        public void Feed () {
            float newHungriness = this.Hungriness + this.FeedHungriness;
            this.Hungriness = (newHungriness < HUNGRINESS_MIN) ? HUNGRINESS_MIN : newHungriness;
        }
        

    }
}
