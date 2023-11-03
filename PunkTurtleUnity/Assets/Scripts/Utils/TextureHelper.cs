using System.IO;
using UnityEngine;

namespace Utils
{
    public static class TextureHelper
    {
        public static void SaveRenderTexture(RenderTexture sourceRenderTexture, string savePath)
        {
            var tempTexture = new Texture2D(sourceRenderTexture.width, sourceRenderTexture.height, TextureFormat.ARGB32, false);
            RenderTexture.active = sourceRenderTexture;
            tempTexture.ReadPixels(new Rect(0, 0, sourceRenderTexture.width, sourceRenderTexture.height), 0, 0);
            tempTexture.Apply();
            RenderTexture.active = null;

            var textureBytes = tempTexture.EncodeToPNG();
            File.WriteAllBytes(savePath, textureBytes);
        }

        public static void LoadImageToRenderTexture(ref RenderTexture destinationRenderTexture, string loadPath)
        {
            var loadedTexture = new Texture2D(destinationRenderTexture.width, destinationRenderTexture.height);
            var imageBytes = File.ReadAllBytes(loadPath);
            loadedTexture.LoadImage(imageBytes);
            
            Graphics.Blit(loadedTexture, destinationRenderTexture);
        }
    }
}