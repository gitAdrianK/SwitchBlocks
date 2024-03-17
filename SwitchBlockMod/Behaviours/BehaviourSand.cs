using Harmony;
using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using System;

namespace SwitchBlocksMod.Behaviours
{
    /// <summary>
    /// Behaviour related to sand.
    /// </summary>
    public class BehaviourSand : IBlockBehaviour
    {
        public float BlockPriority => 1.0f;

        public bool IsPlayerOnBlock { get; set; }
        public bool IsPlayerOnBlockOn { get; set; }
        public bool IsPlayerOnBlockOff { get; set; }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            float num = IsPlayerOnBlock ? 0.25f : 1.0f;
            return inputXVelocity * num;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            BodyComp bodyComp = behaviourContext.BodyComp;
            if ((IsPlayerOnBlockOn && DataSand.State)
                || (IsPlayerOnBlockOff && !DataSand.State))
            {
                // Going up (negative speed)
                return (2.0f * PlayerValues.GRAVITY) - inputYVelocity;
            }
            else if ((IsPlayerOnBlockOn && !DataSand.State)
                || (IsPlayerOnBlockOff && DataSand.State))
            {
                // Going down (positive speed)
                float num = bodyComp.Velocity.Y <= 0f ? 0.5f : 1f;
                return inputYVelocity * num;
            }
            return inputYVelocity;
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
            bool isCollidingWithOn = info.IsCollidingWith<BlockSandOn>();
            bool isCollidingWithOff = info.IsCollidingWith<BlockSandOff>();

            if ((isCollidingWithOn && DataSand.State)
                || (isCollidingWithOff && !DataSand.State))
            {
                // Going up (negative speed)
                // BUG: Entering with low enough speed to go down again snaps to the top
                return behaviourContext.BodyComp.Velocity.Y > 0.0f;
            }
            if ((isCollidingWithOn && !DataSand.State)
                || (isCollidingWithOff && DataSand.State))
            {
                // Going down (positive speed)
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
            IsPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockSandOn>();
            IsPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockSandOff>();
            IsPlayerOnBlock = IsPlayerOnBlockOn || IsPlayerOnBlockOff;

            BodyComp bodyComp = behaviourContext.BodyComp;
            if (IsPlayerOnBlock)
            {
                Traverse.Create(bodyComp).Field("_knocked").SetValue(false);
            }
            if ((IsPlayerOnBlockOn && DataSand.State)
                || (IsPlayerOnBlockOff && !DataSand.State))
            {
                // Going up (negative speed)
            }
            else if ((IsPlayerOnBlockOn && !DataSand.State)
                || (IsPlayerOnBlockOff && DataSand.State))
            {
                // Going down (positive speed)
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