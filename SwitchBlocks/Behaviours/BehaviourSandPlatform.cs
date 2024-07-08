using HarmonyLib;
using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using System;

namespace SwitchBlocks.Behaviours
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
            float num = (DataSand.HasEntered ? 0.5f : 1.0f);
            return inputXVelocity * num;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {

            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return inputYVelocity;
            }
            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            if (!(advCollisionInfo.IsCollidingWith<BlockSandOn>() || advCollisionInfo.IsCollidingWith<BlockSandOff>()))
            {
                return inputYVelocity;
            }

            BodyComp bodyComp = behaviourContext.BodyComp;
            float num = (DataSand.HasEntered && bodyComp.Velocity.Y <= 0.0f) ? 0.75f : 1.0f;
            float result = inputYVelocity * num;
            if (!DataSand.HasEntered && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0.0f)
            {
                bodyComp.Position.Y += 1.0f;
            }
            return result;
        }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (DataSand.HasEntered)
            {
                return false;
            }
            if (info.IsCollidingWith<BlockSandOn>() || info.IsCollidingWith<BlockSandOff>())
            {
                return !IsPlayerOnBlock;
            }
            return false;
        }

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if ((info.IsCollidingWith<BlockSandOn>() && DataSand.State) || (info.IsCollidingWith<BlockSandOff>() && !DataSand.State))
            {
                if (DataSand.HasEntered)
                {
                    return false;
                }
                DataSand.HasEntered = behaviourContext.BodyComp.Velocity.Y < 0.0f;
                return !DataSand.HasEntered;
            }
            if ((info.IsCollidingWith<BlockSandOn>() && !DataSand.State) || (info.IsCollidingWith<BlockSandOff>() && DataSand.State))
            {
                if (DataSand.HasEntered)
                {
                    return false;
                }
                DataSand.HasEntered = behaviourContext.BodyComp.Velocity.Y >= 0.0f;
                return !DataSand.HasEntered;
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

            if (!IsPlayerOnBlock)
            {
                DataSand.HasEntered = false;
            }

            if (DataSand.HasEntered)
            {
                if ((IsPlayerOnBlockOn && DataSand.State) || (IsPlayerOnBlockOff && !DataSand.State))
                {
                    bodyComp.Position.Y -= 0.75f;
                }
                Traverse.Create(bodyComp).Field("_knocked").SetValue(false);
                Camera.UpdateCamera(bodyComp.GetHitbox().Center);
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