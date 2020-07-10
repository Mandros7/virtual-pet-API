using System.Collections.Generic;
using MediatonicPets.Models;
using Xunit;

namespace MediatonicPets.Factories.Tests
{
    public class FactoryTest
    {
        private readonly PetFactory _petFactory;
        private readonly List<PetConfigurationSettings> _allSettings;

        public FactoryTest () {
            _allSettings = GlobalPetConfigurationSettings.generateDefaultSettings().Metrics;
            _petFactory = new PetFactory(_allSettings);
        }

        [Fact]
        public void DogFactory_NotConfigured()
        {
            Pet expectedPet = new Pet();
            expectedPet.Type = "fish";
            Pet generatedPet = _petFactory.GetPet("fish");
            Assert.Equal(expectedPet.Type,generatedPet.Type);
        }

        [Fact]
        public void FactoryByType()
        {
            Pet newPet = _petFactory.GetPet("dog");
            Assert.Equal("dog",newPet.Type);
        }

        [Fact]
        public void UnImplementedPetTest()
        {
            Pet newPet = _petFactory.GetPet("dragon");
            Assert.Null(newPet);
        }
    }
}