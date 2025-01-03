using System;
using UnityEngine;

namespace UGUIParticleEffect
{
    internal class ParticleEventHandler : MonoBehaviour
    {
        public static ParticleEventHandler Create(ParticleSystem particleSystem, Action callback)
        {
            ParticleEventHandler instance = particleSystem.gameObject.AddComponent<ParticleEventHandler>();
            instance.ParticleStopped += callback;
            return instance;
        }

        public event Action ParticleStopped;

        private void OnParticleSystemStopped()
        {
            ParticleStopped?.Invoke();
        }
    }
}