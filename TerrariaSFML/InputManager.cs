using SFML.Window;

namespace TerrariaSFML
{
    public class InputManager
    {
        public static int LeftPressed { private set; get; }
        public static int RightPressed { private set; get; }
        public static int DownPressed { private set; get; }
        public static int UpPressed { private set; get; }
        public static int PlusPressed { private set; get; }
        public static int MinusPressed { private set; get; }

        public static void Update()
        {
            LeftPressed = Keyboard.IsKeyPressed(Keyboard.Key.A) ? 1 : 0;
            RightPressed = Keyboard.IsKeyPressed(Keyboard.Key.D) ? 1 : 0;
            DownPressed = Keyboard.IsKeyPressed(Keyboard.Key.S) ? 1 : 0;
            UpPressed = Keyboard.IsKeyPressed(Keyboard.Key.W) ? 1 : 0;
            PlusPressed = Keyboard.IsKeyPressed(Keyboard.Key.Q) ? 1 : 0;
            MinusPressed = Keyboard.IsKeyPressed(Keyboard.Key.E) ? 1 : 0;
        }
    }
}