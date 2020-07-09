using System.Collections.Generic;
using System.Linq;
using MediatonicPets.Models;

namespace MediatonicPets.Factories
{
    /// <summary>
    /// Class <c>PetFactory</c> provides an interface to Pet Generation Classes
    /// This class includes one static method to Generate a Pet passing just the type.
    /// </summary>
    public abstract class PetFactory
    {
        protected float BASE_HAPPINESS = 50.0f;
        protected float BASE_HUNGRINESS = 50.0f;
        protected float BASE_HAPPINESS_RATE = -0.3f;
        protected float BASE_HUNGRINESS_RATE = 0.4f;
        protected float BASE_STROKE_HAPPINESS = 5.0f;
        protected float BASE_FEED_HUNGRINESS = -10.0f;

        /// <summary>Method <c>GetPet</c> returns the pet using the current configuration values</summary>
        public virtual Pet GetPet() {
            Pet newPet = new Pet();
            newPet.Happiness = BASE_HAPPINESS;
            newPet.Hungriness = BASE_HUNGRINESS;
            newPet.HappinessRate = BASE_HAPPINESS_RATE;
            newPet.HungrinessRate = BASE_HUNGRINESS_RATE;
            newPet.StrokeHappiness = BASE_STROKE_HAPPINESS;
            newPet.FeedHungriness = BASE_FEED_HUNGRINESS;
            return newPet;
        }    

        /// <summary>This constructor allows to use configuration files as sources for the metrics</summary>
        public PetFactory(IPetConfigurationSettings settings)  {
            BASE_HAPPINESS = settings.Happiness;
            BASE_HUNGRINESS = settings.Hungriness;
            BASE_HAPPINESS_RATE = settings.HappinessRate;
            BASE_HUNGRINESS_RATE = settings.HungrinessRate;
            BASE_STROKE_HAPPINESS = settings.StrokeHappiness;
            BASE_FEED_HUNGRINESS = settings.FeedHungriness;  
        }

        /// <summary>This constructor allows to the default values as sources for the metrics</summary>
        public PetFactory() {

        }
        /// <summary>The static method <c>GeneratePetByType</c> instantiates a child Pet Factory 
        /// based on the type of Pet and using the config files as source.
        /// </summary>
        public static Pet GeneratePetByType(string petType, List<PetConfigurationSettings> _petSettings) {
            PetFactory petFact;
            string petTypeLC = petType.ToLower();
            var foundSettings = _petSettings.Where(sett => sett.Type == petTypeLC).ToList();
            if (foundSettings.Count == 0){
                return null;
            }
            switch (petTypeLC)
            {
                case "dog":
                    petFact = new DogFactory(foundSettings.First());
                    break;
                case "cat":
                    petFact = new CatFactory((IPetConfigurationSettings)_petSettings.Where(sett => sett.Type == petTypeLC).First());
                    break;
                default:
                    return null;
            }
            return petFact.GetPet();
        } 
    }
}