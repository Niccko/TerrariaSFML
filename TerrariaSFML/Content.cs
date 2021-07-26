using System;
using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public static class Content
    {
        private const string ContentDir = "..\\..\\Content\\";

        public static Dictionary<int,SpriteSheet> Tiles;
        
        public static Texture TexBasicBack;
        public static Texture TexMountainsBack;
        public static Texture TexBackground55;


        public static void Load()
        {
            Tiles = new Dictionary<int, SpriteSheet>();
            Tiles.Add(0,new SpriteSheet(Tile.TileSize, Tile.TileSize, 1, new Texture(ContentDir + "Tiles_0.png"),
                false));
            Tiles.Add(2,new SpriteSheet(Tile.TileSize, Tile.TileSize, 1, new Texture(ContentDir + "Tiles_2.png"),
                false));
            Tiles.Add(9,new SpriteSheet(Tile.TileSize, Tile.TileSize, 1, new Texture(ContentDir + "Tiles_9.png"),
                false));
            
            TexBasicBack = new Texture(ContentDir + "Background_0.png"){Repeated = true};
            TexMountainsBack = new Texture(ContentDir + "Background_116.png") {Repeated = true};
            TexBackground55 = new Texture(ContentDir + "Background_55.png") {Repeated = true};
            
        }
    }
}