using System;

namespace MediatonicPets.Models
{
    public abstract class PetFactory
    {
        private float HAPPINESS_MAX = 100.0f;
        private float HAPPINESS_MIN = 0.0f;
        private float HUNGRINESS_MAX = 100.0f;
        private float HUNGRINESS_MIN = 0.0f;

        public abstract Pet GetPet();

        public Pet UpdatePetValues (Pet petToUpdate) {
            TimeSpan elapsed = DateTime.Now - petToUpdate.LastUpdate;
            float newHappiness = petToUpdate.Happiness + petToUpdate.HappinessRate * elapsed.Minutes;
            petToUpdate.Happiness = (newHappiness < HAPPINESS_MIN) ? HAPPINESS_MIN : newHappiness;
            float newHungriness = petToUpdate.Hungriness + petToUpdate.HungrinessRate * elapsed.Minutes;
            petToUpdate.Hungriness = (newHungriness > HUNGRINESS_MAX) ? HUNGRINESS_MAX : newHungriness;
            petToUpdate.LastUpdate = DateTime.Now;
            return petToUpdate;
        }

        public Pet StrokePet (Pet petToUpdate) {
            float newHappiness = petToUpdate.Happiness + petToUpdate.StrokeHapiness;
            petToUpdate.Happiness = (newHappiness > HAPPINESS_MAX) ? HAPPINESS_MAX : newHappiness;
            return petToUpdate;
        }
        public Pet FeedPet (Pet petToUpdate) {
            float newHungriness = petToUpdate.Hungriness + petToUpdate.FeedHungriness;
            petToUpdate.Hungriness = (newHungriness < HUNGRINESS_MIN) ? HUNGRINESS_MIN : newHungriness;
            return petToUpdate;
        }
    }
}