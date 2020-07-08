using System.Collections.Generic;
using System.Linq;
using MediatonicPets.Models;

namespace MediatonicPets.Factories
{
    public abstract class PetFactory
    {
        protected float BASE_HAPINESS = 50.0f;
        protected float BASE_HUNGRINESS = 50.0f;
        protected float BASE_HAPINESS_RATE = -0.3f;
        protected float BASE_HUNGRINESS_RATE = 0.4f;
        protected float BASE_STROKE_HAPINESS = 5.0f;
        protected float BASE_FEED_HUNGRINESS = -10.0f;
        public virtual Pet GetPet() {
            Pet newPet = new Pet();
            newPet.Happiness = BASE_HAPINESS;
            newPet.Hungriness = BASE_HUNGRINESS;
            newPet.HappinessRate = BASE_HAPINESS_RATE;
            newPet.HungrinessRate = BASE_HUNGRINESS_RATE;
            newPet.StrokeHapiness = BASE_STROKE_HAPINESS;
            newPet.FeedHungriness = BASE_FEED_HUNGRINESS;
            return newPet;
        }    

        public PetFactory(IPetConfigurationSettings settings)  {
            BASE_HAPINESS = settings.Hapiness;
            BASE_HUNGRINESS = settings.Hungriness;
            BASE_HAPINESS_RATE = settings.HapinessRate;
            BASE_HUNGRINESS_RATE = settings.HungrinessRate;
            BASE_STROKE_HAPINESS = settings.StrokeHapiness;
            BASE_FEED_HUNGRINESS = settings.FeedHungriness;  
        }


        public PetFactory() {

        }

        public static Pet GeneratePetByType(string petType, List<IPetConfigurationSettings> _petSettings) {
            PetFactory petFact;
            string petTypeLC = petType.ToLower();
            switch (petTypeLC)
            {
                case "dog":
                    petFact = new DogFactory((IPetConfigurationSettings)_petSettings.Where(sett => sett.Type == "dog").First());
                    break;
                default:
                    petFact = new DogFactory((IPetConfigurationSettings)_petSettings.Where(sett => sett.Type == "dog").First());
                    break;
            }
            return petFact.GetPet();
        } 
    }
}