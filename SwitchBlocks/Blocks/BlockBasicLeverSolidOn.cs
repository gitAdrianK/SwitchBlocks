namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic lever block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicLeverSolidOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicLeverSolidOn(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.BASIC_LEVER_SOLID_ON;

        public Rectangle GetRect() => this.collider;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
