using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

        private Dictionary<ForceAmountType, (Vector2 minMaxSpeed, float drag)> m_forceDict = new()
        {
            { ForceAmountType.Small, (new Vector2(0, 50), 0.007f) },
            { ForceAmountType.Medium, (new Vector2(0, 500), 0.005f) },
            { ForceAmountType.Big, (new Vector2(0, 1000), 0.003f) }
        };

        public void Initialize(Dictionary<ForceAmountType, (Vector2 minMaxSpeed, float drag)> forces)
        {
            m_forceDict = forces;
        }

        public void Attract(UIParticleTextureData textureData, int amount, Vector3 startScreenSpacePos,
            Vector3 targetScreenSpacePos,
            ParticleSystem particlePrefab,
            ForceAmountType forceAmountType,
            Action onAttracted = default, Action onDestroying = default,
            ParticleAttractEmitType particleAttractEmitType = ParticleAttractEmitType.Delayed,
            UIParticleAttractor.Movement movement = UIParticleAttractor.Movement.Smooth,
            UIParticleAttractor.UpdateMode updateMode = UIParticleAttractor.UpdateMode.Normal,
            Vector2 minMaxSize = default, float delay = -1f, Func<Vector3> configurationAttractorFollowPosition = null)
        {
            Destroying += onDestroying;

            if (amount < 0)
            {
                Destroy(gameObject);
                Debug.Log("Trying to create attract particles with count 0, aborting");
                return;
            }

            ParticleSystem particleInstance = Instantiate(particlePrefab);

            SetupTextures(particleInstance, textureData);
            SetupParticleMaterial(particleInstance,
                CreateMaterialFor(textureData.DefaultTexture, textureData.Material));
            SetupStartPosition(startScreenSpacePos);
            SetupUIParticle(particleInstance);
            SetAttractorParticle(particleInstance);
            SetupAttractorPosition(targetScreenSpacePos);
            SetupEvents(onAttracted);
            SetupMovement(movement);
            SetupUpdate(updateMode);
            SetupParticleSizes(particleInstance, minMaxSize);
            SetupParticleForce(particleInstance, forceAmountType);
            SetupAttractDelay(delay);

            if (configurationAttractorFollowPosition is not null)
                FollowPosition.Create(_particleAttractor.transform, configurationAttractorFollowPosition);

            ParticleEventHandler.Create(particleInstance).ParticleStopped += HandleParticlesStop;

            AnimateEmit(particleAttractEmitType, amount, particleInstance);
        }

        private void SetupAttractDelay(float delay)
        {
            if (delay <= 0f)
                return;

            _particleAttractor.delay = delay;
        }


        private void SetupParticleForce(ParticleSystem particleInstance, ForceAmountType forceAmountType)
        {
            (Vector2 minMaxSpeed, float drag) data = m_forceDict[forceAmountType];

            ParticleSystem.MainModule main = particleInstance.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(data.minMaxSpeed.x, data.minMaxSpeed.y);
            ParticleSystem.LimitVelocityOverLifetimeModule velocity = particleInstance.limitVelocityOverLifetime;
            velocity.drag = data.drag;
        }

        private void SetupParticleSizes(ParticleSystem particleInstance, Vector2 minMax)
        {
            if (minMax == default)
                return;

            ParticleSystem.MainModule main = particleInstance.main;
            main.startSize = new ParticleSystem.MinMaxCurve(minMax.x, minMax.y);
        }

        private void SetupTextures(ParticleSystem particle, UIParticleTextureData textureData)
        {
            if (!textureData.IsTextureSheetMode)
                return;

            ParticleSystem.TextureSheetAnimationModule sheetAnimation = particle.textureSheetAnimation;
            sheetAnimation.enabled = true;
            sheetAnimation.mode = textureData.Mode;
            if (textureData.Mode == ParticleSystemAnimationMode.Grid)
            {
                sheetAnimation.numTilesX = textureData.GridTilesSize.x;
                sheetAnimation.numTilesY = textureData.GridTilesSize.y;
            }
            else
            {
                foreach (Sprite textureDataSprite in textureData.Sprites)
                    sheetAnimation.AddSprite(textureDataSprite);
            }
        }

        private void SetupMovement(UIParticleAttractor.Movement movement)
        {
            Type type = _particleAttractor.GetType();

            FieldInfo field = type.GetField("m_Movement", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null) throw new Exception($"Can't access m_Movement field in {type}");

            field.SetValue(_particleAttractor, movement);
        }

        private void SetupUpdate(UIParticleAttractor.UpdateMode updateMode)
        {
            Type type = _particleAttractor.GetType();

            FieldInfo field = type.GetField("m_UpdateMode", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null) throw new Exception($"Can't access m_UpdateMode field in {type}");

            field.SetValue(_particleAttractor, updateMode);
        }

        private void AnimateEmit(ParticleAttractEmitType particleAttractEmitType, int amount,
            ParticleSystem particleInstance)
        {
            IEnumerator IEDelayedEmit()
            {
                WaitForSeconds waitForSeconds = new(0.01f);
                for (int i = 0; i < amount; i++)
                {
                    particleInstance.Emit(1);
                    yield return waitForSeconds;
                }
            }

            void AtOnce()
            {
                particleInstance.Emit(amount);
            }

            switch (particleAttractEmitType)
            {
                case ParticleAttractEmitType.Delayed:
                    StartCoroutine(IEDelayedEmit());
                    break;
                case ParticleAttractEmitType.AllInOnce:
                    AtOnce();
                    break;
                default:
                    break;
            }
        }

        private void SetupEvents(Action onAttractedAction)
        {
            Type type = _particleAttractor.GetType();

            FieldInfo field = type.GetField("m_OnAttracted", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null) throw new Exception($"Can't access m_OnAttracted field in {type}");

            UnityEvent unityEvent = field.GetValue(_particleAttractor) as UnityEvent;
            if (onAttractedAction == null)
                return;
            unityEvent?.AddListener(new UnityAction(onAttractedAction));
        }

        private void SetupStartPosition(Vector3 startScreenSpacePos)
        {
            transform.position = startScreenSpacePos;
        }

        private void HandleParticlesStop()
        {
            Destroy(gameObject);
        }

        private void SetupAttractorPosition(Vector3 targetScreenSpacePos)
        {
            _particleAttractor.transform.position = targetScreenSpacePos;
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
            Type type = _particleAttractor.GetType();

            FieldInfo field = type.GetField("m_ParticleSystem", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null) throw new Exception($"Can't access m_ParticleSystem field in {type}");

            field.SetValue(_particleAttractor, ps);
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