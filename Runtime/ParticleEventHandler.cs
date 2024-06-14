#if COFFEE_PARTICLES
using System;
using UnityEngine;

namespace UGUIParticleEffect
{
    internal class ParticleEventHandler : MonoBehaviour
    {
        public static ParticleEventHandler Create(ParticleSystem particleSystem)
        {
            return particleSystem.gameObject.AddComponent<ParticleEventHandler>();
        }

        public event Action ParticleStopped;

        private void OnParticleSystemStopped()
        {
            ParticleStopped?.Invoke();
        }
    }
}
#endif