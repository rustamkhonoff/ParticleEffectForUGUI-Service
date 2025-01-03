using System;
using System.Runtime.CompilerServices;
using Coffee.UIExtensions;
using UGUIParticleEffect.Builder;
using UnityEngine;
using Object = UnityEngine.Object;

[assembly: InternalsVisibleTo("UIParticleEffectService.Zenject")]

namespace UGUIParticleEffect.Implementation
{
    internal class IuiParticlesService : IUIParticlesService
    {
        private readonly UIParticlesEffectsConfiguration m_configuration;

        private Transform m_canvasTransform;

        public IuiParticlesService(UIParticlesEffectsConfiguration configuration)
        {
            SpriteExtensions.Dispose();

            m_configuration = configuration;
        }

        public void Attract(UIParticleConfiguration configuration,
            Action<UIParticle> configureUIParticle = null,
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null)
        {
            UIParticleAttractorView view = Object.Instantiate(m_configuration.ParticleAttractorViewPrefab, GetCanvasTransform());
            UIParticleTextureData textureData = new()
            {
                IsTextureSheetMode = configuration.TextureSheetEnabled,
                Mode = configuration.AnimationMode,
                GridTilesSize = configuration.TextureSheetSize ?? new Vector2Int(),
                DefaultTexture = configuration.Texture,
                Sprites = configuration.TextureSheetSprites,
                Material = m_configuration.DefaultReferenceMaterial
            };

            configuration.EmittingInfo.Amount = Mathf.Clamp(configuration.EmittingInfo.Amount, 0, m_configuration.MaxAttractParticlesAmount);
            configuration.Prefab ??= m_configuration.DefaultParticleSystemPrefab;
            view.Attract(textureData, configuration, configureUIParticle, configureParticle, configureAttractor);
        }

        public void ClearAll()
        {
            for (int i = 0; i < m_canvasTransform.childCount; i++)
                Object.Destroy(m_canvasTransform.GetChild(i).gameObject);
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