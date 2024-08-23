using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace SwitchBlocks.Blocks
{
    public class BlockAutoResetFull : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockAutoResetFull(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.AUTO_RESET_FULL; }
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
