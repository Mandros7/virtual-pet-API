using System.Collections.Generic;

namespace MediatonicPets.Factories
{
    /// <summary>
    /// Class <c>GlobalPetConfigurationSettings</c> allows deserialization of all Pet configurations, 
    /// removing the need to modify metric values within the code.
    /// </summary>
    public class GlobalPetConfigurationSettings : IGlobalPetConfigurationSettings
    {
        public List<PetConfigurationSettings> Metrics { get; set; }   
        public static GlobalPetConfigurationSettings generateDefaultSettings() {
            
            GlobalPetConfigurationSettings petSettings = new GlobalPetConfigurationSettings();
            PetConfigurationSettings petMetrics = new PetConfigurationSettings();
            petMetrics.Type = "dog";
            petMetrics.HappinessRate = -0.1f;
            petMetrics.HungrinessRate = 1.0f;
            petMetrics.Hungriness = 50.0f;
            petMetrics.Happiness = 50.0f;
            petMetrics.FeedHungriness = -10.0f;
            petMetrics.StrokeHappiness = 10.0f;
            petSettings.Metrics = new List<PetConfigurationSettings> {petMetrics};
            return petSettings;
        }
    }

    public interface IGlobalPetConfigurationSettings
    {
        List<PetConfigurationSettings> Metrics { get; set; }
    }
    /// <summary>
    /// Class <c>PetConfigurationSettings</c> allows deserialization of one specific set of, 
    /// removing the need to modify metric values within the code.
    /// </summary>
    public class PetConfigurationSettings : IPetConfigurationSettings
    {
        public string Type { get; set; }
        public float Happiness { get; set; }
        public float Hungriness { get; set; }
        public float HappinessRate { get; set; }
        public float HungrinessRate { get; set; }
        public float StrokeHappiness { get; set; }
        public float FeedHungriness { get; set; }
    }

    public interface IPetConfigurationSettings
    {
        string Type { get; set; }
        float Happiness { get; set; }
        float Hungriness { get; set; }
        float HappinessRate { get; set; }
        float HungrinessRate { get; set; }
        float StrokeHappiness { get; set; }
        float FeedHungriness { get; set; }
    }


}