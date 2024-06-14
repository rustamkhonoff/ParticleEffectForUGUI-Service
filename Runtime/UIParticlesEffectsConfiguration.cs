#if COFFEE_PARTICLES
using UnityEngine;

namespace UGUIParticleEffect
{
    [CreateAssetMenu(menuName = "Project/UI/Particles/Create UIParticlesEffectsConfiguration",
        fileName = "UIParticlesEffectsConfiguration", order = 0)]
    internal class UIParticlesEffectsConfiguration : ScriptableObject
    {
        [SerializeField] private UIParticleAttractorView _particleAttractorViewPrefab;
        [SerializeField] private Canvas _canvasPrefab;
        [SerializeField] private int _maxAttractParticlesAmount = 20;
        [SerializeField] private ParticleSystem _defaultParticleSystemPrefab;
        [SerializeField] private Material _defaultReferenceMaterial;
        public UIParticleAttractorView ParticleAttractorViewPrefab => _particleAttractorViewPrefab;
        public Canvas CanvasPrefab => _canvasPrefab;

        public int MaxAttractParticlesAmount => _maxAttractParticlesAmount;

        public Material DefaultReferenceMaterial => _defaultReferenceMaterial;

        public ParticleSystem DefaultParticleSystemPrefab => _defaultParticleSystemPrefab;
    }
}
#endif