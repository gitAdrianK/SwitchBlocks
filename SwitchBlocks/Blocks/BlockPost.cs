namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Uncreateable dummy block to attach a behaviour to.
    /// Specifically to attach the behaviour to that runs with a high priority number
    /// and as such later then the other block behaviours.
    /// </summary>
    public class BlockPost : IBlock
    {
        public Rectangle GetRect() => Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
