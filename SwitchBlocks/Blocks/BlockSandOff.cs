using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace SwitchBlocks.Blocks
{
    /// <summary>
    /// The sand off block.
    /// </summary>
    public class BlockSandOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockSandOff(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.SAND_OFF; }
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
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
