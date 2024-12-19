namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The countdown lever block.
    /// </summary>
    public class BlockCountdownLever : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockCountdownLever(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.COUNTDOWN_LEVER;

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
