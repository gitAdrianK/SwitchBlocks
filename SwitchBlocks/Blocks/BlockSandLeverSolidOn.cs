namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The sand solid lever block, capable of only turning the state on.
    /// </summary>
    public class BlockSandLeverSolidOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockSandLeverSolidOn(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.SAND_LEVER_SOLID_ON;

        /// <inheritdoc/>
        public override Rectangle GetRect() => this.Ccollider;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Ccollider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Ccollider);
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
