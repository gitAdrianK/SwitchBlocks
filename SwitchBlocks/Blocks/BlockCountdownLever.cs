namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The countdown lever block.
    /// </summary>
    public class BlockCountdownLever : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownLever(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.COUNTDOWN_LEVER;

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
