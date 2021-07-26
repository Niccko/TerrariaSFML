using System;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public class World : Transformable, Drawable
    {
        public int WorldWidth { get; }
        public int WorldHeight { get; }

        private const int DrawBuffer = 2;

        private Tile[,] _tiles;

        public static Random Rand { private set; get; }

        public readonly Camera Camera;

        private readonly Perlin _noise = new();


        public World(int width, int height)
        {
            WorldWidth = width;
            WorldHeight = height;
            _tiles = new Tile[WorldWidth, WorldHeight];
            Camera = new Camera(new Vector2f(WorldWidth * Tile.TileSize / 2f, WorldHeight * Tile.TileSize / 2f), this);
            Lighter.Init(this);
            
        }

        public void GenerateWorld(int seed = -1)
        {
            seed = seed >= 0 ? seed : (int) DateTime.Now.Ticks;
            Rand = new Random(seed);

            for (int i = 0; i < WorldWidth; i++)
            {
                for (var j = WorldHeight / 2; j < WorldHeight / 2 + 50; j++)
                {
                    SetTile(TileType.Ground, i, j);
                }

                var off = (int) (20 * _noise.OctavePerlin((float) i / 15, seed, 100, 1, 1));

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
                _tiles[i, j] = tile;
            }
            else
            {
                _tiles[i, j] = null;
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
                    if (_tiles[i, j] != null)
                        target.Draw(_tiles[i, j]);
                }
            }

            if(Program.frameCount%2==0)
                Lighter.UpdateLightTexture();
                
            
            Lighter.DrawLightTexture();
        }

        public Tile GetTile(int i, int j)
        {
            if (i >= 0 && j >= 0 && i < WorldWidth && j < WorldHeight)
                return _tiles[i, j];

            return null;
        }
    }
}