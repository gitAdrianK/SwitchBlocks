using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The basic lever block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicLeverSolidOn : IBlock
    {
        private readonly Rectangle collider;

        public BlockBasicLeverSolidOn(Rectangle collider)
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
                return BlockCollisionType.Collision_Blocking;
            }
            else
            {
                intersection = Rectangle.Empty;
                return BlockCollisionType.NoCollision;
            }
        }
    }
}
