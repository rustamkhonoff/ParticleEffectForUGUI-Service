using System;
using System.Collections.Generic;
using UGUIParticleEffect.Builder;
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
        [SerializeField] private ForceConfiguration _forceConfiguration;
        [SerializeField] private Vector2 _minMaxParticlesSize = new Vector2(5, 6);
        public UIParticleAttractorView ParticleAttractorViewPrefab => _particleAttractorViewPrefab;
        public Canvas CanvasPrefab => _canvasPrefab;

        public int MaxAttractParticlesAmount => _maxAttractParticlesAmount;

        public Material DefaultReferenceMaterial => _defaultReferenceMaterial;

        public ParticleSystem DefaultParticleSystemPrefab => _defaultParticleSystemPrefab;

        public ForceConfiguration ForceConfiguration => _forceConfiguration;

        public Vector2 MinMaxParticlesSize => _minMaxParticlesSize;
    }

    [Serializable]
    public class ForceConfiguration
    {
        [Serializable]
        public class Data
        {
            public Vector2 MinMaxForce;
            public float Drag;

            public Data(Vector2 minMaxForce, float drag)
            {
                MinMaxForce = minMaxForce;
                Drag = drag;
            }
        }

        public Data Small = new(new Vector2(0, 50), 0.007f), Medium = new(new Vector2(0, 500), 0.005f), Big = new(new Vector2(0, 1000), 0.003f);

        public Dictionary<ForceAmountType, (Vector2 minMaxForce, float drag)> GetConfiguration()
        {
            return new Dictionary<ForceAmountType, (Vector2 minMaxForce, float drag)>()
            {
                { ForceAmountType.Small, (Small.MinMaxForce, Small.Drag) },
                { ForceAmountType.Medium, (Medium.MinMaxForce, Medium.Drag) },
                { ForceAmountType.Big, (Big.MinMaxForce, Big.Drag) }
            };
        }
    }
}