using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public abstract class NPC: Entity
    {
        protected NPC(World world) : base(world){}

        public void Spawn(Vector2f position)
        {
            Position = position;
            Velocity = new Vector2f(0,0);
        }

        public void Update()
        {
            base.Update();
            UpdateNPC();
        }
        
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(Rect, states);
            DrawNPC(target,states);
        }

        protected abstract void DrawNPC(RenderTarget target, RenderStates states);
        protected abstract void UpdateNPC();
    }
}