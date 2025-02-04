using System;
using Coffee.UIExtensions;
using UIParticle.Service.Extras;
using UnityEngine;
using UnityEngine.Events;

namespace UIParticle.Service
{
    public class UIParticleAttractorView : MonoBehaviour
    {
        public event Action Destroying;

        [SerializeField] private UIParticleAttractor _particleAttractor;
        [SerializeField] private Coffee.UIExtensions.UIParticle _uiParticle;

        private Camera m_camera;
        private int m_attractCount;

        public void Attract(UIParticleTextureInfo textureInfo, UIParticleConfiguration configuration,
            Action<Coffee.UIExtensions.UIParticle> configureUIParticle = null,
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
            Material material = CreateMaterialFor(textureInfo.DefaultTexture, textureInfo.Material);

            SetupTextureSheetMode(particleInstance, textureInfo);
            SetupParticleMaterial(particleInstance, material);
            SetupUIParticle(particleInstance);
            SetAttractorParticle(particleInstance);
            SetupStartPosition(configuration.StartPosition);
            SetupAttractorPosition(configuration.TargetPosition);
            SetupEvents(configuration.FirstAttractAction, configuration.AttractAction);

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
                PositionInfo.SpaceType.World => m_camera.WorldToScreenPoint(info.PositionFunc()),
                PositionInfo.SpaceType.UI => info.PositionFunc(),
                _ => Vector3.zero
            };
        }

        private void SetupTextureSheetMode(ParticleSystem particle, UIParticleTextureInfo textureInfo)
        {
            if (!textureInfo.IsTextureSheetMode) return;

            ParticleSystem.TextureSheetAnimationModule sheetAnimation = particle.textureSheetAnimation;
            sheetAnimation.enabled = true;
            sheetAnimation.mode = textureInfo.Mode;
            if (textureInfo.Mode == ParticleSystemAnimationMode.Grid)
                (sheetAnimation.numTilesX, sheetAnimation.numTilesY) = (textureInfo.GridTilesSize.x, textureInfo.GridTilesSize.y);
            else
                textureInfo.Sprites.ForEach(sheetAnimation.AddSprite);
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

        private void SetupEvents(Action firstAttract, Action onAttractedAction)
        {
            if (onAttractedAction == null) return;
            _particleAttractor.onAttracted.AddListener(() => HandleAttracted(firstAttract, onAttractedAction));
        }

        private void HandleAttracted(Action firstAttractAction, Action otherAction)
        {
            if (m_attractCount == 0)
                firstAttractAction?.Invoke();

            m_attractCount++;
            otherAction?.Invoke();
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
            _particleAttractor.AddParticleSystem(ps);
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