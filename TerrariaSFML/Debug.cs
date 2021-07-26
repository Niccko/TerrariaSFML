using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public static class Debug
    {
        public static bool Enabled { get; set; }

        private static Dictionary<string, Menu> _menuList = new();

        public static void AddItem(string menuName, string itemName, string itemValue)
        {
            _menuList[menuName].items.Add(itemName, itemValue);
        }

        public static void SetItem(string menuName, string itemName, string itemValue)
        {
            _menuList[menuName].items[itemName] = itemValue;
        }

        public static void AddMenu(float x, float y, string name)
        {
            _menuList.Add(name, new Menu(x,y));
        }
        public static void Draw(View view)
        {
            foreach (var menu in _menuList.Values)
            {
                menu.Update();
                menu.Draw(view);
            }
        }
    }

    internal class Menu
    {
        public Dictionary<string, string> items;
        private RectangleShape _rect;
        private Vector2f pos;
        private Font _font;

        public Menu(float x, float y)
        {
            items = new Dictionary<string, string>();
            _rect = new RectangleShape(new Vector2f(200, 50))
            {
                Position = new Vector2f(x, y), FillColor = new Color(0, 0, 0, 120)
            };
            pos = new Vector2f(x, y);
            _font = new Font("..\\..\\..\\Fonts\\arial.ttf");
        }

        public void Update()
        {
            
        }

        public void Draw(View view)
        {
            _rect.Position = view.Center-view.Size/2 + pos;
            _rect.Scale = new Vector2f(1, items.Count);
            Program.Window.Draw(_rect);
            int index = 0;
            foreach (var item in items)
            {
                Text text = new Text(item.Key + ": " + item.Value,_font,18);
                text.Position = _rect.Position + new Vector2f(_rect.Size.X/8, index * 50+15);
                Program.Window.Draw(text);
                index++;
            }
        }
    }
}