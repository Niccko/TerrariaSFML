using SFML.Graphics;
using SFML.System;

namespace TerrariaSFML
{
    public abstract class NPC: Entity
    {

        public NPC(World world) : base(world){}

        public void Spawn(Vector2f position)
        {
            Position = position;
            velocity = new Vector2f(0,0);
        }

        public void Update()
        {
            base.Update();
            UpdateNPC();
        }
        
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(rect, states);
            DrawNPC(target,states);
        }

        public abstract void DrawNPC(RenderTarget target, RenderStates states);
        public abstract void UpdateNPC();
    }
}