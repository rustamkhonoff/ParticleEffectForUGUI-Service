namespace Extensions
{
    public class ServiceConfiguration
    {
        public string ConfigurationPath { get; set; } = "StaticData/UI/UIParticlesEffectsConfiguration";
        public static ServiceConfiguration Default => new ServiceConfiguration();
    }
}