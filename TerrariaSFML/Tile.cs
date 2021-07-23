using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public enum TileType
    {
        None,
        Ground,
        Grass,
        Stone
    }

    public class Tile : Transformable, Drawable
    {
        public const int TileSize = 16;
        private SpriteSheet SpriteSheet { get; set; }
        private TileType Type { get; set; }

        private readonly RectangleShape _rect;

        private readonly Tile[] _neighbours;

        public Tile this[int index]
        {
            set
            {
                _neighbours[index] = value;
                Update();
            }

            get => _neighbours[index];
        }

        public Tile(TileType type, Tile up, Tile right, Tile down, Tile left)
        {
            Type = type;
            _rect = new RectangleShape(new Vector2f(TileSize, TileSize));
            _neighbours = new[] {up, right, down, left};

            for (var i = 0; i < 4; i++)
            {
                if (_neighbours[i] != null)
                {
                    _neighbours[i][(i + 2) % 4] = this;
                }
            }

            UpdateSpriteSheet();
        }


        private void Update()
        {
            var config = CalculateConfiguration();
            var offset = World.Rand.Next(0, 3);

            _rect.TextureRect = config switch
            {
                0 => SpriteSheet.GetTextureRect(9 + offset, 3),
                1 => SpriteSheet.GetTextureRect(6 + offset, 3),
                2 => SpriteSheet.GetTextureRect(9, offset),
                3 => SpriteSheet.GetTextureRect(2 * offset, 4),
                4 => SpriteSheet.GetTextureRect(6 + offset, 0),
                5 => SpriteSheet.GetTextureRect(5, offset),
                6 => SpriteSheet.GetTextureRect(2 * offset, 3),
                7 => SpriteSheet.GetTextureRect(0, offset),
                8 => SpriteSheet.GetTextureRect(12, offset),
                9 => SpriteSheet.GetTextureRect(2 * offset + 1, 4),
                10 => SpriteSheet.GetTextureRect(6 + offset, 4),
                11 => SpriteSheet.GetTextureRect(offset + 1, 2),
                12 => SpriteSheet.GetTextureRect(2 * offset + 1, 3),
                13 => SpriteSheet.GetTextureRect(4, offset),
                14 => SpriteSheet.GetTextureRect(offset + 1, 0),
                15 => SpriteSheet.GetTextureRect(offset + 1, 1),
                _ => _rect.TextureRect
            };

            switch (Type)
            {
                case TileType.Ground:
                    for (int i = 0; i < 4; i++)
                    {
                        if (_neighbours[i] != null) continue;
                        Type = TileType.Grass;
                        break;
                    }

                    break;
                case TileType.Grass:
                    bool hasEmpty = false;

                    for (int i = 0; i < 4; i++)
                        hasEmpty |= _neighbours[i] == null;

                    if (!hasEmpty)
                        Type = TileType.Ground;
                    break;
            }

            UpdateSpriteSheet();
        }

        private void UpdateSpriteSheet()
        {
            SpriteSheet = Type switch
            {
                TileType.Ground => Content.TexTileGround,
                TileType.Grass => Content.TexTileGrass,
                TileType.Stone => Content.TexTiles9,
                _ => SpriteSheet
            };
            _rect.Texture = SpriteSheet.Texture;
        }

        private int CalculateConfiguration()
        {
            var config = _neighbours[3] != null ? 8 : 0;
            config += _neighbours[2] != null ? 4 : 0;
            config += _neighbours[1] != null ? 2 : 0;
            config += _neighbours[0] != null ? 1 : 0;
            return config;
        }


        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_rect, states);
        }
    }
}