using System;
using Tools;
using UnityEngine;

namespace Manager
{
    class TextureManager : Singleton<TextureManager>
    {
        public void Init()
        {

        }

        public static RenderTexture CreateTexture(int width, int height)
        {
            return new RenderTexture(width, height, 24);
        }
    }
}
