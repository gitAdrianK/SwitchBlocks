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
        /// <summary>
        /// This block does not draw a <see cref="Rectangle"/> in debug.
        /// </summary>
        /// <returns><see cref="Rectangle.Empty"/>.</returns>
        public Rectangle GetRect() => Rectangle.Empty;

        /// <summary>
        /// This block cannot intersect.
        /// </summary>
        /// <param name="hitbox">Hitbx to check against</param>
        /// <param name="intersection"><see cref="BlockCollisionType.NoCollision"/>.</param>
        /// <returns><see cref="BlockCollisionType.NoCollision"/></returns>
        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
