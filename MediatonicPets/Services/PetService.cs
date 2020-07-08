using MediatonicPets.Models;
using MediatonicPets.Factories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicPets.Services
{
    public class PetService
    {
        private readonly IMongoCollection<Pet> _pets;
        private readonly IMongoCollection<User> _owners;

        private readonly List<IPetConfigurationSettings> _petSettings;

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
                pet.UpdateMetrics();
            }
            return foundPets;
        }

        public Pet Get(string id) {
            Pet foundPet = _pets.Find<Pet>(pet => pet.Id == id).FirstOrDefault();
            foundPet.UpdateMetrics();
            return foundPet;
        }

        public List<Pet> GetUserPets(string ownerId) {
            List<Pet> foundPets = _pets.Find<Pet>(pet => pet.OwnerID == ownerId).ToList();
            foreach (Pet pet in foundPets)
            {
                pet.UpdateMetrics();
            }
            return foundPets;        
        }
        public Pet Create(string id, string petType = "dog")
        {
            Pet newPet = PetFactory.GeneratePetByType(petType,_petSettings);
            newPet.OwnerID = id;
            newPet.LastUpdate = DateTime.Now;
            _pets.InsertOne(newPet);
            var update = Builders<User>.Update.Push<string>(owner => owner.OwnedPets, newPet.Id);
            _owners.UpdateOne(user => user.Id == newPet.OwnerID, update);
            return newPet;
        }

        public void Update(string id, Pet newPet) {
            newPet.UpdateMetrics();
            _pets.ReplaceOne(pet => pet.Id == id, newPet);
        }

        public void Stroke(string id) {
            Pet petToStroke = Get(id); 
            petToStroke.UpdateMetrics();
            petToStroke.Stroke();
            Update(id, petToStroke);
        }

        public void Feed(string id) {
            Pet petToFeed = Get(id);
            petToFeed.UpdateMetrics();
            petToFeed.Feed();
            Update(id, petToFeed);
        }


        public void Remove(string id) {
            string ownerID = Get(id).OwnerID;
            _pets.DeleteOne(pet => pet.Id == id);
            var update = Builders<User>.Update.Pull(user => user.OwnedPets, id);
            _owners.UpdateOne(user => user.Id == ownerID ,update);
        }
    }
}