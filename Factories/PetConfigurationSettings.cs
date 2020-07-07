using System.Collections.Generic;

namespace MediatonicPets.Factories
{

    public class GlobalPetConfigurationSettings : IGlobalPetConfigurationSettings
    {
        public List<IPetConfigurationSettings> Metrics { get; set; }   
    }

    public interface IGlobalPetConfigurationSettings
    {
        List<IPetConfigurationSettings> Metrics { get; set; }
    }

    public class PetConfigurationSettings : IPetConfigurationSettings
    {
        public string Type { get; set; }
        public float Hapiness { get; set; }
        public float Hungriness { get; set; }
        public float HapinessRate { get; set; }
        public float HungrinessRate { get; set; }
        public float StrokeHapiness { get; set; }
        public float FeedHungriness { get; set; }

    }

    public interface IPetConfigurationSettings
    {
        string Type { get; set; }
        float Hapiness { get; set; }
        float Hungriness { get; set; }
        float HapinessRate { get; set; }
        float HungrinessRate { get; set; }
        float StrokeHapiness { get; set; }
        float FeedHungriness { get; set; }
    }

}