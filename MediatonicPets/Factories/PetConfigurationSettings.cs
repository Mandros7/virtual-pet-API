using System.Collections.Generic;

namespace MediatonicPets.Factories
{
    /// <summary>
    /// Class <c>GlobalPetConfigurationSettings</c> allows deserialization of all Pet configurations, 
    /// removing the need to modify metric values within the code.
    /// </summary>
    public class GlobalPetConfigurationSettings : IGlobalPetConfigurationSettings
    {
        public List<IPetConfigurationSettings> Metrics { get; set; }   
        public static GlobalPetConfigurationSettings generateDefaultSettings() {
            
            GlobalPetConfigurationSettings petSettings = new GlobalPetConfigurationSettings();
            PetConfigurationSettings petMetrics = new PetConfigurationSettings();
            petMetrics.Type = "dog";
            petMetrics.HapinessRate = -0.1f;
            petMetrics.HungrinessRate = 1.0f;
            petMetrics.Hungriness = 50.0f;
            petMetrics.Hapiness = 50.0f;
            petMetrics.FeedHungriness = -10.0f;
            petMetrics.StrokeHapiness = 10.0f;
            petSettings.Metrics = new List<IPetConfigurationSettings> {petMetrics};
            return petSettings;
        }
    }

    public interface IGlobalPetConfigurationSettings
    {
        List<IPetConfigurationSettings> Metrics { get; set; }
    }
    /// <summary>
    /// Class <c>PetConfigurationSettings</c> allows deserialization of one specific set of pet metrics, 
    /// removing the need to modify metric values within the code.
    /// </summary>
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