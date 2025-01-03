using System;
using Coffee.UIExtensions;
using UGUIParticleEffect.Builder;
using UnityEngine;
using UnityEngine.Events;

namespace UGUIParticleEffect
{
    public class UIParticleAttractorView : MonoBehaviour
    {
        public event Action Destroying;

        [SerializeField] private UIParticleAttractor _particleAttractor;
        [SerializeField] private UIParticle _uiParticle;

        private Camera m_camera;

        public void Attract(UIParticleTextureData textureData, UIParticleConfiguration configuration,
            Action<UIParticle> configureUIParticle = null,
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null)
        {
            Destroying += configuration.EndAction;

            if (configuration.EmittingInfo.Amount < 0)
            {
                Destroy(gameObject);
                Debug.Log("Trying to create attract particles with count 0, aborting");
                return;
            }

            m_camera = Camera.main;
            ParticleSystem particleInstance = Instantiate(configuration.Prefab);
            Material material = CreateMaterialFor(textureData.DefaultTexture, textureData.Material);

            SetupTextureSheetMode(particleInstance, textureData);
            SetupParticleMaterial(particleInstance, material);
            SetupUIParticle(particleInstance);
            SetAttractorParticle(particleInstance);
            SetupStartPosition(configuration.StartPosition);
            SetupAttractorPosition(configuration.TargetPosition);
            SetupEvents(configuration.AttractAction);

            configureUIParticle?.Invoke(_uiParticle);
            configureParticle?.Invoke(particleInstance);
            configureAttractor?.Invoke(_particleAttractor);

            ParticleEventHandler.Create(particleInstance, HandleParticlesStop);

            AnimateEmit(configuration.EmittingInfo, particleInstance);
        }

        private Vector3 GetPositionForInfo(PositionInfo info)
        {
            return info.Space switch
            {
                PositionInfo.SpaceType.World => m_camera!.ScreenToWorldPoint(info.PositionFunc()),
                PositionInfo.SpaceType.UI => info.PositionFunc(),
                _ => Vector3.zero
            };
        }

        private void SetupTextureSheetMode(ParticleSystem particle, UIParticleTextureData textureData)
        {
            if (!textureData.IsTextureSheetMode) return;

            ParticleSystem.TextureSheetAnimationModule sheetAnimation = particle.textureSheetAnimation;
            sheetAnimation.enabled = true;
            sheetAnimation.mode = textureData.Mode;
            if (textureData.Mode == ParticleSystemAnimationMode.Grid)
                (sheetAnimation.numTilesX, sheetAnimation.numTilesY) = (textureData.GridTilesSize.x, textureData.GridTilesSize.y);
            else
                textureData.Sprites.ForEach(sheetAnimation.AddSprite);
        }

        private void AnimateEmit(EmittingInfo emittingInfo, ParticleSystem particleInstance)
        {
            ParticleSystem.EmissionModule module = particleInstance.emission;
            module.burstCount = 1;
            bool delayed = emittingInfo.Delay > 0f;
            if (delayed)
            {
                ParticleSystem.MainModule main = particleInstance.main;
                main.duration += emittingInfo.Amount * emittingInfo.Delay;
            }
            short emitCount = (short)(delayed ? 1 : emittingInfo.Amount);
            short cycles = (short)(delayed ? emittingInfo.Amount : 1);
            float interval = emittingInfo.Delay;
            module.SetBurst(0, new ParticleSystem.Burst(0f, emitCount, _cycleCount: cycles, _repeatInterval: interval));
            particleInstance.Play();
        }

        private void SetupEvents(Action onAttractedAction)
        {
            if (onAttractedAction == null) return;
            _particleAttractor.onAttracted.AddListener(new UnityAction(onAttractedAction));
        }

        private void SetupStartPosition(PositionInfo info)
        {
            transform.position = GetPositionForInfo(info);
            if (info.UpdatePositionOnUpdate)
                FollowPosition.Create(transform.transform, () => GetPositionForInfo(info));
        }

        private void SetupAttractorPosition(PositionInfo info)
        {
            _particleAttractor.transform.position = GetPositionForInfo(info);
            if (info.UpdatePositionOnUpdate)
                FollowPosition.Create(_particleAttractor.transform, () => GetPositionForInfo(info));
        }

        private void HandleParticlesStop()
        {
            Destroy(gameObject);
        }


        private void SetupUIParticle(ParticleSystem particleInstance)
        {
            _uiParticle.SetParticleSystemInstance(particleInstance.gameObject);
        }

        private static void SetupParticleMaterial(ParticleSystem particleInstance, Material newMaterial)
        {
            particleInstance.GetComponent<ParticleSystemRenderer>().material = newMaterial;
        }

        private void SetAttractorParticle(ParticleSystem ps)
        {
            _particleAttractor.enabled = false;
            _particleAttractor.particleSystem = ps;
            _particleAttractor.enabled = true;
        }

        private Material CreateMaterialFor(Texture texture2D, Material referenceMaterial)
        {
            if (texture2D == null)
                return new Material(referenceMaterial);

            Material instance = new(referenceMaterial)
            {
                mainTexture = texture2D
            };
            instance.name += texture2D.name;
            return instance;
        }

        private void OnDestroy()
        {
            Destroying?.Invoke();
        }
    }
}