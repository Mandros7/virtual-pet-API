using System.Collections.Generic;
using MediatonicPets.Factories;
using MediatonicPets.Models;
using MediatonicPets.Services;
using MongoDB.Driver;
using Xunit;

namespace MediatonicPets.Tests.Services
{
    [Collection("SequentialDatabaseUse")]
    public class PetServiceTests
    {
        private PetService _petService;
        private UserService _userService;

        private readonly IMongoCollection<User> _usersDB;
        private readonly IMongoCollection<Pet> _petsDB;

        public PetServiceTests() {
            PetDatabaseSettings settings = new PetDatabaseSettings();
            settings.ConnectionString = "mongodb+srv://sampleuser:sampleuser@cluster0.xcels.mongodb.net/petDbStoreTest?retryWrites=true&w=majority";
;
            settings.DatabaseName = "petDbStore";
            settings.PetCollectionName = "pets";
            settings.UserCollectionName = "users";
            _userService = new UserService(settings);
            _petService = new PetService(settings,GlobalPetConfigurationSettings.generateDefaultSettings());
            
            // Cleanup environment before running tests
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _usersDB = database.GetCollection<User>(settings.UserCollectionName);
            _petsDB = database.GetCollection<Pet>(settings.PetCollectionName);
        }

        [Fact]
        public void CreatePetTest() {
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            Pet singlePet = _petService.Create(singleUser.Id,"dog");
            Pet foundPet = _petsDB.Find(pet => pet.Id == singlePet.Id).FirstOrDefault();
            User updatedUser = _userService.Get(singleUser.Id);
            Assert.Equal(singlePet.Id, foundPet.Id);
            Assert.Equal(singlePet.OwnerID,singleUser.Id);
            Assert.Contains(singlePet.Id,updatedUser.OwnedPets);
        }

        [Fact]
        public void CreateInvalidPetTest() {
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            Pet singlePet = _petService.Create(singleUser.Id,"dragon");
            Assert.Null(singlePet);
        }
        [Fact]
        public void GetPetByIDTest() {
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            Pet singlePet = _petService.Create(singleUser.Id,"dog");
            Pet foundPet = _petService.Get(singlePet.Id);
            Assert.Equal(singlePet.Id,foundPet.Id);
        }

        [Fact]
        public void GetAllPetsTest() {
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            _petService.Create(singleUser.Id,"dog");
            _petService.Create(singleUser.Id,"cat");
            List<Pet> foundPets = _petService.Get();
            List<Pet> foundUserPets = _petService.GetUserPets(singleUser.Id);
            Assert.Equal(2,foundPets.Count);
            Assert.Equal(2,foundUserPets.Count);
        }

        [Fact]
        public void StrokePetTest(){
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            Pet initialPet = _petService.Create(singleUser.Id,"dog");
            _petService.Stroke(initialPet.Id);
            Pet foundPet = _petService.Get(initialPet.Id);
            Assert.NotEqual(initialPet.LastUpdate, foundPet.LastUpdate);
        }

        [Fact]
        public void FeedPetTest(){
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            Pet initialPet = _petService.Create(singleUser.Id,"dog");
            _petService.Feed(initialPet.Id);
            Pet foundPet = _petService.Get(initialPet.Id);
            Assert.NotEqual(initialPet.LastUpdate, foundPet.LastUpdate);
        }

        [Fact]
        public void DeleteUserPetsTest() {
            //Setup with a single user
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            Pet insertedPet1 = _petService.Create(singleUser.Id,"dog");
            Pet insertedPet2 = _petService.Create(singleUser.Id,"dog");
            
            _userService.Remove(singleUser.Id);
            List<Pet> petsInDB = _petService.Get();
            Assert.Empty(petsInDB);
        }
        [Fact]
        public void DeletePetTest() {
            //Setup with a single user
            _usersDB.DeleteMany(user => true);
            _petsDB.DeleteMany(pet => true);
            User singleUser = _userService.Create(new User());
            Pet insertedPet1 = _petService.Create(singleUser.Id,"dog");
            Pet insertedPet2 = _petService.Create(singleUser.Id,"dog");
            
            _petService.Remove(insertedPet1.Id);
            List<Pet> petsInDB = _petService.Get();
            User updatedUser = _userService.Get(singleUser.Id);
            Assert.Empty(petsInDB.FindAll(pet => pet.Id == insertedPet1.Id));
            Assert.Empty(updatedUser.OwnedPets.FindAll(pet => pet == insertedPet1.Id));
            
        }
    }
}