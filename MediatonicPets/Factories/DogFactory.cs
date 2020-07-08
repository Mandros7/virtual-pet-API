using MediatonicPets.Models;

namespace MediatonicPets.Factories
{
    public class DogFactory : PetFactory
    {       
        private string BASE_TYPE = "dog"; 

        public override Pet GetPet() {
            Pet newPet = base.GetPet();
            newPet.Type = BASE_TYPE;
            return newPet;
        }

        public DogFactory(IPetConfigurationSettings settings) : base(settings) {
            BASE_TYPE = "dog";                   
        }

        public DogFactory() : base() {
        }
    }
}