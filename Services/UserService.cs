using MediatonicPets.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicPets.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Pet> _pets;

        public UserService(IPetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _pets = database.GetCollection<Pet>(settings.PetCollectionName);
            _users = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }
        
        public void Remove(string id) {
            _users.DeleteOne(user => user.Id == id);
            _pets.DeleteMany(pet => pet.OwnerID == id);
        }
    }
}