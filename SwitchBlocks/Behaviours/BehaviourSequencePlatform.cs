using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourSequencePlatform : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }
        public static bool IsPlayerOnIce { get; set; }

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
            IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockSequenceA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceD>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceD>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowD>();

            if (!IsPlayerOnBlock)
            {
                return true;
            }

            List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockSequenceA)
                || b.GetType() == typeof(BlockSequenceIceA)
                || b.GetType() == typeof(BlockSequenceSnowA)
                || b.GetType() == typeof(BlockSequenceB)
                || b.GetType() == typeof(BlockSequenceIceB)
                || b.GetType() == typeof(BlockSequenceSnowB)
                || b.GetType() == typeof(BlockSequenceC)
                || b.GetType() == typeof(BlockSequenceIceC)
                || b.GetType() == typeof(BlockSequenceSnowC)
                || b.GetType() == typeof(BlockSequenceD)
                || b.GetType() == typeof(BlockSequenceIceD)
                || b.GetType() == typeof(BlockSequenceSnowD));
            foreach (IBlockGroupId block in blocks.Cast<IBlockGroupId>())
            {
                int groupId = block.GroupId;
                if (!DataSequence.GetState(groupId) || DataSequence.Touched == groupId)
                {
                    continue;
                }

                if (SettingsSequence.Duration == 0)
                {
                    if (groupId != 1)
                    {
                        DataSequence.SetTick(groupId - 1, Int32.MinValue);
                    }
                }
                else
                {
                    int tick = AchievementManager.GetTicks();
                    DataSequence.SetTick(groupId, tick + SettingsSequence.Duration);
                }
                DataSequence.SetTick(groupId + 1, Int32.MaxValue);
                DataSequence.Touched = groupId;
                break;
            }

            return true;
        }
    }
}