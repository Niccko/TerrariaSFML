using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TerrariaSFML.Entities
{
    public class Player : NPC
    {
        private int _jumpForce = 15;
        private int _maxSpeed = 10;
        private int _speed = 0;
        private float _acceleration = 0.55f;

        public Player(World world) : base(world)
        {
            rect = new RectangleShape(new Vector2f(Tile.TileSize * 1.5f, Tile.TileSize * 2.8f));
            rect.Origin = new Vector2f(rect.Size.X / 2, 0);
            rect.FillColor = Color.White;

            movement = new Vector2f();
        }

        public override void DrawNPC(RenderTarget target, RenderStates states)
        {
            target.Draw(rect, states);
        }

        private void UpdateMovement()
        {
            if (Input.KeyPressed(Keyboard.Key.Space) == 1 && !inAir)
                velocity.Y = -_jumpForce;

            if (Input.KeyReleased(Keyboard.Key.Space) == 1 && velocity.Y < -4)
                velocity.Y = -4;

            var isMoveLeft = Keyboard.IsKeyPressed(Keyboard.Key.A);
            var isMoveRight = Keyboard.IsKeyPressed(Keyboard.Key.D);
            var isMove = isMoveLeft || isMoveRight;

            if (isMove)
            {
                if (isMoveLeft)
                {
                    if (movement.X > 0)
                        movement.X = 0;

                    movement.X -= _acceleration;
                }
                else if (isMoveRight)
                {
                    if (movement.X < 0)
                        movement.X = 0;

                    movement.X += _acceleration;
                }

                if (movement.X > _maxSpeed)
                    movement.X = _maxSpeed;
                else if (movement.X < -_maxSpeed)
                    movement.X = -_maxSpeed;
            }
            else
            {
                movement *= 0.8f;
            }
        }


        public override void UpdateNPC()
        {
            UpdateMovement();
        }
    }
}