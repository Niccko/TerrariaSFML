using System;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public class Content
    {
        private const string ContentDir = "..\\..\\Content\\";

        public static SpriteSheet TexTileGround;
        public static Texture TexBasicBack;
        public static Texture TexMountainsBack;
        public static Texture TexBackground55;
        public static SpriteSheet TexTileGrass;
        public static SpriteSheet TexTiles9;

        public static void Load()
        {
            TexTileGround = new SpriteSheet(Tile.TileSize, Tile.TileSize, 1, new Texture(ContentDir + "Tiles_0.png"),
                false);
            TexBasicBack = new Texture(ContentDir + "Background_0.png"){Repeated = true};
            TexMountainsBack = new Texture(ContentDir + "Background_116.png") {Repeated = true};
            TexBackground55 = new Texture(ContentDir + "Background_55.png") {Repeated = true};
            TexTileGrass = new SpriteSheet(Tile.TileSize, Tile.TileSize, 1, new Texture(ContentDir + "Tiles_2.png"),false);
            TexTiles9 = new SpriteSheet(Tile.TileSize, Tile.TileSize, 1, new Texture(ContentDir + "Tiles_9.png"),false);
        }
    }
}