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
        protected RectangleShape rect;
        protected Vector2f velocity;
        protected Vector2f movement;
        protected World world;
        protected bool inAir = true;

        public Entity(World world)
        {
            this.world = world;
        }

        public void Update()
        {
            UpdatePhysics();
        }

        private void UpdatePhysics()
        {
            velocity.X *= 0.99f;
            velocity.Y += 0.55f;

            var offset = velocity + movement;
            var dist = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);

            var countStep = 1;
            if (dist > (float) Tile.TileSize / 2)
                countStep = (int) dist / (Tile.TileSize / 2);

            var nextPos = Position + offset;
            var stepPos = Position - rect.Origin;
            var stepRect = new FloatRect(stepPos, rect.Size);
            var stepVec = (nextPos - Position) / countStep;

            for (int step = 0; step < countStep; step++)
            {
                bool breakStep = false;
                stepPos += stepVec;
                stepRect = new FloatRect(stepPos, rect.Size);

                int i = (int) (stepPos.X + rect.Size.X / 2) / Tile.TileSize;
                int j = (int) (stepPos.Y + rect.Size.Y) / Tile.TileSize;

                Tile tile = world.GetTile(i, j);
                if (tile != null)
                {
                    FloatRect tileRect = new FloatRect(tile.Position, new Vector2f(Tile.TileSize, Tile.TileSize));

                    if (handleCollision(stepRect, tileRect, Direction.Down, ref stepPos))
                    {
                        velocity.Y = 0;
                        inAir = false;
                        breakStep = true;
                    }
                    else
                    {
                        inAir = true;
                    }
                }
                else
                {
                    inAir = true;
                }
                
                if (handleHorizontalCollision(i, j, -1, ref stepPos, stepRect) || handleHorizontalCollision(i, j, 1, ref stepPos, stepRect))
                {
                    breakStep = true;
                }

                if (breakStep)
                    break;
            }

            Position = stepPos + rect.Origin;
        }

        private bool handleCollision(FloatRect entityRect, FloatRect tileRect, Direction dir, ref Vector2f pos)
        {
            if (entityRect.Intersects(tileRect))
            {
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

            return false;
        }
        
        bool handleHorizontalCollision(int i, int j, int iOffset, ref Vector2f stepPos, FloatRect stepRect)
        {
            var dirType = iOffset > 0 ? Direction.Right : Direction.Left;

            Tile[] walls = new Tile[] {
                world.GetTile(i + iOffset, j - 1),
                world.GetTile(i + iOffset, j - 2),
                world.GetTile(i + iOffset, j - 3),
            };

            bool isWallCollided = false;
            foreach (Tile t in walls)
            {
                if (t == null) continue;

                FloatRect tileRect = new FloatRect(t.Position, new Vector2f(Tile.TileSize, Tile.TileSize));
                

                if (handleCollision(stepRect, tileRect, dirType, ref stepPos))
                {
                    isWallCollided = true;
                }
            }

            return isWallCollided;
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}