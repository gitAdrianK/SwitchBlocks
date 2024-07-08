using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    /// <summary>
    /// Behaviour related to countdown levers.
    /// </summary>
    public class BehaviourCountdownLever : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        private Vector2 prevVelocity = new Vector2(0, 0);

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            return false;
        }

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            return false;
        }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            return inputXVelocity;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            return inputYVelocity;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            bool collidingWithLever = advCollisionInfo.IsCollidingWith<BlockCountdownLever>();
            bool collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockCountdownLeverSolid>();
            IsPlayerOnBlock = collidingWithLever || collidingWithLeverSolid;

            if (IsPlayerOnBlock)
            {
                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid)
                {
                    if (!ResolveCollisionDirection(behaviourContext, advCollisionInfo, prevVelocity))
                    {
                        prevVelocity = behaviourContext.BodyComp.Velocity;
                        return true;
                    }
                }

                DataCountdown.RemainingTime = ModBlocks.countdownDuration;
                DataCountdown.HasBlinkedOnce = false;
                DataCountdown.HasBlinkedTwice = false;

                if (DataCountdown.HasSwitched)
                {
                    prevVelocity = behaviourContext.BodyComp.Velocity;
                    return true;
                }

                if (!DataCountdown.State)
                {
                    ModSounds.countdownFlip?.PlayOneShot();
                }

                DataCountdown.HasSwitched = true;
                DataCountdown.State = true;
            }
            else
            {
                DataCountdown.HasSwitched = false;
            }
            prevVelocity = behaviourContext.BodyComp.Velocity;
            return true;
        }

        /// <summary>
        /// Checks direction a collision happened from and checks if the direction is allowed for that direction.
        /// </summary>
        /// <param name="behaviourContext">Behaviour context</param>
        /// <param name="advCollisionInfo">Advanced collision info</param>
        /// <returns>True if the collision is allowed, false otherwise</returns>
        private bool ResolveCollisionDirection(BehaviourContext behaviourContext, AdvCollisionInfo advCollisionInfo, Vector2 prevVelocity)
        {
            IBlock block = advCollisionInfo.GetCollidedBlocks().ToList().Find(b => b.GetType() == typeof(BlockCountdownLeverSolid));
            Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
            Rectangle blockRect = block.GetRect();
            HashSet<Directions> directions = ModBlocks.countdownDirections;
            if (playerRect.Bottom - blockRect.Top == 0.0f && prevVelocity.Y > 0.0f && directions.Contains(Directions.Up))
            {
                return true;
            }
            else if (blockRect.Bottom - playerRect.Top == 0.0f && prevVelocity.Y < 0.0f && directions.Contains(Directions.Down))
            {
                return true;
            }
            else if (playerRect.Right - blockRect.Left == 0.0f && prevVelocity.X > 0.0f && directions.Contains(Directions.Left))
            {
                return true;
            }
            else if (blockRect.Right - playerRect.Left == 0.0f && prevVelocity.X < 0.0f && directions.Contains(Directions.Right))
            {
                return true;
            }
            return false;
        }
    }
}
