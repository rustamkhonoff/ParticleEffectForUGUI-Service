using System;
using UnityEngine;

namespace UGUIParticleEffect
{
    [CreateAssetMenu(menuName = "Plugins/UI/Particles/Create UIParticlesEffectsConfiguration",
        fileName = "UIParticlesEffectsConfiguration", order = 0)] [Serializable]
    internal class UIParticlesEffectsConfiguration : ScriptableObject
    {
        [field: SerializeField] public UIParticleAttractorView ParticleAttractorViewPrefab { get; private set; }
        [field: SerializeField] public Canvas CanvasPrefab { get; private set; }
        [field: SerializeField] public int MaxAttractParticlesAmount { get; private set; } = 20;
        [field: SerializeField] public ParticleSystem DefaultParticleSystemPrefab { get; private set; }
        [field: SerializeField] public Material DefaultReferenceMaterial { get; private set; }
    }
}