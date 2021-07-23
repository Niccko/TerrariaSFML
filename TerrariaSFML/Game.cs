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

        public Shader shader { private set; get; }
        public Game()
        {
            world = new World(400,400);
            world.GenerateWorld();
            Player = new Player(world);
            Player.Spawn(new Vector2f(world.WorldWidth*Tile.TileSize/2f,2800));

            shader = new Shader(null, null, "..\\..\\..\\Shaders\\light.frag");
            //shader.SetUniform("texture",Lighter.GetLightTexture());
            Debug.AddMenu(0,0,"main");
            
        }

        public void Update()
        {
            Player.Update();
            world.Camera.SetPosition(Player.Position);
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
            Shader.Bind(shader);
            Program.Window.Draw(world);
            Program.Window.Draw(Player);
            Shader.Bind(null);
            Debug.Draw(world.Camera.View);
        }
    }
}