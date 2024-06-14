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
            diContainer
                .Bind<IUIParticleEffectsService>()
                .To<UGUIParticleEffect.Implementation.UIParticleEffectsService>()
                .AsSingle()
                .WithArguments(Resources.Load("StaticData/UI/UIParticlesEffectsConfiguration"))
                .NonLazy();
        }
    }
}
#endif