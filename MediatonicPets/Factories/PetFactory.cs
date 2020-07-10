using System;
using System.Collections.Generic;
using System.Linq;
using MediatonicPets.Models;

namespace MediatonicPets.Factories
{
    /// <summary>
    /// Class <c>PetFactory</c> provides an interface to Pet Generation Classes
    /// This class includes one static method to Generate a Pet passing just the type.
    /// </summary>
    public class PetFactory
    {
        private float BASE_HAPPINESS = 50.0f;
        private float BASE_HUNGRINESS = 50.0f;
        private float BASE_HAPPINESS_RATE = -0.3f;
        private float BASE_HUNGRINESS_RATE = 0.4f;
        private float BASE_STROKE_HAPPINESS = 5.0f;
        private float BASE_FEED_HUNGRINESS = -10.0f;

        private List<PetConfigurationSettings> _settings;

        /// <summary>Method <c>GetPet</c> returns the pet using the current configuration values</summary>
        public Pet GetPet() {
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
        public PetFactory(List<PetConfigurationSettings> settings)  {
            _settings = settings; 
        }

        
        /// <summary>The method <c>GetPet</c> instantiates a  Pet 
        /// based on the type of Pet and using the config files as source.
        /// </summary>
        public Pet GetPet(string petType) {
            string petTypeLC = petType.ToLower();
            var foundSettings = _settings.Where(sett => sett.Type.Equals(petTypeLC)).ToList();
            if (foundSettings.Count == 0){
                if (Enum.IsDefined(typeof(PetTypes), petTypeLC)){
                    Pet basePet = GetPet();
                    basePet.Type = petTypeLC;
                    return basePet;
                }
                else return null; 
            }
            else {
                PetConfigurationSettings petToCreateSettings = foundSettings.First();
                Pet newPet = new Pet();
                newPet.Happiness = petToCreateSettings.Happiness;
                newPet.Hungriness = petToCreateSettings.Hungriness;
                newPet.HappinessRate = petToCreateSettings.HappinessRate;
                newPet.HungrinessRate = petToCreateSettings.HungrinessRate;
                newPet.StrokeHappiness = petToCreateSettings.StrokeHappiness;
                newPet.FeedHungriness = petToCreateSettings.FeedHungriness;
                newPet.Type = petTypeLC;
                return newPet;
            }
        } 
    }
}