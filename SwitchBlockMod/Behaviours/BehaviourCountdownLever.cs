using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using System.Linq;

namespace SwitchBlocksMod.Behaviours
{
    /// <summary>
    /// Behaviour related to countdown levers.
    /// </summary>
    public class BehaviourCountdownLever : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

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

            if (collidingWithLever || collidingWithLeverSolid)
            {
                DataCountdown.RemainingTime = ModBlocks.countdownDuration;
                DataCountdown.HasBlinkedOnce = false;
                DataCountdown.HasBlinkedTwice = false;

                if (DataCountdown.HasSwitched)
                {
                    return true;
                }

                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid)
                {
                    if (!ResolveCollisionDirection(behaviourContext, advCollisionInfo))
                    {
                        return true;
                    }
                }

                if (!DataCountdown.State)
                {
                    ModSounds.countdownFlip?.Play();
                }

                DataCountdown.HasSwitched = true;
                DataCountdown.State = true;
            }
            else
            {
                DataCountdown.HasSwitched = false;
            }
            return true;
        }

        /// <summary>
        /// Checks direction a collision happened from and checks if the direction is allowed for that direction.
        /// </summary>
        /// <param name="behaviourContext">Behaviour context</param>
        /// <param name="advCollisionInfo">Advanced collision info</param>
        /// <returns>True if the collision is allowed, false otherwise</returns>
        private bool ResolveCollisionDirection(BehaviourContext behaviourContext, AdvCollisionInfo advCollisionInfo)
        {
            IBlock block = advCollisionInfo.GetCollidedBlocks().ToList().Find(b => b.GetType() == typeof(BlockCountdownLeverSolid));
            Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
            Rectangle blockRect = block.GetRect();
            float bottomCollision = blockRect.Bottom - playerRect.Top;
            float topCollision = playerRect.Bottom - blockRect.Top;
            float leftCollision = playerRect.Right - blockRect.Left;
            float rightCollision = blockRect.Right - playerRect.Left;
            if (topCollision < bottomCollision && topCollision < leftCollision && topCollision < rightCollision)
            {
                if (ModBlocks.countdownDirections.Contains(Util.Directions.Up))
                {
                    return true;
                }
            }
            else if (bottomCollision < topCollision && bottomCollision < leftCollision && bottomCollision < rightCollision)
            {
                if (ModBlocks.countdownDirections.Contains(Util.Directions.Down))
                {
                    return true;
                }
            }
            else if (leftCollision < topCollision && leftCollision < bottomCollision && leftCollision < rightCollision)
            {
                if (ModBlocks.countdownDirections.Contains(Util.Directions.Left))
                {
                    return true;
                }
            }
            else if (rightCollision < topCollision && rightCollision < bottomCollision && rightCollision < leftCollision)
            {
                if (ModBlocks.countdownDirections.Contains(Util.Directions.Right))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
