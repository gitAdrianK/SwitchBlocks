namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The wind lever block, capable of only turning the state on.
    /// </summary>
    public class BlockWindLeverOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockWindLeverOn(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.WIND_LEVER_ON;

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
