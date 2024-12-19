namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Uncreateable dummy block to attach a behaviour to.
    /// Specifically to attach the behaviour to that runs with a low priority number
    /// and as such earlier then the other block behaviours.
    /// </summary>
    public class BlockPre : IBlock
    {
        public Rectangle GetRect() => Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
