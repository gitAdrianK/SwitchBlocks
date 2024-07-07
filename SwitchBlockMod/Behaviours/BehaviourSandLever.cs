using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;
using System.Collections.Generic;
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
                    prevVelocity = behaviourContext.BodyComp.Velocity;
                    return true;
                }
                DataSand.HasSwitched = true;

                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid || collidingWithLeverSolidOn || collidingWithLeverSolidOff)
                {
                    if (!ResolveCollisionDirection(behaviourContext, advCollisionInfo, prevVelocity))
                    {
                        prevVelocity = behaviourContext.BodyComp.Velocity;
                        return true;
                    }
                }

                bool stateBefore = DataSand.State;
                if (collidingWithAnyLever)
                {
                    DataSand.State = !DataSand.State;
                }
                else if (collidingWithAnyLeverOn)
                {
                    DataSand.State = true;
                }
                else if (collidingWithAnyLeverOff)
                {
                    DataSand.State = false;
                }

                if (stateBefore != DataSand.State)
                {
                    ModSounds.sandFlip?.PlayOneShot();
                }
            }
            else
            {
                DataSand.HasSwitched = false;
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
            IBlock block = advCollisionInfo.GetCollidedBlocks().ToList().Find(b => b.GetType() == typeof(BlockSandLeverSolid)
                        || b.GetType() == typeof(BlockSandLeverSolidOn)
                        || b.GetType() == typeof(BlockSandLeverSolidOff));
            Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
            Rectangle blockRect = block.GetRect();
            HashSet<Directions> directions = ModBlocks.sandDirections;
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
