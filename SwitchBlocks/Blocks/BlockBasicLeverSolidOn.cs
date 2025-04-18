namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic solid lever block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicLeverSolidOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicLeverSolidOn(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.BASIC_LEVER_SOLID_ON;

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
