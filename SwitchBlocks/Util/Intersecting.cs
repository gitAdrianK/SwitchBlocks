namespace SwitchBlocks.Util
{
    using System;
    using System.Linq;
    using JumpKing.BodyCompBehaviours;

    public class Intersecting
    {
        /// <summary>
        /// Checks if the player is intersecting one of the blocks in a larger than zero manner.
        /// </summary>
        /// <param name="behaviourContext">The behaviour context of the situation</param>
        /// <param name="blocks">The blocks to check for</param>
        /// <returns>True if the player is intersecting one of the blocks, false otherwise</returns>
        public static bool IsIntersectingBlocks(BehaviourContext behaviourContext, params Type[] blocks)
        {
            var playerRect = behaviourContext.BodyComp.GetHitbox();
            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            var filtered = advCollisionInfo.GetCollidedBlocks().Where(b => blocks.Contains(b.GetType()));
            foreach (var block in filtered)
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
