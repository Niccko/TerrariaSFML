﻿using System;
using System.IO.IsolatedStorage;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using t4ccer.Noisy;

namespace TerrariaSFML
{
    class World : Transformable, Drawable
    {
        public int WorldWidth { get; }
        public int WorldHeight { get; }

        private const int DrawBuffer = 2;

        private Tile[,] tiles;

        //public LightRenderer LightRenderer;
        public static Random Rand { private set; get; }

        public readonly Camera Camera;

        private readonly Perlin _noise = new();

        public World()
        {
            WorldWidth = 1200;
            WorldHeight = 600;
            tiles = new Tile[WorldWidth, WorldHeight];
            
            Camera = new Camera(new Vector2f(WorldWidth * Tile.TileSize / 2, WorldHeight * Tile.TileSize / 2), this);
        }

        public void GenerateWorld(int seed = -1)
        {
            Rand = seed >= 0 ? new Random(seed) : new Random((int) DateTime.Now.Ticks);

            for (int i = 0; i < WorldWidth; i++)
            {
                var off = (int) (20 * _noise.OctavePerlin((float) i / 15, 100, 100, 1, 1));

                for (var j = WorldHeight / 2 - off; j < WorldHeight / 2 + 50; j++)
                {
                    SetTile(TileType.Ground, i, j);
                }

                off = (int) (20 * _noise.OctavePerlin((float) i / 15, 200, 100, 1, 1));

                for (var j = WorldHeight / 2 + 50 - off; j < WorldHeight; j++)
                {
                    SetTile(TileType.Stone, i, j);
                }
            }
        }

        public void SetTile(TileType type, int i, int j)
        {
            var upTile = GetTile(i, j - 1);
            var downTile = GetTile(i, j + 1);
            var leftTile = GetTile(i - 1, j);
            var rightTile = GetTile(i + 1, j);
            if (type != TileType.None)
            {
                var tile = new Tile(type, upTile, rightTile, downTile, leftTile)
                {
                    Position = new Vector2f(i * Tile.TileSize, j * Tile.TileSize) + Position
                };
                tiles[i, j] = tile;
            }
            else
            {
                tiles[i, j] = null;
                if (upTile != null) upTile[2] = null;
                if (rightTile != null) rightTile[3] = null;
                if (downTile != null) downTile[0] = null;
                if (leftTile != null) leftTile[1] = null;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            AmbienceRenderer.DrawBackground(BackgroundType.Basic);

            var lu = (Vector2i) ((Camera.View.Center - Camera.View.Size / 2) / Tile.TileSize);
            var rd = (Vector2i) ((Camera.View.Center + Camera.View.Size / 2) / Tile.TileSize);

            for (var i = Math.Max(0, lu.X - DrawBuffer); i < Math.Min(WorldWidth, rd.X + DrawBuffer); i++)
            {
                for (var j = Math.Max(0, lu.Y - DrawBuffer); j < Math.Min(WorldHeight, rd.Y + DrawBuffer); j++)
                {
                    if (tiles[i, j] != null)
                        target.Draw(tiles[i, j]);
                }
            }

            //LightRenderer.DrawLightSurface();
        }

        public Tile GetTile(int i, int j)
        {
            if (i >= 0 && j >= 0 && i < WorldWidth && j < WorldHeight)
                return tiles[i, j];

            return null;
        }
    }
}