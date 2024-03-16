using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The auto on block.
    /// </summary>
    public class BlockAutoOn : IBlock
    {
        private readonly Rectangle collider;

        public BlockAutoOn(Rectangle collider)
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
                if (DataAuto.State)
                {
                    return BlockCollisionType.Collision_Blocking;
                }
                else
                {
                    return BlockCollisionType.Collision_NonBlocking;
                }
            }
            else
            {
                intersection = Rectangle.Empty;
                return BlockCollisionType.NoCollision;
            }
        }
    }
}
