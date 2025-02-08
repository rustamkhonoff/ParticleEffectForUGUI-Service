namespace UIParticle.Service.Extensions
{
    public class ServiceConfiguration
    {
        //Path to UIParticlesEffectsConfiguration located in Resources folder
        public string ConfigurationPath { get; set; } = "UIParticlesEffectsConfiguration";
        public static ServiceConfiguration Default => new();
    }
}