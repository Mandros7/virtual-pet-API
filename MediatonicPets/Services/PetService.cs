using MediatonicPets.Models;
using MediatonicPets.Factories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicPets.Services
{
    /// <summary>
    /// Class <c>PetService</c>  makes uses of Models, Factories and MongoDB drivers to 
    /// act as a bridge between the received controller actions and the persistence MongoDB back-end.
    /// </summary>
    public class PetService
    {
        private readonly IMongoCollection<Pet> _pets;
        private readonly IMongoCollection<User> _owners;
        private readonly PetFactory _petFactory;



        /// <summary>The constructor <c>PetService</c> will receive setting parameters provided that 
        /// those have been configured accordingly on the Setup.cs file of the solution
        /// </summary>
        public PetService(IPetDatabaseSettings settings, List<PetConfigurationSettings> petSettings)
        {
            string detectedHost = Environment.GetEnvironmentVariable("MONGODB_HOST");
            if (detectedHost == null) {
                detectedHost = settings.ConnectionString;
            }
            var client = new MongoClient(detectedHost);
            var database = client.GetDatabase(settings.DatabaseName);

            _petFactory = new PetFactory(petSettings);
            _pets = database.GetCollection<Pet>(settings.PetCollectionName); 
            _owners = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<Pet> Get() {
            List<Pet> foundPets = _pets.FindSync(pet => true).ToList();
            foundPets.ForEach(pet => pet.UpdateMetrics());
            return foundPets;
        }

        public Pet Get(string id) {
            Pet foundPet = _pets.Find<Pet>(pet => pet.Id.Equals(id)).FirstOrDefault();
            foundPet.UpdateMetrics(); //return updated Metrics based on LastUpdate, no need to persist them
            return foundPet;
        }

        public List<Pet> GetUserPets(string ownerId) {
            var foundPets = _pets.Find<Pet>(pet => pet.OwnerID.Equals(ownerId)).ToList();
            foundPets.ForEach(pet => pet.UpdateMetrics());
            return foundPets;
        }
        public Pet Create(string id, string petType)
        {
            // Prevent non-existing types of Pets to be created. A null return will result in a
            // BadRequest response on the side of the Controller        
            Pet newPet = _petFactory.GetPet(petType);        
            if (newPet == null) {
                return null;
            }
            if (_owners.CountDocuments(user => user.Id.Equals(id)) == 0){
                return null;
            }
            newPet.OwnerID = id;
            newPet.LastUpdate = DateTime.UtcNow;
            _pets.InsertOne(newPet);
            // Owners get their pet list updated accordingly
            var update = Builders<User>.Update.Push<string>(owner => owner.OwnedPets, newPet.Id);
            _owners.UpdateOne(user => user.Id.Equals(newPet.OwnerID), update);
            return newPet;
        }

        public void Update(string id, Pet newPet) {
            // This method could potentially be marked as private to avoid unwanted 
            // updates of Pet attributes! (and leave only Stroke and Feed as allowed)
            // But it could also allow extension of the capabilities of users if Pet attributes are extended
            newPet.UpdateMetrics();
            _pets.ReplaceOneAsync(pet => pet.Id.Equals(id), newPet);
        }

        
        public Pet Stroke(string id) {
            Pet petToStroke = _pets.Find<Pet>(pet => pet.Id.Equals(id)).FirstOrDefault(); 
            petToStroke.UpdateMetrics();    // Update metrics first
            petToStroke.Stroke();           // Apply Stroke after metric are updated
            _pets.ReplaceOneAsync(pet => pet.Id.Equals(id), petToStroke);
            return petToStroke;
        }

        public Pet Feed(string id) {
            Pet petToFeed = _pets.Find<Pet>(pet => pet.Id.Equals(id)).FirstOrDefault(); 
            petToFeed.UpdateMetrics();      // Update metrics first
            petToFeed.Feed();               // Apply Feed after metrics are updated
            _pets.ReplaceOne(pet => pet.Id.Equals(id), petToFeed);
            return petToFeed;
        }


        public void Remove(string id) {
            string ownerID = Get(id).OwnerID;
            _pets.DeleteOne(pet => pet.Id.Equals(id));
            // Deletion of a pet also removes it from the Owners List
            var update = Builders<User>.Update.Pull(user => user.OwnedPets, id);
            _owners.UpdateOne(user => user.Id.Equals(ownerID) ,update);
        }
    }
}
