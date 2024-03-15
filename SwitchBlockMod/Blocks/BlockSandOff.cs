using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The sand off block.
    /// </summary>
    public class BlockSandOff : IBlock
    {
        private readonly Rectangle collider;

        public BlockSandOff(Rectangle collider)
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
                return BlockCollisionType.Collision_NonBlocking;
            }
            else
            {
                intersection = Rectangle.Empty;
                return BlockCollisionType.NoCollision;
            }
        }
    }
}