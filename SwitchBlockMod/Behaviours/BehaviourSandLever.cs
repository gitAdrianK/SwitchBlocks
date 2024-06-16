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
    /// Behaviour related to sand levers.
    /// </summary>
    public class BehaviourSandLever : IBlockBehaviour
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
            bool collidingWithLever = advCollisionInfo.IsCollidingWith<BlockSandLever>();
            bool collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockSandLeverOn>();
            bool collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockSandLeverOff>();
            bool collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockSandLeverSolid>();
            bool collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOn>();
            bool collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOff>();
            bool collidingWithAnyLever = collidingWithLever || collidingWithLeverSolid;
            bool collidingWithAnyLeverOn = collidingWithLeverOn || collidingWithLeverSolidOn;
            bool collidingWithAnyLeverOff = collidingWithLeverOff || collidingWithLeverSolidOff;
            bool colliding = collidingWithAnyLever
                || collidingWithAnyLeverOn
                || collidingWithAnyLeverOff;

            if (colliding)
            {
                if (DataSand.HasSwitched)
                {
                    return true;
                }
                DataSand.HasSwitched = true;

                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid || collidingWithLeverSolidOn || collidingWithLeverSolidOff)
                {
                    if (!ResolveCollisionDirection(behaviourContext, advCollisionInfo))
                    {
                        return true;
                    }
                }

                bool stateBefore = DataSand.State;
                if (collidingWithLever)
                {
                    DataSand.State = !DataSand.State;
                }
                else if (collidingWithLeverOn)
                {
                    DataSand.State = true;
                }
                else if (collidingWithLeverOff)
                {
                    DataSand.State = false;
                }

                if (stateBefore != DataSand.State)
                {
                    ModSounds.sandFlip?.Play();
                }
            }
            else
            {
                DataSand.HasSwitched = false;
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
            IBlock block = advCollisionInfo.GetCollidedBlocks().ToList().Find(b => b.GetType() == typeof(BlockSandLeverSolid)
                        || b.GetType() == typeof(BlockSandLeverSolidOn)
                        || b.GetType() == typeof(BlockSandLeverSolidOff));
            Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
            Rectangle blockRect = block.GetRect();
            float bottomCollision = blockRect.Bottom - playerRect.Top;
            float topCollision = playerRect.Bottom - blockRect.Top;
            float leftCollision = playerRect.Right - blockRect.Left;
            float rightCollision = blockRect.Right - playerRect.Left;
            if (topCollision < bottomCollision && topCollision < leftCollision && topCollision < rightCollision)
            {
                if (ModBlocks.sandDirections.Contains(Util.Directions.Up))
                {
                    return true;
                }
            }
            else if (bottomCollision < topCollision && bottomCollision < leftCollision && bottomCollision < rightCollision)
            {
                if (ModBlocks.sandDirections.Contains(Util.Directions.Down))
                {
                    return true;
                }
            }
            else if (leftCollision < topCollision && leftCollision < bottomCollision && leftCollision < rightCollision)
            {
                if (ModBlocks.sandDirections.Contains(Util.Directions.Left))
                {
                    return true;
                }
            }
            else if (rightCollision < topCollision && rightCollision < bottomCollision && rightCollision < leftCollision)
            {
                if (ModBlocks.sandDirections.Contains(Util.Directions.Right))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
