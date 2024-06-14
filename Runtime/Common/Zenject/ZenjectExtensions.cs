#if ZENJECT
using UGUIParticleEffect;
using UnityEngine;
using Zenject;

namespace ParticleEffectForUGUIService.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddUIParticlesService(this DiContainer diContainer)
        {
#if COFFEE_PARTICLES
            diContainer
                .Bind<IUIParticleEffectsService>()
                .To<global::UGUIParticleEffect.Implementation.UIParticleEffectsService>()
                .AsSingle()
                .WithArguments(Resources.Load("StaticData/UI/UIParticlesEffectsConfiguration"))
                .NonLazy();
#else
            Debug.LogError("There is no implementations for UIParticleService");
#endif
        }
    }
}
#endif