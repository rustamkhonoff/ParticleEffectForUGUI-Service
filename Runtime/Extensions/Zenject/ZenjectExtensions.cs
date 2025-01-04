#if ZENJECT
using System;
using UIParticle.Service;
using UIParticle.Service.Extensions;
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
                .Bind<IUIParticleService>()
                .To<DefaultUIParticleService>()
                .AsSingle()
                .WithArguments(Resources.Load(serviceConfiguration.ConfigurationPath))
                .NonLazy();
        }
    }
}
#endif