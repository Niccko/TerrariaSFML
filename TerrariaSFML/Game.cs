using System;
using SFML.System;
using SFML.Window;

namespace TerrariaSFML
{
    public class Game
    {
        private World world;

        
        public Game()
        {
            world = new World();
            world.GenerateWorld();
            
        }

        public void Update()
        {
            InputManager.Update();
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var pos = Mouse.GetPosition(Program.Window);
                var worldPos = (Vector2i) Program.Window.MapPixelToCoords(pos);
                var tile = new Vector2i(worldPos.X / Tile.TileSize, worldPos.Y / Tile.TileSize);
                if(world.GetTile(tile.X,tile.Y)!=null)
                    world.SetTile(TileType.None, tile.X,tile.Y);
            }
            

            var move = new Vector2f(InputManager.RightPressed - InputManager.LeftPressed,
                InputManager.DownPressed - InputManager.UpPressed);
            world.Camera.Move(move*50);
            world.Camera.View.Zoom(1+(float)(InputManager.MinusPressed-InputManager.PlusPressed)/50);
            world.Camera.Update();
        }

        public void Draw()
        {
            Program.Window.Draw(world);
        }
    }
}