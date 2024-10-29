using EntityComponent;
using JumpKing.API;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System.Collections.Generic;

namespace SwitchBlocks.Setups
{
    public static class SetupGroup
    {
        // The Groups cannot be reset on start or end as the factory only runs when a new level is loaded
        // clearing would result in the dict being empty on same level reload.
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupA { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupB { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupC { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupD { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsGroup.IsUsed)
            {
                return;
            }

            // Quite frankly I don't need to call them here, but I like it.
            _ = DataGroup.Instance;
            _ = CacheGroup.Instance;

            int groupId = 1;
            AssignGroupIds(ref groupId);
            BlockGroup.CreateGroupData(groupId, DataGroup.Groups, true);

            _ = EntityGroupPlatforms.Instance;

            IBlockBehaviour behaviourGroupPlatform;
            if (SettingsGroup.Duration == 0)
            {
                behaviourGroupPlatform = new BehaviourGroupLeaving();
            }
            else
            {
                behaviourGroupPlatform = new BehaviourGroupDuration();
            }
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupA), behaviourGroupPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupB), behaviourGroupPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupC), behaviourGroupPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupD), behaviourGroupPlatform);

            BehaviourGroupIceA behaviourGroupIceA = new BehaviourGroupIceA();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceA), behaviourGroupIceA);
            BehaviourGroupIceB behaviourGroupIceB = new BehaviourGroupIceB();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceB), behaviourGroupIceB);
            BehaviourGroupIceC behaviourGroupIceC = new BehaviourGroupIceC();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceC), behaviourGroupIceC);
            BehaviourGroupIceD behaviourGroupIceD = new BehaviourGroupIceD();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceD), behaviourGroupIceD);

            BehaviourGroupSnow behaviourGroupSnow = new BehaviourGroupSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowA), behaviourGroupSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowB), behaviourGroupSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowC), behaviourGroupSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowD), behaviourGroupSnow);

            BehaviourGroupReset behaviourGroupReset = new BehaviourGroupReset();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupReset), behaviourGroupReset);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupResetSolid), behaviourGroupReset);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsGroup.IsUsed)
            {
                return;
            }

            entityManager.RemoveObject(EntityGroupPlatforms.Instance);
            EntityGroupPlatforms.Instance.Reset();

            DataGroup.Instance.SaveToFile();
            DataGroup.Instance.Reset();

            CacheGroup.Instance.SaveToFile();
            CacheGroup.Instance.Reset();
        }

        private static void AssignGroupIds(ref int groupId)
        {
            if (CacheGroup.Seed.Count != 0)
            {
                BlockGroup.AssignGroupIdFromSeed(BlocksGroupA, CacheGroup.Seed, ref groupId);
                BlockGroup.AssignGroupIdFromSeed(BlocksGroupB, CacheGroup.Seed, ref groupId);
                BlockGroup.AssignGroupIdFromSeed(BlocksGroupC, CacheGroup.Seed, ref groupId);
                BlockGroup.AssignGroupIdFromSeed(BlocksGroupD, CacheGroup.Seed, ref groupId);
            }

            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupA, CacheGroup.Seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupB, CacheGroup.Seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupC, CacheGroup.Seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupD, CacheGroup.Seed, ref groupId);
        }
    }
}