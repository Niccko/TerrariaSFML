using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TerrariaSFML.Entities;

namespace TerrariaSFML
{
    public class Game
    {
        private World world;

        public Player Player { private set; get; }

        private Shader Shader { set; get; }
        public Game()
        {
            Debug.AddMenu(0,0,"main");
            world = new World(400,400);
            world.GenerateWorld();
            Player = new Player(world);
            Player.Spawn(new Vector2f(world.WorldWidth*Tile.TileSize/2f,2800));

            Shader = new Shader(null, null, "..\\..\\..\\Shaders\\light.frag");
            //shader.SetUniform("texture",Lighter.GetLightTexture());
            
            Debug.AddItem("main","FPS","0");
            
        }

        public void Update()
        {
            Player.Update();
            world.Camera.Follow(Player.Position);
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var pos = Mouse.GetPosition(Program.Window);
                var worldPos = (Vector2i) Program.Window.MapPixelToCoords(pos);
                var tile = new Vector2i(worldPos.X / Tile.TileSize, worldPos.Y / Tile.TileSize);
                if(world.GetTile(tile.X,tile.Y)!=null)
                    world.SetTile(TileType.None, tile.X,tile.Y);
            }
            

            
            world.Camera.View.Zoom(1+(float)(Input.KeyPressed(Keyboard.Key.Q)-Input.KeyPressed(Keyboard.Key.E))/50);
            world.Camera.Update();
           
        }

        public void Draw()
        {
            //Shader.Bind(Shader);
            Program.Window.Draw(world);
            Program.Window.Draw(Player);
            //Shader.Bind(null);
            Debug.Draw(world.Camera.View);
        }
    }
}