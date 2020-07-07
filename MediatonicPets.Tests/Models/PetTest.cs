using System;
using MediatonicPets.Factories;
using MediatonicPets.Models;
using Xunit;

namespace MediatonicPets.Tests.Models
{
    public class PetTest
    {

        private readonly DogFactory _dogFactory;
        private readonly PetConfigurationSettings _dogSettings;
        public PetTest() {
            _dogFactory = new DogFactory();
        }
        
        [Fact]
        public void UpdateStats()
        {
            Pet generatedPet = _dogFactory.GetPet();
            float initialHapiness = generatedPet.Happiness;
            float initialHungriness = generatedPet.Hungriness;
            float minutesElapsed = 5.0f;
            float expectedHapiness = initialHapiness + minutesElapsed * generatedPet.HappinessRate;
            float expectedHungriness = initialHungriness + minutesElapsed * generatedPet.HungrinessRate;
            generatedPet.LastUpdate =  DateTime.Now - TimeSpan.FromMinutes(minutesElapsed);
            generatedPet.UpdateMetrics();
            Assert.Equal(generatedPet.Hungriness, expectedHungriness);
            Assert.Equal(generatedPet.Happiness, expectedHapiness);
        }
        [Fact]
        public void UpdateStatsHungryAndUnhappyLimits()
        {
            Pet generatedPet = _dogFactory.GetPet();
            float minutesToSadness = MathF.Ceiling((generatedPet.Happiness - generatedPet.HAPPINESS_MIN) / MathF.Abs(generatedPet.HappinessRate));
            float minutesToStarve = MathF.Ceiling((generatedPet.HUNGRINESS_MAX - generatedPet.Hungriness) / MathF.Abs(generatedPet.HungrinessRate));
            float minutesElapsed = (minutesToSadness > minutesToStarve) ? minutesToSadness : minutesToStarve;
            generatedPet.LastUpdate =  DateTime.Now - TimeSpan.FromMinutes(minutesElapsed);
            generatedPet.UpdateMetrics();
            Assert.Equal(generatedPet.Hungriness, generatedPet.HUNGRINESS_MAX);
            Assert.Equal(generatedPet.Happiness, generatedPet.HAPPINESS_MIN);
        }

    }
}