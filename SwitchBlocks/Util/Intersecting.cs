using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Util
{
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
            Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            List<IBlock> filtered = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => blocks.Contains(b.GetType()));
            foreach (IBlock block in filtered)
            {
                block.Intersects(playerRect, out Rectangle collision);
                if (collision.Size.X > 0 || collision.Size.Y > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
