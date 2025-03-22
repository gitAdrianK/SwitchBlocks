namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Util;

    /// <summary>
    /// The group reset block.
    /// </summary>
    public class BlockGroupReset : ModBlock, IResetGroupIds
    {
        /// <inheritdoc/>
        public int[] ResetIds { get; set; } = { };

        /// <inheritdoc/>
        public BlockGroupReset(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.GROUP_RESET;

        /// <inheritdoc/>
        public override Rectangle GetRect() => this.Ccollider;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Ccollider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Ccollider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
