namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The wind lever block.
    /// </summary>
    public class BlockWindLever : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockWindLever(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.WIND_LEVER;

        public Rectangle GetRect() => this.collider;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
