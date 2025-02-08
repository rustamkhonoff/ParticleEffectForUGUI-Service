using System;
using System.Collections.Generic;
using System.Linq;
using UIParticle.Service.Extras;
using UnityEngine;

namespace UIParticle.Service
{
    [Serializable]
    public class UIParticleConfiguration
    {
        /// <summary>
        /// Base texture used for the particle system.
        /// </summary>
        [field: SerializeField] public Texture Texture { get; internal set; }

        /// <summary>
        /// Contains information about emission, such as delay and amount of particles.
        /// </summary>
        [field: SerializeField] public EmittingInfo EmittingInfo { get; internal set; } = new(0f, 0);

        /// <summary>
        /// The target position where particles are directed.
        /// </summary>
        [field: SerializeField] public PositionInfo TargetPosition { get; internal set; } = PositionInfo.ScreenCenter;

        /// <summary>
        /// The initial spawn position of particles.
        /// </summary>
        [field: SerializeField] public PositionInfo StartPosition { get; internal set; } = PositionInfo.ScreenCenter;

        /// <summary>
        /// Defines the texture sheet size for grid-based animation.
        /// </summary>
        [field: SerializeField] public Vector2Int? TextureSheetSize { get; internal set; }

        /// <summary>
        /// A collection of sprites used for randomized emission. All sprites must be part of the same sprite atlas.
        /// </summary>
        [field: SerializeField] public List<Sprite> TextureSheetSprites { get; internal set; } = new();

        [field: SerializeField] public ParticleSystemAnimationMode AnimationMode { get; internal set; }

        [field: SerializeField] public bool TextureSheetEnabled { get; internal set; }

        /// <summary>
        /// The particle system prefab used for instantiation.
        /// </summary>
        [field: SerializeField] public ParticleSystem Prefab { get; internal set; }

        /// <summary>
        /// Invoked when the particle system stops emitting.
        /// </summary>
        public Action EndAction { get; internal set; }

        /// <summary>
        /// Invoked every time a particle is attracted.
        /// </summary>
        public Action AttractAction { get; internal set; }

        /// <summary>
        /// Invoked once when the first particle is attracted.
        /// </summary>
        public Action FirstAttractAction { get; internal set; }

        /// <summary>
        /// A builder class to create instances of UIParticleConfiguration.
        /// </summary>
        public class Builder
        {
            private readonly UIParticleConfiguration m_configuration = new();

            /// <summary>
            /// Basic constructor for particles using the same texture.
            /// </summary>
            public Builder(Texture texture, int amount, PositionInfo targetPosition)
            {
                m_configuration.Texture = texture;
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
            }

            /// <summary>
            /// Constructor for particles using a sprite, optionally from a sprite atlas.
            /// </summary>
            public Builder(Sprite sprite, int amount, PositionInfo targetPosition, bool spriteIsMultipleSprite = false)
            {
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
                m_configuration.Texture = spriteIsMultipleSprite ? sprite.GetTexture() : sprite.texture;
            }

            /// <summary>
            /// Constructor for animated particles using a sprite sheet.
            /// </summary>
            public Builder(Texture texture, Vector2Int textureSheetSize, int amount, PositionInfo targetPosition)
            {
                m_configuration.TextureSheetEnabled = true;
                m_configuration.TextureSheetSize = textureSheetSize;
                m_configuration.AnimationMode = ParticleSystemAnimationMode.Grid;
                m_configuration.Texture = texture;
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
            }

            /// <summary>
            /// Constructor for randomized particles using different sprites from the same sprite atlas.
            /// </summary>
            public Builder(IEnumerable<Sprite> sprites, int amount, PositionInfo targetPosition)
            {
                m_configuration.TextureSheetEnabled = true;
                m_configuration.AnimationMode = ParticleSystemAnimationMode.Sprites;
                m_configuration.TextureSheetSprites = sprites.ToList();
                m_configuration.EmittingInfo.Amount = amount;
                m_configuration.TargetPosition = targetPosition;
            }

            /// <summary>
            /// Sets the base texture.
            /// </summary>
            public Builder WithTexture(Texture texture)
            {
                m_configuration.Texture = texture;
                return this;
            }

            /// <summary>
            /// Sets the texture from a sprite, optionally from a sprite atlas.
            /// </summary>
            public Builder WithTexture(Sprite sprite, bool spriteIsMultipleSprite = false)
            {
                m_configuration.Texture = spriteIsMultipleSprite ? sprite.GetTexture() : sprite.texture;
                return this;
            }

            /// <summary>
            /// Sets the number of particles to emit.
            /// </summary>
            public Builder WithAmount(int amount)
            {
                m_configuration.EmittingInfo.Amount = amount;
                return this;
            }

            /// <summary>
            /// Sets emission details.
            /// </summary>
            public Builder WithEmitInfo(EmittingInfo emittingInfo)
            {
                m_configuration.EmittingInfo = emittingInfo;
                return this;
            }

            /// <summary>
            /// Sets the delay between particle emissions.
            /// </summary>
            public Builder WithEmitDelay(float delay)
            {
                m_configuration.EmittingInfo.Delay = delay;
                return this;
            }

            /// <summary>
            /// Sets the target position where particles should move.
            /// </summary>
            public Builder WithTargetPosition(PositionInfo targetPosition)
            {
                m_configuration.TargetPosition = targetPosition;
                return this;
            }

            /// <summary>
            /// Sets the starting position of particles.
            /// </summary>
            public Builder WithStartPosition(PositionInfo startPosition)
            {
                m_configuration.StartPosition = startPosition;
                return this;
            }

            /// <summary>
            /// Assigns a callback when the first particle is attracted.
            /// </summary>
            public Builder WithFirstAttractCallback(Action action)
            {
                m_configuration.FirstAttractAction = action;
                return this;
            }

            /// <summary>
            /// Assigns a callback when the particle system stops emitting.
            /// </summary>
            public Builder WithEndCallback(Action endAction)
            {
                m_configuration.EndAction = endAction;
                return this;
            }

            /// <summary>
            /// Assigns a callback for each time a particle is attracted.
            /// </summary>
            public Builder WithAttractCallback(Action attractAction)
            {
                m_configuration.AttractAction = attractAction;
                return this;
            }

            /// <summary>
            /// Builds and returns the configured UIParticleConfiguration instance.
            /// </summary>
            public UIParticleConfiguration Build()
            {
                return m_configuration;
            }
        }
    }
}