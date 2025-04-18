namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Util;

    /// <summary>
    /// The group solid reset block.
    /// </summary>
    public class BlockGroupResetSolid : ModBlock, IResetGroupIds
    {
        /// <inheritdoc/>
        public int[] ResetIds { get; set; } = { };

        /// <inheritdoc/>
        public BlockGroupResetSolid(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.GROUP_RESET_SOLID;

        /// <inheritdoc/>
        public override Rectangle GetRect() => this.Collider;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Collider);
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
