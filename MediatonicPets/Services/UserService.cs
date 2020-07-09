using MediatonicPets.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicPets.Services
{
    /// <summary>
    /// Class <c>UserService</c>  makes uses of Models and MongoDB drivers to 
    /// act as a bridge between the received controller actions and the persistence MongoDB back-end.
    /// </summary>
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Pet> _pets;

        public UserService(IPetDatabaseSettings settings)
        {
            string detectedHost = Environment.GetEnvironmentVariable("MONGODB_HOST");
            if (detectedHost == null || detectedHost.Equals("")) {
                detectedHost = settings.ConnectionString;
            }
            var client = new MongoClient(detectedHost);
            var database = client.GetDatabase(settings.DatabaseName);

            _pets = database.GetCollection<Pet>(settings.PetCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id.Equals(id)).FirstOrDefault();

        public User Create(User user)
        {
            user.OwnedPets = new List<string> {}; // Initialized with no pets
            _users.InsertOne(user);
            return user;
        }
        
        public void Remove(string id) {
            _users.DeleteOne(user => user.Id.Equals(id));
            // Remove all associated pets
            _pets.DeleteMany(pet => pet.OwnerID.Equals(id)); 
            // We could also allow them to stay as orphans and let someone else adopt them...
        }

        public void AdoptPet(string id) {
            throw new NotImplementedException();  
        }
        
    }
}