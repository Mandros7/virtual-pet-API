namespace MediatonicPets.Models
{
    public class DogFactory : PetFactory
    {
        //TODO: Extract configuration values!
        //Move factories outside of Models! 
        public override Pet GetPet() {
            Pet newPet = new Pet();
            newPet.Type = "dog";
            newPet.Happiness = 50.0f;
            newPet.Hungriness = 50.0f;
            newPet.HappinessRate = -0.3f;
            newPet.HungrinessRate = 0.4f;
            newPet.StrokeHapiness = 5.0f;
            newPet.FeedHungriness = -10.0f;
            return newPet;
        }
    }
}