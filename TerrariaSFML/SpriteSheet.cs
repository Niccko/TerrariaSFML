using System;
using SFML.Graphics;

namespace TerrariaSFML
{
    public class SpriteSheet
    {
        public int SpriteWidth { get; private set; }
        public int SpriteHeight { get; private set; }
        public int CountX { get; private set; }
        public int CountY { get; private set; }

        public Texture Texture;

        private int borderSize;

        public SpriteSheet(int wCount, int hCount, int borderWidth, Texture texture, bool byCount)
        {
            Texture = texture;
            borderSize = borderWidth > 0 ? borderWidth + 1 : borderWidth;
            if (byCount)
            {
                SpriteWidth = (int) Math.Ceiling((float) texture.Size.X / wCount);
                SpriteHeight = (int) Math.Ceiling((float) texture.Size.Y / hCount);
                CountX = wCount;
                CountY = hCount;
            }
            else
            {
                SpriteWidth = wCount;
                SpriteHeight = hCount;
                CountX = (int) Math.Ceiling((float) texture.Size.X / wCount);
                CountY = (int) Math.Ceiling((float) texture.Size.Y / hCount);
            }
        }

        public IntRect GetTextureRect(int i, int j)
        {
            int x = i * (SpriteWidth + borderSize);
            int y = j * (SpriteHeight + borderSize);
            return new IntRect(x, y, SpriteWidth, SpriteHeight);
        }
    }
}