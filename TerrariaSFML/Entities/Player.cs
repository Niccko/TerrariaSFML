using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TerrariaSFML.Entities
{
    public class Player : NPC
    {
        private int JumpForce = 9;
        private int MaxSpeed = 3;
        private float _acceleration = 40f;
        private bool _canJump = true;

        public Player(World world) : base(world)
        {
            Rect = new RectangleShape(new Vector2f(Tile.TileSize * 2, Tile.TileSize * 3));
            Rect.Origin = new Vector2f(Rect.Size.X / 2, 0);
            Rect.FillColor = Color.White;
            Movement = new Vector2f();
            Debug.AddItem("main","Can jump","");
        }

        protected override void DrawNPC(RenderTarget target, RenderStates states)
        {
            target.Draw(Rect, states);
        }

        private void UpdateMovement()
        {
            Debug.SetItem("main","Can jump",_canJump.ToString());
            if (Input.KeyPressed(Keyboard.Key.Space) == 1 && !InAir && _canJump)
            {
                Velocity.Y = -JumpForce;
                _canJump = false;
            }


            if (Input.KeyReleased(Keyboard.Key.Space) == 1)
            {
                if(Velocity.Y < -5)
                    Velocity.Y = -5;
                _canJump = true;
            }

            var isMoveLeft = Keyboard.IsKeyPressed(Keyboard.Key.A);
            var isMoveRight = Keyboard.IsKeyPressed(Keyboard.Key.D);
            var isMove = isMoveLeft || isMoveRight;

            if (isMove)
            {
                if (InAir) _acceleration = 10;
                if (isMoveLeft)
                {
                    if (Movement.X > 0)
                        Movement.X = 0;

                    Movement.X -= _acceleration * Program.DeltaTime;
                }
                else
                {
                    if (Movement.X < 0)
                        Movement.X = 0;

                    Movement.X += _acceleration * Program.DeltaTime;
                }

                Movement.X = Math.Clamp(Movement.X, -MaxSpeed, MaxSpeed);
            }
            else
            {
                _acceleration = 40;
                Movement *= 0.8f;
            }
        }


        protected override void UpdateNPC()
        {
            UpdateMovement();
        }
    }
}