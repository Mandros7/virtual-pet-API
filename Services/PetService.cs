using MediatonicPets.Models;
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
        private readonly PetFactory _petFactory;

        public PetService(IPetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _pets = database.GetCollection<Pet>(settings.PetCollectionName);
            _owners = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<Pet> Get() {
            List<Pet> foundPets = _pets.Find(pet => true).ToList();
            return foundPets.Select(pet => _petFactory.UpdatePetValues(pet)).ToList();
        }

        public Pet Get(string id) {
            Pet foundPet = _pets.Find<Pet>(pet => pet.Id == id).FirstOrDefault();
            return _petFactory.UpdatePetValues(foundPet);
        }

        public List<Pet> GetPets(string ownerId) {
            List<Pet> foundPets = _pets.Find<Pet>(pet => pet.OwnerID == ownerId).ToList();
            return foundPets.Select(pet => _petFactory.UpdatePetValues(pet)).ToList();
        }
        public Pet Create(string id, string petType)
        {
            if (Enum.IsDefined(typeof(PetTypes), petType)) {
                return null;
            }
            Pet newPet = GeneratePetByType(petType);
            newPet.LastUpdate = DateTime.Now;
            _pets.InsertOne(newPet);
            var update = Builders<User>.Update.Push<string>(owner => owner.OwnedPets, newPet.Id);
            _owners.UpdateOne(user => user.Id == newPet.OwnerID, update);
            return newPet;
        }

        public void Update(string id, Pet newPet) =>
            _pets.ReplaceOne(pet => pet.Id == id, newPet);

        public void Stroke(string id, Pet petToStroke) {
            Pet updatedPet = _petFactory.UpdatePetValues(petToStroke);
            Update(id, _petFactory.StrokePet(updatedPet));
        }

        public void Feed(string id, Pet petToFeed) {
            Pet updatedPet = _petFactory.UpdatePetValues(petToFeed);
            Update(id, _petFactory.FeedPet(updatedPet));
        }


        public void Remove(string id) {
            _pets.DeleteOne(pet => pet.Id == id);
            var update = Builders<User>.Update.PullFilter(user => user.OwnedPets, pet => pet == id);
            _owners.UpdateOne(user => user.OwnedPets.Contains(id),update);
        }

        private Pet GeneratePetByType(string petType) {
            PetFactory petFact;
            string petTypeLC = petType.ToLower();
            switch (petTypeLC)
            {
                case "dog":
                    petFact = new DogFactory();
                    break;
                default:
                    petFact = new DogFactory();
                    break;
            }
            return petFact.GetPet();
        }

    }
}