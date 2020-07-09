using System;
using MediatonicPets.Factories;
using MediatonicPets.Models;
using Xunit;

namespace MediatonicPets.Tests.Models
{
    public class PetTest
    {

        private readonly DogFactory _dogFactory;
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
            generatedPet.LastUpdate =  DateTime.UtcNow - TimeSpan.FromMinutes(minutesElapsed);
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
            generatedPet.LastUpdate =  DateTime.UtcNow - TimeSpan.FromMinutes(minutesElapsed);
            generatedPet.UpdateMetrics();
            Assert.Equal(generatedPet.Hungriness, generatedPet.HUNGRINESS_MAX);
            Assert.Equal(generatedPet.Happiness, generatedPet.HAPPINESS_MIN);
        }

        [Fact]
        public void StrokeTest()
        {
            Pet generatedPet = _dogFactory.GetPet();
            generatedPet.Happiness = generatedPet.HAPPINESS_MAX - 2.0f*generatedPet.StrokeHappiness;
            float expectedHapiness = generatedPet.Happiness + generatedPet.StrokeHappiness;
            generatedPet.Stroke();
            Assert.Equal(generatedPet.Happiness, expectedHapiness);
        }

        [Fact]
        public void StrokeTestMax()
        {
            Pet generatedPet = _dogFactory.GetPet();
            generatedPet.Happiness = generatedPet.HAPPINESS_MAX;
            float expectedHapiness = generatedPet.HAPPINESS_MAX;
            generatedPet.Stroke();
            Assert.Equal(generatedPet.Happiness, expectedHapiness);
        }

        [Fact]
        public void FeedTest()
        {
            Pet generatedPet = _dogFactory.GetPet();
            generatedPet.Hungriness = generatedPet.HUNGRINESS_MAX + 2.0f*generatedPet.FeedHungriness;
            float expectedHungriness = generatedPet.Hungriness + generatedPet.FeedHungriness;
            generatedPet.Feed();
            Assert.Equal(generatedPet.Hungriness, expectedHungriness);
        }

        [Fact]
        public void FeedTestMin()
        {
            Pet generatedPet = _dogFactory.GetPet();
            generatedPet.Hungriness = generatedPet.HUNGRINESS_MIN;
            float expectedHungriness = generatedPet.HUNGRINESS_MIN;
            generatedPet.Feed();
            Assert.Equal(generatedPet.Hungriness, expectedHungriness);
        }

    }
}