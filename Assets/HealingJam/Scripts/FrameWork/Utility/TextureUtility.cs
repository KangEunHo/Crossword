using UnityEngine;
namespace HealingJam
{
    public static class TextureUtilities
    {
        public static Texture2D CopyTexture(Texture2D origin, RectInt rectInt)
        {
            Texture2D texture2D = new Texture2D(rectInt.width, rectInt.height);
            Color[] pixelBuffer = origin.GetPixels(rectInt.x, rectInt.x, rectInt.width, rectInt.height);
            texture2D.SetPixels(rectInt.x, rectInt.y, rectInt.width, rectInt.height, pixelBuffer);
            texture2D.Apply();
            return texture2D;
        }

        public static Texture2D CopyTextureReverse(Texture2D origin, RectInt rectInt)
        {
            Texture2D texture2D = new Texture2D(rectInt.width, rectInt.height);
            Color[] pixelBuffer = origin.GetPixels(rectInt.x, origin.height - (rectInt.y + rectInt.height), rectInt.width, rectInt.height);
            texture2D.SetPixels(0, 0, rectInt.width, rectInt.height, pixelBuffer);
            texture2D.Apply();
            return texture2D;
        }

        public static void SetOneColor(Texture2D texture, Color color)
        {
            int w = texture.width;
            int h = texture.height;

            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
        }

        public static Color[,] OneDimensionalColorToTwoDimensionalColor(ref Color[] colors, int width, int height)
        {
            Color[,] newColors = new Color[height, width];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    newColors[y, x] = colors[y * width + x];
                }
            }
            return newColors;
        }

        public static Sprite ConvertToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}