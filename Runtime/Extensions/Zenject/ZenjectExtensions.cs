#if ZENJECT
using System;
using Extensions;
using UGUIParticleEffect;
using UnityEngine;
using Zenject;

namespace ParticleEffectForUGUIService.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddUIParticlesService(this DiContainer diContainer, Action<ServiceConfiguration> configure = null)
        {
            ServiceConfiguration serviceConfiguration = ServiceConfiguration.Default;
            configure?.Invoke(serviceConfiguration);
            diContainer
                .Bind<IUIParticlesService>()
                .To<UGUIParticleEffect.Implementation.IuiParticlesService>()
                .AsSingle()
                .WithArguments(Resources.Load(serviceConfiguration.ConfigurationPath))
                .NonLazy();
        }
    }
}
#endif