using System;
using Coffee.UIExtensions;
using UnityEngine;

namespace UIParticle.Service
{
    public interface IUIParticleService
    {
        void Attract(UIParticleConfiguration configuration,
            Action<Coffee.UIExtensions.UIParticle> configureUIParticle = null,
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null);

        void ClearAll();
    }
}