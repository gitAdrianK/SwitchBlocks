using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Settings;
using SwitchBlocks.Setups;
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

            IEnumerable<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                Type type = b.GetType();
                return type == typeof(BlockSequenceA)
                || type == typeof(BlockSequenceIceA)
                || type == typeof(BlockSequenceSnowA)
                || type == typeof(BlockSequenceB)
                || type == typeof(BlockSequenceIceB)
                || type == typeof(BlockSequenceSnowB)
                || type == typeof(BlockSequenceC)
                || type == typeof(BlockSequenceIceC)
                || type == typeof(BlockSequenceSnowC)
                || type == typeof(BlockSequenceD)
                || type == typeof(BlockSequenceIceD)
                || type == typeof(BlockSequenceSnowD);
            });
            foreach (IBlockGroupId block in blocks.Cast<IBlockGroupId>())
            {
                int groupId = block.GroupId;
                if (!DataSequence.GetState(groupId)
                    || DataSequence.Touched != (groupId - 1)
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsSequence.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }

                if (SettingsSequence.Duration == 0)
                {
                    if (groupId > 1)
                    {
                        DataSequence.SetTick(groupId - 1, Int32.MinValue);
                        DataSequence.Active.Add(groupId - 1);
                    }
                }
                else
                {
                    int tick = AchievementManager.GetTicks();
                    DataSequence.SetTick(groupId, tick + SettingsSequence.Duration);
                    DataSequence.Active.Add(groupId);
                }
                if (groupId < SetupSequence.SequenceCount)
                {
                    DataSequence.SetTick(groupId + 1, int.MaxValue);
                    DataSequence.Active.Add(groupId + 1);
                }
                DataSequence.Touched = groupId;
                DataSequence.Active.Add(groupId + 1);
                break;
            }

            return true;
        }
    }
}
