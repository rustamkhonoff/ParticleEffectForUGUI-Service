using System.Collections.Generic;
using UnityEngine;

namespace UIParticle.Service
{
    public class UIParticleTextureInfo
    {
        public bool IsTextureSheetMode { get; internal set; }
        public ParticleSystemAnimationMode Mode { get; internal set; } = ParticleSystemAnimationMode.Sprites;
        public Vector2Int GridTilesSize { get; internal set; }
        public Texture DefaultTexture { get; internal set; }
        public List<Sprite> Sprites { get; internal set; }
        public Material Material { get; internal set; }
    }
}