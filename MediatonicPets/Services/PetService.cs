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

        private readonly List<IPetConfigurationSettings> _petSettings;

        /// <summary>The constructor <c>PetService</c> will receive setting parameters provided that 
        /// those have been configured accordingly on the Setup.cs file of the solution
        /// </summary>
        public PetService(IPetDatabaseSettings settings, IGlobalPetConfigurationSettings petSettings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _petSettings = petSettings.Metrics;
            _pets = database.GetCollection<Pet>(settings.PetCollectionName); 
            _owners = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<Pet> Get() {
            List<Pet> foundPets = _pets.Find(pet => true).ToList();
            foreach (Pet pet in foundPets)
            {
                pet.UpdateMetrics(); //return updated Metrics based on LastUpdate, no need to persist them
            }
            return foundPets;
        }

        public Pet Get(string id) {
            Pet foundPet = _pets.Find<Pet>(pet => pet.Id == id).FirstOrDefault();
            foundPet.UpdateMetrics(); //return updated Metrics based on LastUpdate, no need to persist them
            return foundPet;
        }

        public List<Pet> GetUserPets(string ownerId) {
            List<Pet> foundPets = _pets.Find<Pet>(pet => pet.OwnerID == ownerId).ToList();
            foreach (Pet pet in foundPets)
            {
                pet.UpdateMetrics(); //return updated Metrics based on LastUpdate, no need to persist them
            }
            return foundPets;        
        }
        public Pet Create(string id, string petType)
        {
            // Prevent non-existing types of Pets to be created. A null return will result in a
            // BadRequest response on the side of the Controller
            if (!Enum.GetNames(typeof(PetTypes)).Contains(petType)) {
                return null;
            }
            Pet newPet = PetFactory.GeneratePetByType(petType,_petSettings);
            // This check should not be necessary thanks to the above check, but there could be other conditions 
            // to return null on the static Method above.
            if (newPet == null) {
                return null;
            }
            newPet.OwnerID = id;
            newPet.LastUpdate = DateTime.Now;
            _pets.InsertOne(newPet);
            // Owners get their pet list updated accordingly
            var update = Builders<User>.Update.Push<string>(owner => owner.OwnedPets, newPet.Id);
            _owners.UpdateOne(user => user.Id == newPet.OwnerID, update);
            return newPet;
        }

        public void Update(string id, Pet newPet) {
            // This method could potentially be marked as private to avoid unwanted 
            // updates of Pet attributes! (and leave only Stroke and Feed as allowed)
            // But it could also allow extension of the capabilities of users if Pet attributes are extended
            newPet.UpdateMetrics();
            _pets.ReplaceOne(pet => pet.Id == id, newPet);
        }

        
        public void Stroke(string id) {
            Pet petToStroke = Get(id); 
            petToStroke.UpdateMetrics();    // Update metrics first
            petToStroke.Stroke();           // Apply Stroke after metric are updated
            Update(id, petToStroke);
        }

        public void Feed(string id) {
            Pet petToFeed = Get(id);
            petToFeed.UpdateMetrics();      // Update metrics first
            petToFeed.Feed();               // Apply Feed after metrics are updated
            Update(id, petToFeed);
        }


        public void Remove(string id) {
            string ownerID = Get(id).OwnerID;
            _pets.DeleteOne(pet => pet.Id == id);
            // Deletion of a pet also removes it from the Owners List
            var update = Builders<User>.Update.Pull(user => user.OwnedPets, id);
            _owners.UpdateOne(user => user.Id == ownerID ,update);
        }
    }
}