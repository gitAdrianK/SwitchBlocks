namespace SwitchBlocks.Util
{
    using System;
    using System.Linq;
    using JumpKing.BodyCompBehaviours;

    /// <summary>Contains a way to determine if the player is intersecting blocks.</summary>
    public static class Intersecting
    {
        /// <summary>
        ///     Checks if the player is intersecting one of the blocks in a larger than zero manner.
        /// </summary>
        /// <param name="behaviourContext"><see cref="BehaviourContext" /> of the situation.</param>
        /// <param name="blocks">Blocks to check for.</param>
        /// <returns><c>true</c> if the player is intersecting one of the blocks, <c>false</c> otherwise.</returns>
        public static bool IsIntersectingBlocks(BehaviourContext behaviourContext, params Type[] blocks)
        {
            var playerRect = behaviourContext.BodyComp.GetHitbox();
            foreach (var block in behaviourContext
                         .CollisionInfo
                         .PreResolutionCollisionInfo
                         .GetCollidedBlocks()
                         .Where(b => blocks.Contains(b.GetType())))
            {
                _ = block.Intersects(playerRect, out var collision);
                if (collision.Size.X > 0 || collision.Size.Y > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
