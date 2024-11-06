using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Util
{
    public static class Directions
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        }

        /// <summary>
        /// Checks from with direction the collision took place and if its a valid direction.
        /// </summary>
        /// <param name="behaviourContext">The behaviour context</param>
        /// <param name="prevVelocity">The previous velocity before the collision</param>
        /// <param name="validDirections">Valid directions</param>
        /// <param name="validTypes">Valid block types</param>
        /// <returns>true if the direction for the block type is valid, otherwise, false</returns>
        public static bool ResolveCollisionDirection(BehaviourContext behaviourContext, Vector2 prevVelocity, HashSet<Direction> validDirections, params Type[] validTypes)
        {
            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            IBlock block = advCollisionInfo.GetCollidedBlocks().ToList().Find(b => validTypes.Contains(b.GetType()));
            return ResolveCollisionDirection(behaviourContext, prevVelocity, validDirections, block);
        }

        public static bool ResolveCollisionDirection(BehaviourContext behaviourContext, Vector2 prevVelocity, HashSet<Direction> validDirections, IBlock block)
        {
            Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
            Rectangle blockRect = block.GetRect();
            if (playerRect.Bottom - blockRect.Top == 0.0f && prevVelocity.Y > 0.0f && validDirections.Contains(Direction.Up))
            {
                return true;
            }
            else if (blockRect.Bottom - playerRect.Top == 0.0f && prevVelocity.Y < 0.0f && validDirections.Contains(Direction.Down))
            {
                return true;
            }
            else if (playerRect.Right - blockRect.Left == 0.0f && prevVelocity.X > 0.0f && validDirections.Contains(Direction.Left))
            {
                return true;
            }
            else if (blockRect.Right - playerRect.Left == 0.0f && prevVelocity.X < 0.0f && validDirections.Contains(Direction.Right))
            {
                return true;
            }
            return false;
        }
    }
}
