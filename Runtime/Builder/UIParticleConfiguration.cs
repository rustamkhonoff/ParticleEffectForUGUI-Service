using System;
using System.Collections.Generic;
using System.Linq;
using UGUIParticleEffect.Implementation;
using UnityEngine;

namespace UGUIParticleEffect.Builder
{
    [Serializable]
    public class EmittingInfo
    {
        [field: SerializeField] public float Delay { get; internal set; }
        [field: SerializeField] public int Amount { get; internal set; }
    }
    [Serializable]
    public class PositionInfo
    {
        public Func<Vector3> PositionFunc { get; }
        public SpaceType Space { get; }
        public bool UpdatePositionOnUpdate { get; }

        public enum SpaceType
        {
            World = 0,
            UI = 1
        }

        public PositionInfo(Vector3 vector3, SpaceType space = SpaceType.UI)
        {
            Space = space;
            PositionFunc = () => vector3;
        }

        public PositionInfo(Transform transform, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = false)
        {
            PositionFunc = () => transform.position;
            UpdatePositionOnUpdate = updatePositionOnUpdate;
            Space = space;
        }

        public PositionInfo(Func<Vector3> positionFunc, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = false)
        {
            PositionFunc = positionFunc;
            UpdatePositionOnUpdate = updatePositionOnUpdate;
            Space = space;
        }

        public override string ToString()
        {
            return $"Pos: {PositionFunc()}, Space: {Space}, Update: {UpdatePositionOnUpdate}";
        }

        public static PositionInfo ScreenCenter => new(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
    }

    [Serializable]
    public class UIParticleConfiguration
    {
        [field: SerializeField] public Texture Texture { get; internal set; }
        [field: SerializeField] public EmittingInfo EmittingInfo { get; internal set; } = new();
        [field: SerializeField] public PositionInfo TargetPosition { get; internal set; } = PositionInfo.ScreenCenter;
        [field: SerializeField] public PositionInfo StartPosition { get; internal set; } = PositionInfo.ScreenCenter;
        [field: SerializeField] public Vector2Int? TextureSheetSize { get; internal set; }
        [field: SerializeField] public List<Sprite> TextureSheetSprites { get; internal set; } = new();
        [field: SerializeField] public ParticleSystemAnimationMode AnimationMode { get; internal set; }
        [field: SerializeField] public bool TextureSheetEnabled { get; internal set; }
        [field: SerializeField] public ParticleSystem Prefab { get; internal set; }

        public Action EndAction { get; internal set; }
        public Action AttractAction { get; internal set; }

        public class Builder
        {
            private readonly UIParticleConfiguration m_configuration = new();

            public Builder(Texture texture, int amount, PositionInfo targetPosition)
            {
                m_configuration.Texture = texture;
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
            }

            public Builder(Sprite sprite, int amount, PositionInfo targetPosition, bool spriteIsMultipleSprite = false)
            {
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
                m_configuration.Texture = spriteIsMultipleSprite ? sprite.GetTexture() : sprite.texture;
            }


            public Builder(Texture texture, Vector2Int textureSheetSize, int amount, PositionInfo targetPosition)
            {
                m_configuration.TextureSheetEnabled = true;
                m_configuration.TextureSheetSize = textureSheetSize;
                m_configuration.AnimationMode = ParticleSystemAnimationMode.Grid;
                m_configuration.Texture = texture;
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
            }

            public Builder(IEnumerable<Sprite> sprites, int amount, PositionInfo targetPosition)
            {
                m_configuration.TextureSheetEnabled = true;
                m_configuration.AnimationMode = ParticleSystemAnimationMode.Sprites;
                m_configuration.TextureSheetSprites = sprites.ToList();
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
            }

            public Builder WithTexture(Texture texture)
            {
                m_configuration.Texture = texture;
                return this;
            }

            public Builder WithTexture(Sprite sprite, bool spriteIsMultipleSprite = false)
            {
                m_configuration.Texture = spriteIsMultipleSprite ? sprite.GetTexture() : sprite.texture;
                return this;
            }

            public Builder WithAmount(int amount)
            {
                m_configuration.EmittingInfo.Amount = amount;
                return this;
            }

            public Builder WithEmitInfo(EmittingInfo emittingInfo)
            {
                m_configuration.EmittingInfo = emittingInfo;
                return this;
            }

            public Builder WithEmitDelay(float delay)
            {
                m_configuration.EmittingInfo.Delay = delay;
                return this;
            }

            public Builder WithTargetPosition(PositionInfo targetPosition)
            {
                m_configuration.TargetPosition = targetPosition;
                return this;
            }

            public Builder WithStartPosition(PositionInfo startPosition)
            {
                m_configuration.StartPosition = startPosition;
                return this;
            }

            public Builder WithTextureSheetSize(Vector2Int textureSheetSize)
            {
                m_configuration.TextureSheetSize = textureSheetSize;
                return this;
            }

            public Builder WithTextureSheetSprites(List<Sprite> textureSheetSprites)
            {
                m_configuration.TextureSheetSprites = textureSheetSprites;
                return this;
            }

            public Builder WithAnimationMode(ParticleSystemAnimationMode animationMode)
            {
                m_configuration.AnimationMode = animationMode;
                return this;
            }

            public Builder WithTextureSheetEnabled(bool textureSheetEnabled)
            {
                m_configuration.TextureSheetEnabled = textureSheetEnabled;
                return this;
            }

            public Builder WithCustomPrefab(ParticleSystem customPrefab)
            {
                m_configuration.Prefab = customPrefab;
                return this;
            }

            public Builder WithEndAction(Action endAction)
            {
                m_configuration.EndAction = endAction;
                return this;
            }

            public Builder WithAttractAction(Action attractAction)
            {
                m_configuration.AttractAction = attractAction;
                return this;
            }

            public UIParticleConfiguration Build()
            {
                return m_configuration;
            }
        }
    }
}