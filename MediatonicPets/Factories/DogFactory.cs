using MediatonicPets.Models;

namespace MediatonicPets.Factories
{
    public class DogFactory : PetFactory
    {
        private float BASE_HAPINESS = 50.0f;
        private float BASE_HUNGRINESS = 50.0f;
        private float BASE_HAPINESS_RATE = -0.3f;
        private float BASE_HUNGRINESS_RATE = 0.4f;
        private float BASE_STROKE_HAPINESS = 5.0f;
        private float BASE_FEED_HUNGRINESS = -10.0f;

        private string BASE_TYPE = "dog"; 

        public override Pet GetPet() {
            Pet newPet = new Pet();
            newPet.Type = BASE_TYPE;
            newPet.Happiness = BASE_HAPINESS;
            newPet.Hungriness = BASE_HUNGRINESS;
            newPet.HappinessRate = BASE_HAPINESS_RATE;
            newPet.HungrinessRate = BASE_HUNGRINESS_RATE;
            newPet.StrokeHapiness = BASE_STROKE_HAPINESS;
            newPet.FeedHungriness = BASE_FEED_HUNGRINESS;
            return newPet;
        }

        public DogFactory(IPetConfigurationSettings settings) {
            BASE_HAPINESS = settings.Hapiness;
            BASE_HUNGRINESS = settings.Hungriness;
            BASE_HAPINESS_RATE = settings.HapinessRate;
            BASE_HUNGRINESS_RATE = settings.HungrinessRate;
            BASE_STROKE_HAPINESS = settings.StrokeHapiness;
            BASE_FEED_HUNGRINESS = settings.FeedHungriness;       
        }
    }
}