namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic lever block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicLeverSolidOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicLeverSolidOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.BASIC_LEVER_SOLID_OFF;

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
