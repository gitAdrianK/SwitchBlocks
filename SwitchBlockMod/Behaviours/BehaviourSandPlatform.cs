using HarmonyLib;
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
    /// Behaviour related to sand platforms.
    /// </summary>
    public class BehaviourSandPlatform : IBlockBehaviour
    {
        public float BlockPriority => 1.0f;

        // TODO: Player falls too fast
        // TODO: Cant really jump to the side

        public bool IsPlayerOnBlock { get; set; }
        public bool IsPlayerOnBlockOn { get; set; }
        public bool IsPlayerOnBlockOff { get; set; }
        public static bool HasEntered { get; private set; }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            // 0.25f from SandBlockBehaviour results in the wrong X speed, 0.5f seems to be about right.
            float num = (HasEntered ? 0.5f : 1.0f);
            return inputXVelocity * num;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            BodyComp bodyComp = behaviourContext.BodyComp;
            float num = ((HasEntered && bodyComp.Velocity.Y <= 0.0f) ? 0.75f : 1.0f);
            float result = inputYVelocity * num;
            if (!HasEntered && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0.0f)
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
            if ((info.IsCollidingWith<BlockSandOn>() && DataSand.State) || (info.IsCollidingWith<BlockSandOff>() && !DataSand.State) && !IsPlayerOnBlock)
            {
                if (HasEntered)
                {
                    return false;
                }
                HasEntered = behaviourContext.BodyComp.Velocity.Y >= 0.0f;
                return !HasEntered;
            }
            else if ((info.IsCollidingWith<BlockSandOn>() && !DataSand.State) || (info.IsCollidingWith<BlockSandOff>() && DataSand.State) && !IsPlayerOnBlock)
            {
                if (HasEntered)
                {
                    return false;
                }
                HasEntered = behaviourContext.BodyComp.Velocity.Y < 0.0f;
                return !HasEntered;
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
                HasEntered = false;
            }

            if (HasEntered)
            {
                if ((IsPlayerOnBlockOn && !DataSand.State) || (IsPlayerOnBlockOff && DataSand.State))
                {
                    // Going up
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