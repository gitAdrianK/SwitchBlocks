using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The auto off block.
    /// </summary>
    public class BlockAutoOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockAutoOff(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.AUTO_OFF; }
        }

        public Rectangle GetRect()
        {
            return !DataAuto.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return !DataAuto.State;
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
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
