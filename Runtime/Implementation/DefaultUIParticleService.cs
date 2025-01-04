using System;
using System.Runtime.CompilerServices;
using Coffee.UIExtensions;
using UIParticle.Service.Extras;
using UnityEngine;
using Object = UnityEngine.Object;

[assembly: InternalsVisibleTo("UIParticleEffectService.Zenject")]

namespace UIParticle.Service
{
    internal class DefaultUIParticleService : IUIParticleService
    {
        private readonly UIParticlesEffectsConfiguration m_configuration;

        private Transform m_canvasTransform;

        public DefaultUIParticleService(UIParticlesEffectsConfiguration configuration)
        {
            SpriteExtensions.Dispose();

            m_configuration = configuration;
        }

        public void Attract(UIParticleConfiguration configuration,
            Action<Coffee.UIExtensions.UIParticle> configureUIParticle = null,
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null)
        {
            UIParticleAttractorView view = Object.Instantiate(m_configuration.ParticleAttractorViewPrefab, GetCanvasTransform());
            UIParticleTextureInfo textureInfo = new()
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
            view.Attract(textureInfo, configuration, configureUIParticle, configureParticle, configureAttractor);
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