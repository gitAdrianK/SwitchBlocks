using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The countdown on block.
    /// </summary>
    public class BlockCountdownOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockCountdownOn(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.COUNTDOWN_ON; }
        }

        public Rectangle GetRect()
        {
            return DataCountdown.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataCountdown.State)
                {
                    return BlockCollisionType.Collision_Blocking;
                }
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
