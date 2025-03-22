namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic lever block.
    /// </summary>
    public class BlockBasicLever : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicLever(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.BASIC_LEVER;

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
