namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Wind block returned by all types (that can influence the wind) when a wind enabled block is placed.
    /// </summary>
    public class BlockWind : IBlock
    {
        public Rectangle GetRect() => Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
