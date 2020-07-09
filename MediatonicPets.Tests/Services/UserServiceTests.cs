using System;
using System.Collections.Generic;
using MediatonicPets.Models;
using MediatonicPets.Services;
using MongoDB.Driver;
using Xunit;

namespace MediatonicPets.Tests.Services
{
    [Collection("SequentialDatabaseUse")]
    public class UserServiceTests
    {
        private UserService _userService;
        private readonly IMongoCollection<User> _usersDB;

        public UserServiceTests() {
            PetDatabaseSettings settings = new PetDatabaseSettings();
            settings.ConnectionString = "mongodb+srv://sampleuser:sampleuser@cluster0.xcels.mongodb.net/petDbStoreTest?retryWrites=true&w=majority";
;
            settings.DatabaseName = "petDbStore";
            settings.PetCollectionName = "pets";
            settings.UserCollectionName = "users";
            _userService = new UserService(settings);
            
            // Cleanup environment before running tests
            string detectedHost = Environment.GetEnvironmentVariable("MONGODB_HOST");
            if (detectedHost == null) {
                detectedHost = settings.ConnectionString;
            }
            var client = new MongoClient(detectedHost);
            var database = client.GetDatabase(settings.DatabaseName);
            _usersDB = database.GetCollection<User>(settings.UserCollectionName);
        }

        [Fact]
        public void CreateUserTest() {
            _usersDB.DeleteMany(user => true);
            User singleUser = _userService.Create(new User());
            User userInDB = _usersDB.Find(user => user.Id == singleUser.Id).FirstOrDefault();
            Assert.Equal(singleUser.Id,userInDB.Id);
            Assert.Empty(userInDB.OwnedPets);
        }

        [Fact]
        public void GetSingleUserTest() {
            //Setup with a single user
            _usersDB.DeleteMany(user => true);
            User insertedUser = _userService.Create(new User());

            User extractedUser = _userService.Get(insertedUser.Id);
            Assert.Equal(extractedUser.Id,insertedUser.Id);
        }

        [Fact]
        public void GetMultipleUsersTest() {
            //Setup with a two users
            _usersDB.DeleteMany(user => true);
            User insertedUser1 = _userService.Create(new User());
            User insertedUser2 = _userService.Create(new User());
            
            List<User> extractedUsers = _userService.Get();
            Assert.Equal(2,extractedUsers.Count);
        }
        [Fact]
        public void DeleteUserTest() {
            //Setup with a single user
            _usersDB.DeleteMany(user => true);
            User insertedUser1 = _userService.Create(new User());
            User insertedUser2 = _userService.Create(new User());
            
            _userService.Remove(insertedUser1.Id);
            List<User> usersInDB = _userService.Get();
            Assert.Empty(usersInDB.FindAll(user => user.Id == insertedUser1.Id));
        }

    }
}