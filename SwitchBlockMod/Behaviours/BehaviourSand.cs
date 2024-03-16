using Harmony;
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
            //TODO: Move player upwards
            // Up is negative, down is positive
            BodyComp bodyComp = behaviourContext.BodyComp;
            float num = ((IsPlayerOnBlock && bodyComp.Velocity.Y <= 0f) ? 0.5f : 1f);
            float result = inputYVelocity * num;
            if (!IsPlayerOnBlock && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0f)
            {
                bodyComp.Position.Y += 1f;
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
            //TODO: Block top when platform is moving up

            if (((info.IsCollidingWith<BlockSandOn>() && DataSand.State)
                || (info.IsCollidingWith<BlockSandOff>()) && !DataSand.State)
                && !IsPlayerOnBlock)
            {
                return behaviourContext.BodyComp.Velocity.Y < 0f;
            }

            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            //TODO: Sand
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
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
                Traverse.Create(bodyComp).Field("_knocked").SetValue(false);
            }
            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            // TODO: Gravity doesn't apply?
            return inputGravity;
        }
    }
}