using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Data;

namespace SwitchBlocks.Blocks
{
    /// <summary>
    /// The basic on block.
    /// </summary>
    public class BlockBasicOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicOn(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.BASIC_ON; }
        }

        public Rectangle GetRect()
        {
            return DataBasic.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return DataBasic.State;
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataBasic.State)
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
