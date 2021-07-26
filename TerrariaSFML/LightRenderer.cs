using System;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public static class Lighter
    {
        private static RenderTexture _texture;
        private static World _world;
        private static float[,] _lightValues;
        private static float _airDropoff = 0.9f;
        public static void Init(World world)
        {
            _texture = new RenderTexture(Program.Window.Size.X,Program.Window.Size.Y);
            _world = world;
            _lightValues = new float[_world.WorldWidth,_world.WorldHeight];
            for (int i = 0; i < _world.WorldWidth; i++)
            {
                for (int j = 0; j < _world.WorldHeight; j++)
                {
                    _lightValues[i, j] = 0;
                }
            }
        }

        public static void UpdateLightTexture()
        {
            _texture.Clear(Color.White);
            UpdateLights();
            var rect = new RectangleShape(new Vector2f(Tile.TileSize, Tile.TileSize));
            
                var lu = (Vector2i) ((_world.Camera.View.Center - _world.Camera.View.Size / 2) / Tile.TileSize);
            var rd = (Vector2i) ((_world.Camera.View.Center + _world.Camera.View.Size / 2) / Tile.TileSize);

            for (var i = Math.Max(0, lu.X); i < Math.Min(_world.WorldWidth, rd.X); i++)
            {
                for (var j = Math.Max(0, lu.Y); j < Math.Min(_world.WorldHeight, rd.Y); j++)
                {
                    var tile = _world.GetTile(i, j);
                    if (tile != null)
                    {
                        rect.Position = (Vector2f) Program.Window.MapCoordsToPixel(tile.Position);
                        byte color = (byte) (255 * _lightValues[i, j]);
                        rect.FillColor = new Color(color,color,color);
                        _texture.Draw(rect);
                    }
                }
            }
            _texture.Display();

        }

        public static void UpdateLights()
        {
            var lu = (Vector2i) ((_world.Camera.View.Center - _world.Camera.View.Size / 2) / Tile.TileSize);
            var rd = (Vector2i) ((_world.Camera.View.Center + _world.Camera.View.Size / 2) / Tile.TileSize);

            for (var i = Math.Max(0, lu.X); i < Math.Min(_world.WorldWidth, rd.X); i++)
            {
                for (var j = Math.Max(0, lu.Y); j < Math.Min(_world.WorldHeight, rd.Y); j++)
                {
                    var tile = _world.GetTile(i, j);
                    if (tile == null) SetLight(i,j,1,8,0);

                    
                    
                }
            }
        }

        public static void SetLight(int x, int y, float intensity,int radius, int it)
        {
            if(it>radius)
                return;
            _lightValues[x, y] = intensity;
            
            for (int nx = x - 1; nx < x + 2; nx++)
            {
                for (int ny = y - 1; ny < y + 2; ny++)
                {
                    if (nx != x || ny != y)
                    {
                        var dropoff = _airDropoff;
                        if (_world.GetTile(nx, ny) != null)
                            dropoff = 0.7f;
                        var dist = Math.Sqrt((nx - x) * (nx - x) + (ny - y) * (ny - y));
                        var target = intensity * Math.Pow(dropoff, dist);
                        if(target>_lightValues[nx,ny])
                            SetLight(nx,ny,(float)target,radius,it+1);
                    }
                }
            }
        }

        public static void DrawLightTexture()
        {
            Program.Window.Draw(new Sprite(_texture.Texture)
            {
                Position = _world.Camera.View.Center-_world.Camera.View.Size/2
                    
            }, new RenderStates(BlendMode.Multiply));
        }
    }
}