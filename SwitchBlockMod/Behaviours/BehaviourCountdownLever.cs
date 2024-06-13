﻿using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;

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
            bool collidingWithLever = advCollisionInfo.IsCollidingWith<BlockCountdownLever>()
                || advCollisionInfo.IsCollidingWith<BlockCountdownLeverSolid>();

            if (collidingWithLever)
            {
                DataCountdown.RemainingTime = ModBlocks.countdownDuration;
                DataCountdown.HasBlinkedOnce = false;
                DataCountdown.HasBlinkedTwice = false;

                if (DataCountdown.HasSwitched)
                {
                    return true;
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
    }
}
