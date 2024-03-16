using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The auto off block.
    /// </summary>
    public class BlockAutoOff : IBlock
    {
        private readonly Rectangle collider;

        public BlockAutoOff(Rectangle collider)
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
                    return BlockCollisionType.Collision_NonBlocking;
                }
                else
                {
                    return BlockCollisionType.Collision_Blocking;
                }
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
