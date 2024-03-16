using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The basic lever block.
    /// </summary>
    public class BlockBasicLever : IBlock
    {
        private readonly Rectangle collider;

        public BlockBasicLever(Rectangle collider)
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
