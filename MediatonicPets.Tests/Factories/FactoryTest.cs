using MediatonicPets.Models;
using Xunit;

namespace MediatonicPets.Factories.Tests
{
    public class FactoryTest
    {
        private readonly DogFactory _dogFactory;
        private readonly PetConfigurationSettings _dogSettings;
        public FactoryTest () {

            _dogSettings = new PetConfigurationSettings();
            _dogSettings.Type = "dog";
            _dogFactory = new DogFactory(_dogSettings);
        }

        [Fact]
        public void DogFactory_Default()
        {
            Pet expectedPet = new Pet();
            expectedPet.Type = "dog";
            Pet generatedPet = _dogFactory.GetPet();
            Assert.Equal(expectedPet.Type,generatedPet.Type);
        }
    }
}