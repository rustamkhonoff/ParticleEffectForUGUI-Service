using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGUIParticleEffect.Implementation
{
    public static class SpriteExtensions
    {
        private static Dictionary<Sprite, Texture2D> _spriteCache = new();

        public static void Dispose()
        {
            _spriteCache = new Dictionary<Sprite, Texture2D>();
        }

        public static Texture2D GetTexture(this Sprite sprite)
        {
            if (_spriteCache.ContainsKey(sprite))
            {
                return _spriteCache[sprite];
            }

            if (Math.Abs(sprite.rect.width - sprite.texture.width) < 0.001f && Math.Abs(sprite.rect.height - sprite.texture.height) < 0.001f) 
                return sprite.texture;
            
            int width = (int)sprite.rect.width;
            int height = (int)sprite.rect.height;

            int maxDimension = Mathf.Max(width, height);
            Texture2D newTexture = new(maxDimension, maxDimension);

            Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x,
                (int)sprite.rect.y,
                width,
                height);

            Color[] newPixels = new Color[maxDimension * maxDimension];
            for (int i = 0; i < newPixels.Length; i++)
            {
                newPixels[i] = new Color(0, 0, 0, 0);
            }

            int xOffset = (maxDimension - width) / 2;
            int yOffset = (maxDimension - height) / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int newX = x + xOffset;
                    int newY = y + yOffset;
                    newPixels[newY * maxDimension + newX] = pixels[y * width + x];
                }
            }

            newTexture.SetPixels(newPixels);
            newTexture.Apply();

            _spriteCache[sprite] = newTexture;

            return newTexture;

        }
    }
}