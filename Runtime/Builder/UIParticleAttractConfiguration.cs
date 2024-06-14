#if COFFEE_PARTICLES
using System;
using System.Collections.Generic;
using System.Linq;
using Coffee.UIExtensions;
using UnityEngine;

namespace UGUIParticleEffect.Builder
{
    [Serializable]
    public class UIParticleAttractConfiguration
    {
        public Texture Texture;
        public int Amount;
        public Vector3 TargetPosition;
        public Action EndAction;
        public Action AttractAction;
        public ParticleAttractEmitType EmitType = ParticleAttractEmitType.Delayed;
        public Vector3 StartPosition = new(Screen.width / 2f, Screen.height / 2f);
        public UIParticleAttractor.Movement Movement;
        public UIParticleAttractor.UpdateMode UpdateMode;
        public Vector2Int TextureSheetSize;
        public List<Sprite> TextureSheetSprites;
        public ParticleSystemAnimationMode AnimationMode;
        public bool TextureSheetEnabled;
        public Vector2 MinMaxSize = Vector2.zero;
        public ParticleSystem CustomPrefab;
        public ForceAmountType ForceAmountType = ForceAmountType.Small;
        public float Delay = 0.3f;

        public UIParticleAttractConfiguration(Texture texture, int amount, Vector3 targetPosition)
        {
            Texture = texture;
            Amount = amount;
            TargetPosition = targetPosition;
        }


        public UIParticleAttractConfiguration(Sprite sprite, int amount, Vector3 targetPosition)
        {
            Texture = sprite.texture;
            Amount = amount;
            TargetPosition = targetPosition;
        }

        public UIParticleAttractConfiguration(Texture texture, Vector2Int textureSheetSize, int amount,
            Vector3 targetPosition)
        {
            TextureSheetEnabled = true;
            TextureSheetSize = textureSheetSize;
            AnimationMode = ParticleSystemAnimationMode.Grid;
            Texture = texture;
            Amount = amount;
            TargetPosition = targetPosition;
        }

        public UIParticleAttractConfiguration(IEnumerable<Sprite> sprites, int amount, Vector3 targetPosition)
        {
            TextureSheetEnabled = true;
            AnimationMode = ParticleSystemAnimationMode.Sprites;
            TextureSheetSprites = sprites.ToList();
            Amount = amount;
            TargetPosition = targetPosition;
        }

        public UIParticleAttractConfiguration WithDelay(float delay)
        {
            Delay = delay;
            return this;
        }

        public UIParticleAttractConfiguration WithForce(ForceAmountType forceAmountType)
        {
            ForceAmountType = forceAmountType;
            return this;
        }

        public UIParticleAttractConfiguration WithMinMaxSize(Vector2 vector2)
        {
            MinMaxSize = vector2;
            return this;
        }

        public UIParticleAttractConfiguration WithCustomParticlePrefab(ParticleSystem prefab)
        {
            CustomPrefab = prefab;
            return this;
        }

        public UIParticleAttractConfiguration WithEndAction(Action endAction)
        {
            EndAction = endAction;
            return this;
        }


        public UIParticleAttractConfiguration WithMovement(UIParticleAttractor.Movement movement)
        {
            Movement = movement;
            return this;
        }

        public UIParticleAttractConfiguration WithUpdate(UIParticleAttractor.UpdateMode updateMode)
        {
            UpdateMode = updateMode;
            return this;
        }


        public UIParticleAttractConfiguration WithAttractAction(Action attractAction)
        {
            AttractAction = attractAction;
            return this;
        }

        public UIParticleAttractConfiguration WithStartPosition(Vector3 startPosition)
        {
            StartPosition = startPosition;
            return this;
        }

        public UIParticleAttractConfiguration WithEmitType(ParticleAttractEmitType emitType)
        {
            EmitType = emitType;
            return this;
        }
    }
}
#endif