using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The countdown off block.
    /// </summary>
    public class BlockCountdownOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockCountdownOff(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.COUNTDOWN_OFF; }
        }

        public Rectangle GetRect()
        {
            return !DataCountdown.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataCountdown.State)
                {
                    return BlockCollisionType.Collision_NonBlocking;
                }
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
