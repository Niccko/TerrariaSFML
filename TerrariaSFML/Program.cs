﻿using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TerrariaSFML
{
    class Program
    {
        public static RenderWindow Window { private set; get; }
        public static Game Game { private set; get; }
        

        static void Main(string[] args)
        {
            var clock = new Clock();
            
            //Create window
            Window = new RenderWindow(new VideoMode(1920, 1080), "Terraria");
            Window.SetFramerateLimit(60);

            //Load content, initialize input system  and create new game process
            Content.Load();
            Input.Init();
            Game = new Game();

            //Control Close and Resize events
            Window.Closed += WindowClose;
            Window.Resized += WindowResize;
            Window.KeyPressed += KeyPressed;
            Window.KeyReleased += KeyReleased;

            
            Debug.AddItem("main", "FPS", 0.ToString());

            
            clock.Restart();
            var cnt = 0;
            while (Window.IsOpen)
            {
                Window.DispatchEvents();


                Window.Clear(Color.Black);

                Game.Update();
                Game.Draw();
                Window.Display();


                var end = clock.ElapsedTime;
                cnt++;
                if (cnt > 20)
                {
                    Debug.SetItem("main", "FPS", ((int) 1e6 / end.AsMicroseconds()).ToString());
                    cnt = 0;
                }

                Input.Reset();
                clock.Restart();
            }
        }


        private static void WindowResize(object sender, SizeEventArgs e)
        {
            Window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        private static void WindowClose(object sender, EventArgs e)
        {
            Window.Close();
        }

        private static void KeyPressed(object sender, KeyEventArgs e)
        {
            Input.UpdateKeyPressed(e.Code);
        }

        private static void KeyReleased(object sender, KeyEventArgs e)
        {
            Input.UpdateKeyReleased(e.Code);
        }
    }
}