using HarmonyLib;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using SwitchBlocksMod.Blocks;
using System;

namespace SwitchBlocksMod.Behaviours
{
    /// <summary>
    /// Behaviour related to sand platforms.
    /// </summary>
    public class BehaviourSandPlatform : IBlockBehaviour
    {
        public float BlockPriority => 1.0f;

        public bool IsPlayerOnBlock { get; set; }
        public bool IsPlayerOnBlockOn { get; set; }
        public bool IsPlayerOnBlockOff { get; set; }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            // 0.25f from SandBlockBehaviour results in the wrong X speed, 0.5f seems to be about right.
            float num = (IsPlayerOnBlock ? 0.5f : 1.0f);
            return inputXVelocity * num;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            BodyComp bodyComp = behaviourContext.BodyComp;
            float num = ((IsPlayerOnBlock && bodyComp.Velocity.Y <= 0.0f) ? 0.5f : 1.0f);
            float result = inputYVelocity * num;
            if (!IsPlayerOnBlock && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0f)
            {
                bodyComp.Position.Y += 1.0f;
            }

            return result;
        }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.IsCollidingWith<BlockSandOn>() || info.IsCollidingWith<BlockSandOff>())
            {
                return !IsPlayerOnBlock;
            }
            return false;
        }

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if ((info.IsCollidingWith<BlockSandOn>() || info.IsCollidingWith<BlockSandOff>()) && !IsPlayerOnBlock)
            {
                return behaviourContext.BodyComp.Velocity.Y < 0.0f;
            }
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }


            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            BodyComp bodyComp = behaviourContext.BodyComp;

            IsPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockSandOn>();
            IsPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockSandOff>();
            IsPlayerOnBlock = IsPlayerOnBlockOn || IsPlayerOnBlockOff;

            if (IsPlayerOnBlock)
            {
                //Camera.UpdateCamera(bodyComp.GetHitbox().Center);
                //Traverse.Create(bodyComp).Field("_is_on_ground").SetValue(true);
                Traverse.Create(bodyComp).Field("_knocked").SetValue(false);
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
            }
            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }
    }
}