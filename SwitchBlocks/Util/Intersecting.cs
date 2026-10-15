namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>Contains a way to determine if the player is intersecting blocks.</summary>
    public static class Intersecting
    {
        /// <summary>
        ///     Checks if the player is intersecting one of the blocks in a larger than zero manner.
        /// </summary>
        /// <param name="hitbox"><see cref="Rectangle" /> to check against.</param>
        /// <param name="blocks">Blocks to check for.</param>
        /// <returns><c>true</c> if the player is intersecting one of the blocks, <c>false</c> otherwise.</returns>
        public static bool IsIntersectingBlocks(Rectangle hitbox, params IReadOnlyList<IBlock>[] blocks)
        {
            foreach (var block in blocks.SelectMany(block => block))
            {
                _ = block.Intersects(hitbox, out var collision);
                if (collision.Size.X > 0 || collision.Size.Y > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
