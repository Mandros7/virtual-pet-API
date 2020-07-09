using System.Collections.Generic;
using MediatonicPets.Models;
using Xunit;

namespace MediatonicPets.Factories.Tests
{
    public class FactoryTest
    {
        private readonly DogFactory _dogFactory;
        private readonly PetConfigurationSettings _dogSettings;

        private readonly List<PetConfigurationSettings> _allSettings;

        public FactoryTest () {

            _dogSettings = new PetConfigurationSettings();
            _dogSettings.Type = "dog";
            _dogFactory = new DogFactory(_dogSettings);
            _allSettings = GlobalPetConfigurationSettings.generateDefaultSettings().Metrics;
        }

        [Fact]
        public void DogFactory_Default()
        {
            Pet expectedPet = new Pet();
            expectedPet.Type = "dog";
            Pet generatedPet = _dogFactory.GetPet();
            Assert.Equal(expectedPet.Type,generatedPet.Type);
        }

        [Fact]
        public void FactoryByType()
        {
            Pet newPet = PetFactory.GeneratePetByType("dog",_allSettings);
            Assert.Equal("dog",newPet.Type);
        }
    }
}