using System;
using Coffee.UIExtensions;
using UGUIParticleEffect.Builder;
using UnityEngine;

namespace UGUIParticleEffect
{
    public interface IUIParticlesService
    {
        void Attract(UIParticleConfiguration configuration,
            Action<UIParticle> configureUIParticle = null,
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null);

        void ClearAll();
    }
}