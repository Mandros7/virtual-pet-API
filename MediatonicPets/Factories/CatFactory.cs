using MediatonicPets.Models;

namespace MediatonicPets.Factories
{
    /// <summary>
    /// Class <c>CatFactory</c> inherits from PetFactory and allows to potentially insert additional
    /// features to the generation of specific Pets. In the event that different pets have different metrics,
    /// it will be possible to attend to those here.
    /// </summary>
    public class CatFactory : PetFactory
    {       
        private string BASE_TYPE = "cat"; 
        public override Pet GetPet() {
            Pet newPet = base.GetPet();
            newPet.Type = BASE_TYPE;
            return newPet;
        }

        public CatFactory(IPetConfigurationSettings settings) : base(settings) {
            BASE_TYPE = "dog";                   
        }

        public CatFactory() : base() {
        }
    }
}