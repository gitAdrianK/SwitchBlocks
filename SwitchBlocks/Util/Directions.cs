﻿using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Behaviours;
using System.Collections.Generic;

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
        /// <param name="validDirections">Valid directions</param>
        /// <param name="block">The block that has its direction checked</param>
        /// <returns>true if the direction for the block type is valid, otherwise, false</returns>
        public static bool ResolveCollisionDirection(BehaviourContext behaviourContext, HashSet<Direction> validDirections, IBlock block)
        {
            Vector2 prevVelocity = BehaviourPost.PrevVelocity;
            Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
            Rectangle blockRect = block.GetRect();
            if (playerRect.Bottom - blockRect.Top == 0 && prevVelocity.Y > 0.0f && validDirections.Contains(Direction.Up))
            {
                return true;
            }
            else if (blockRect.Bottom - playerRect.Top == 0 && prevVelocity.Y < 0.0f && validDirections.Contains(Direction.Down))
            {
                return true;
            }
            else if (playerRect.Right - blockRect.Left == 0 && prevVelocity.X > 0.0f && validDirections.Contains(Direction.Left))
            {
                return true;
            }
            else if (blockRect.Right - playerRect.Left == 0 && prevVelocity.X < 0.0f && validDirections.Contains(Direction.Right))
            {
                return true;
            }
            return false;
        }
    }
}
