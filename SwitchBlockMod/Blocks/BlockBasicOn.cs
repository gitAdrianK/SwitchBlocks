using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The basic on block.
    /// </summary>
    public class BlockBasicOn : IBlock
    {
        private readonly Rectangle collider;

        public BlockBasicOn(Rectangle collider)
        {
            this.collider = collider;
        }

        public Rectangle GetRect()
        {
            return collider;
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataBasic.State)
                {
                    return BlockCollisionType.Collision_Blocking;
                }
                else
                {
                    return BlockCollisionType.Collision_NonBlocking;
                }
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
