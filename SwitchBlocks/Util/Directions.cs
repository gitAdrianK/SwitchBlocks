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
        /// Checks direction a collision happened from and checks if the direction is allowed for that direction.
        /// </summary>
        /// <param name="behaviourContext">Behaviour context</param>
        /// <param name="advCollisionInfo">Advanced collision info</param>
        /// <returns>True if the collision is allowed, false otherwise</returns>
        public static bool ResolveCollisionDirection(BehaviourContext behaviourContext, AdvCollisionInfo advCollisionInfo, Vector2 prevVelocity, HashSet<Direction> validDirections, params Type[] validTypes)
        {
            IBlock block = advCollisionInfo.GetCollidedBlocks().ToList().Find(b => validTypes.Contains(b.GetType()));
            return ResolveCollisionDirection(behaviourContext, advCollisionInfo, prevVelocity, validDirections, block);
        }

        public static bool ResolveCollisionDirection(BehaviourContext behaviourContext, AdvCollisionInfo advCollisionInfo, Vector2 prevVelocity, HashSet<Direction> validDirections, IBlock block)
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
