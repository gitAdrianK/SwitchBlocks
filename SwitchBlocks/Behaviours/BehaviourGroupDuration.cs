using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourGroupDuration : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }
        public static bool IsPlayerOnIce { get; set; }

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
            IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockGroupA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupC>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceC>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowC>()
                || advCollisionInfo.IsCollidingWith<BlockGroupD>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceD>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowD>();

            if (!IsPlayerOnBlock)
            {
                prevVelocity = behaviourContext.BodyComp.Velocity;
                return true;
            }

            int tick = AchievementManager.GetTicks();
            List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockGroupA)
                || b.GetType() == typeof(BlockGroupIceA)
                || b.GetType() == typeof(BlockGroupSnowA)
                || b.GetType() == typeof(BlockGroupB)
                || b.GetType() == typeof(BlockGroupIceB)
                || b.GetType() == typeof(BlockGroupSnowB)
                || b.GetType() == typeof(BlockGroupC)
                || b.GetType() == typeof(BlockGroupIceC)
                || b.GetType() == typeof(BlockGroupSnowC)
                || b.GetType() == typeof(BlockGroupD)
                || b.GetType() == typeof(BlockGroupIceD)
                || b.GetType() == typeof(BlockGroupSnowD));
            foreach (IBlockGroupId block in blocks.Cast<IBlockGroupId>())
            {
                int groupId = block.GroupId;
                if (!DataGroup.GetState(groupId)
                    || DataGroup.Touched.Contains(groupId)
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                    prevVelocity,
                    SettingsGroup.PlatformDirections,
                    (IBlock)block))
                {
                    continue;
                }
                DataGroup.SetTick(groupId, tick + SettingsGroup.Duration);
                DataGroup.Touched.Add(groupId);
            }

            prevVelocity = behaviourContext.BodyComp.Velocity;
            return true;
        }
    }
}
