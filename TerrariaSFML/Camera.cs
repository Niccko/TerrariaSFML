using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public class Camera
    {
        public View View { private set; get; }
        private readonly World _world;
        public Camera(Vector2f pos, World world)
        {
            View = new View(pos,(Vector2f)Program.Window.Size);
            _world = world;
        }

        public void Update()
        {
            Program.Window.SetView(View);
        }

        public void Move(Vector2f offset)
        {
            var pos = View.Center + offset;
            if (pos.X >= (View.Size / 2).X && pos.X <= _world.WorldWidth * Tile.TileSize - (View.Size / 2).X &&
                pos.Y >= (View.Size / 2).Y && pos.Y <= _world.WorldHeight * Tile.TileSize - (View.Size / 2).Y)
            {
                View.Move(offset);
            }
        }

        public void SetPosition(Vector2f pos)
        {
            if (pos.X >= (View.Size / 2).X && pos.X <= _world.WorldWidth * Tile.TileSize - (View.Size / 2).X &&
                pos.Y >= (View.Size / 2).Y && pos.Y <= _world.WorldHeight * Tile.TileSize - (View.Size / 2).Y)
            {
                View.Center = pos;
            }
        }

        public void Follow(Vector2f pos)
        {
            if (pos.X >= (View.Size / 2).X && pos.X <= _world.WorldWidth * Tile.TileSize - (View.Size / 2).X &&
                pos.Y >= (View.Size / 2).Y && pos.Y <= _world.WorldHeight * Tile.TileSize - (View.Size / 2).Y)
            {
                Move((pos - View.Center)/10);
            }
        }
    }
}