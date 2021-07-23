using System;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public static class Lighter
    {
        private static RenderTexture _texture;
        private static World _world;
        public static void Init(World world)
        {
            _texture = new RenderTexture(Program.Window.Size.X,Program.Window.Size.Y);
            _world = world;
        }

        public static Texture GetLightTexture()
        {
            _texture.Clear(Color.White);
            var lu = (Vector2i) ((_world.Camera.View.Center - _world.Camera.View.Size / 2) / Tile.TileSize);
            var rd = (Vector2i) ((_world.Camera.View.Center + _world.Camera.View.Size / 2) / Tile.TileSize);

            for (var i = Math.Max(0, lu.X); i < Math.Min(_world.WorldWidth, rd.X); i++)
            {
                for (var j = Math.Max(0, lu.Y); j < Math.Min(_world.WorldHeight, rd.Y); j++)
                {
                    Tile tile = _world.GetTile(i, j);
                    if (tile != null)
                    {
                        var tileRect = new RectangleShape(new Vector2f(Tile.TileSize,Tile.TileSize));
                        tileRect.Position = (Vector2f)Program.Window.MapCoordsToPixel(tile.Position)-(_world.Camera.View.Center-_world.Camera.View.Size/2);
                        tileRect.FillColor = Color.Black;
                        _texture.Draw(tileRect);
                    }
                }
            }
            
            Program.Window.Draw(new Sprite(_texture.Texture)
            {
                Position = _world.Camera.View.Center-_world.Camera.View.Size/2
                    
            });
            return _texture.Texture;
        }
    }
}