namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Util;

    /// <summary>
    /// The countdown single use lever block.
    /// </summary>
    public class BlockCountdownSingleUse : ModBlock, IBlockGroupId
    {
        /// <inheritdoc/>
        public int GroupId { get; set; } = 0;

        /// <inheritdoc/>
        public BlockCountdownSingleUse(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.COUNTDOWN_SINGLE_USE;

        /// <inheritdoc/>
        public override Rectangle GetRect() => this.Collider;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Collider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
