using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Blocks
{
    /// <summary>
    /// The group snow B block.
    /// </summary>
    public class BlockGroupSnowB : IBlock, IBlockDebugColor, IBlockGroupId
    {
        public int GroupId { get; set; } = 0;

        private readonly Rectangle collider;

        public BlockGroupSnowB(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.GROUP_SNOW_B; }
        }

        public Rectangle GetRect()
        {
            return DataGroup.GetState(GroupId) ? collider : new Rectangle(0, 0, 0, 0);
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataGroup.GetState(GroupId))
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