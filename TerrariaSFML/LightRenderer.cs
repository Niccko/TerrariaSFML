using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TerrariaSFML
{
    public class LightRenderer
    {
        private byte _darkness = 0;
        private Sprite _surfaceSprite;
        private RenderTexture _surface;
        private BlendMode _mpyBlend;

        public LightRenderer(uint width, uint height)
        {
            _surfaceSprite = new Sprite();
            _surface = new RenderTexture(width*Tile.TileSize,height*Tile.TileSize);
            _mpyBlend = BlendMode.Multiply;
        }
        public void DrawLightSurface()
        {
            _surface.Clear(new Color(_darkness,_darkness,_darkness));
            _surface.Display();
            _surfaceSprite.Texture = _surface.Texture;
            Program.Window.Draw(_surfaceSprite, new RenderStates(_mpyBlend));
        }
    }
}