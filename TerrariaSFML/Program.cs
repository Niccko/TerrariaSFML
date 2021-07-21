using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TerrariaSFML
{
    class Program
    {
        public static RenderWindow Window { private set; get; }
        public static Game Game { private set; get; }

        public static View View { private set; get; }

        public static FloatRect getViewRect()
        {
            var viewRectPos = (View.Center - View.Size / 2);
            return new FloatRect(viewRectPos, View.Size);
        }

        static void Main(string[] args)
        {
            //Create window
            Window = new RenderWindow(new VideoMode(1920, 1080), "Terraria");
            Window.SetFramerateLimit(60);
            
            //Load content and create new game process
            Content.Load();
            Game = new Game();

            //Control Close and Resize events
            Window.Closed += WindowClose;
            Window.Resized += WindowResize;

           

            Clock clock = new Clock();
            clock.Restart();
            int cnt = 0;
            while (Window.IsOpen)
            {
                cnt++;
                Window.DispatchEvents();
                Window.Clear(Color.Black);

                Game.Update();
                Game.Draw();
                Window.Display();
                //GC.Collect();
                var end = clock.ElapsedTime;
                clock.Restart();
                if (cnt > 40)
                {
                    Console.WriteLine(1e6 / end.AsMicroseconds());
                    cnt = 0;
                }
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
    }
}