using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    internal enum BackgroundType
    {
        Basic
    }

    internal static class AmbienceRenderer
    {
        private const int WorldWidth = 300 * Tile.TileSize;
        private const int WorldHeight = 100 * Tile.TileSize;

        private static Sprite CreateBackSprite(Texture tex)
        {
            return new(tex, new IntRect(new Vector2i(0, 0), new Vector2i(WorldWidth*Tile.TileSize, (int)tex.Size.Y)));
        }

        public static void DrawBackground(BackgroundType type)
        {
            var windowSize = (Vector2f) Program.Window.Size;
            var layers = new List<Sprite>();

            switch (type)
            {
                case BackgroundType.Basic:
                    layers.Add(CreateBackSprite(Content.TexBasicBack));
                    layers.Add(CreateBackSprite(Content.TexMountainsBack));
                    //layers.Add(CreateBackSprite(Content.TexBackground55));
                    break;
            }

            foreach (var layer in layers)
            {
                var scale = WorldHeight*Tile.TileSize / layer.Texture.Size.Y;
                layer.Scale = new Vector2f(scale, scale);
                Program.Window.Draw(layer);
            }
        }
    }
}