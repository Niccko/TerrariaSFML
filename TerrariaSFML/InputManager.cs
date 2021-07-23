using System.Collections.Generic;
using SFML.Window;

namespace TerrariaSFML
{
    public static class Input
    {
        private static Dictionary<Keyboard.Key, int> Keys { get; set; }
        private static Dictionary<Keyboard.Key, int> ReleasedKeys { get; set; }

        public static void Init()
        {
            Keys = new Dictionary<Keyboard.Key, int>();
            ReleasedKeys = new Dictionary<Keyboard.Key, int>();
        }

        public static void UpdateKeyPressed(Keyboard.Key key)
        {
            if (!Keys.ContainsKey(key))
            {
                Keys.Add(key, 0);
            }

            Keys[key] = 1;
        }

        public static void UpdateKeyReleased(Keyboard.Key key)
        {
            if (!ReleasedKeys.ContainsKey(key))
            {
                ReleasedKeys.Add(key, 0);
            }

            ReleasedKeys[key] = 1;
            Keys[key] = 0;
        }

        public static int KeyPressed(Keyboard.Key key)
        {
            return Keys.ContainsKey(key) ? Keys[key] : 0;
        }

        public static int KeyReleased(Keyboard.Key key)
        {
            return ReleasedKeys.ContainsKey(key) ? ReleasedKeys[key] : 0;
        }

        public static void Reset()
        {
            foreach (var key in ReleasedKeys.Keys)
            {
                ReleasedKeys[key] = 0;
            }
        }
    }
}