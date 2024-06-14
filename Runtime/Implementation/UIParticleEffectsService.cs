using System.Runtime.CompilerServices;
using UGUIParticleEffect.Builder;
using UnityEngine;
using Object = UnityEngine.Object;

[assembly: InternalsVisibleTo("UIParticleEffectService.Zenject")]

namespace UGUIParticleEffect.Implementation
{
    internal class UIParticleEffectsService : IUIParticleEffectsService
    {
        private readonly UIParticlesEffectsConfiguration m_configuration;

        private Transform m_canvasTransform;

        public UIParticleEffectsService(UIParticlesEffectsConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public void Attract(UIParticleAttractConfiguration configuration)
        {
            UIParticleAttractorView view =
                Object.Instantiate(m_configuration.ParticleAttractorViewPrefab, GetCanvasTransform());
            UIParticleTextureData textureData = new()
            {
                IsTextureSheetMode = configuration.TextureSheetEnabled,
                Mode = configuration.AnimationMode,
                GridTilesSize = configuration.TextureSheetSize,
                DefaultTexture = configuration.Texture,
                Sprites = configuration.TextureSheetSprites,
                Material = m_configuration.DefaultReferenceMaterial
            };
            ParticleSystem prefab = configuration.CustomPrefab != null
                ? configuration.CustomPrefab
                : m_configuration.DefaultParticleSystemPrefab;

            view.Attract(
                textureData,
                Mathf.Clamp(configuration.Amount, 0, m_configuration.MaxAttractParticlesAmount),
                configuration.StartPosition,
                configuration.TargetPosition,
                prefab,
                configuration.ForceAmountType,
                configuration.AttractAction,
                configuration.EndAction,
                configuration.EmitType,
                configuration.Movement,
                configuration.UpdateMode,
                configuration.MinMaxSize,
                configuration.Delay
            );
        }

        private Transform GetCanvasTransform()
        {
            if (m_canvasTransform != null)
                return m_canvasTransform;
            m_canvasTransform = Object.Instantiate(m_configuration.CanvasPrefab).transform;
            m_canvasTransform.name = "UIParticleEffectsService Transform";
            Object.DontDestroyOnLoad(m_canvasTransform.gameObject);
            return m_canvasTransform;
        }
    }
}