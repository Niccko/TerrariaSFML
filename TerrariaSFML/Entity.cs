using System;
using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public abstract class Entity : Transformable, Drawable
    {
        protected RectangleShape Rect;
        protected Vector2f Velocity;
        protected Vector2f Movement;
        private readonly World _world;
        protected bool InAir = true;

        protected Entity(World world)
        {
            _world = world;
        }

        protected void Update()
        {
            UpdatePhysics();
        }

        private void UpdatePhysics()
        {
            Velocity.X *= 0.99f;
            Velocity.Y += 20*Program.DeltaTime;

            var offset = (Velocity + Movement) * Program.DeltaTime * 100;
            var dist = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);

            var countStep = 1;
            if (dist > (float) Tile.TileSize / 2)
                countStep = (int) dist / (Tile.TileSize / 2);

            var nextPos = Position + offset;
            var stepPos = Position - Rect.Origin;
            var stepVec = (nextPos - Position) / countStep;

            for (var step = 0; step < countStep; step++)
            {
                var breakStep = false;
                stepPos += stepVec;
                var stepRect = new FloatRect(stepPos, Rect.Size);

                var i = (int) (stepPos.X + Rect.Size.X / 2) / Tile.TileSize;
                var j = (int) (stepPos.Y + Rect.Size.Y) / Tile.TileSize;

                Tile[] tiles =
                {
                    _world.GetTile(i, j),
                    _world.GetTile(i - 1, j)
                };
                foreach (var tile in tiles)
                {
                    if (tile != null)
                    {
                        var tileRect = new FloatRect(tile.Position, new Vector2f(Tile.TileSize, Tile.TileSize));

                        if (HandleCollision(stepRect, tileRect, Direction.Down, ref stepPos))
                        {
                            Velocity.Y = 0;
                            InAir = false;
                            breakStep = true;
                        }
                        else
                        {
                            InAir = true;
                        }
                    }
                    else
                    {
                        InAir = true;
                    }


                    if (HandleHorizontalCollision(i, j, -1, ref stepPos, stepRect) ||
                        HandleHorizontalCollision(i, j, 1, ref stepPos, stepRect))
                    {
                        breakStep = true;
                    }

                    if (breakStep)
                        break;
                }
            }

            Position = stepPos + Rect.Origin;
        }

        private bool HandleCollision(FloatRect entityRect, FloatRect tileRect, Direction dir, ref Vector2f pos)
        {
            if (!entityRect.Intersects(tileRect)) return false;
            pos = dir switch
            {
                Direction.Up => new Vector2f(pos.X, tileRect.Top + tileRect.Height - 1),
                Direction.Right => new Vector2f(tileRect.Left - entityRect.Width + 1, pos.Y),
                Direction.Down => new Vector2f(pos.X, tileRect.Top - entityRect.Height + 1),
                Direction.Left => new Vector2f(tileRect.Left + tileRect.Width - 1, pos.Y),
                _ => pos
            };
            return true;
        }

        private bool HandleHorizontalCollision(int i, int j, int iOffset, ref Vector2f stepPos, FloatRect stepRect)
        {
            var dirType = iOffset > 0 ? Direction.Right : Direction.Left;

            Tile[] walls =
            {
                _world.GetTile(i + iOffset, j - 1),
                _world.GetTile(i + iOffset, j - 2),
                _world.GetTile(i + iOffset, j - 3),
            };

            var isWallCollided = false;
            foreach (var t in walls)
            {
                if (t == null) continue;

                var tileRect = new FloatRect(t.Position, new Vector2f(Tile.TileSize, Tile.TileSize));


                if (HandleCollision(stepRect, tileRect, dirType, ref stepPos))
                {
                    isWallCollided = true;
                }
            }

            return isWallCollided;
        }

        private bool HandleVerticalCollision(int i, int j, int jOffset, ref Vector2f stepPos, FloatRect stepRect)
        {
            var dirType = jOffset > 0 ? Direction.Down : Direction.Up;

            Tile[] walls =
            {
                _world.GetTile(i - 1, j + jOffset),
                _world.GetTile(i, j + jOffset)
            };

            var collision = false;
            foreach (var tile in walls)
            {
                if (tile == null) continue;
                var tileRect = new FloatRect(tile.Position, new Vector2f(Tile.TileSize, Tile.TileSize));

                if (!HandleCollision(stepRect, tileRect, dirType, ref stepPos)) continue;
                collision = true;
                if (dirType == Direction.Down)
                {
                    Velocity.Y = 0;
                    InAir = false;
                    break;
                }

                InAir = true;
            }

            return collision;
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}