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
using System.Threading.Tasks;

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
            /* For performance testing purposes
            const int cycles = 1000;
            long total = 0;
            for (int i = 0; i < cycles; i++)
            {
                long start = Stopwatch.GetTimestamp();
            */
            AssignGroupIds();
            /*
                total += Stopwatch.GetTimestamp() - start;
                Debugger.Log(1, "", "> " + i + "\n");
                DataGroup.Instance.Reset();
                CacheGroup.Instance.Reset();
                foreach (var item in BlocksGroupA.Values)
                {
                    item.GroupId = 0;
                }
                foreach (var item in BlocksGroupB.Values)
                {
                    item.GroupId = 0;
                }
                foreach (var item in BlocksGroupC.Values)
                {
                    item.GroupId = 0;
                }
                foreach (var item in BlocksGroupD.Values)
                {
                    item.GroupId = 0;
                }
            }
            Debugger.Log(1, "", ">>> Avg: " + (total / cycles) + "\n");
            */

            Task saving = Task.Run(() =>
            {
                CacheGroup.Instance.SaveToFile();
                CacheGroup.Instance.Reset();
            });

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

            saving.Wait();
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
        }

        private static void AssignGroupIds()
        {
            int groupId = 1;

            if (CacheGroup.Seed.Count > 0)
            {
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupA, CacheGroup.Seed, ref groupId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupB, CacheGroup.Seed, ref groupId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupC, CacheGroup.Seed, ref groupId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupD, CacheGroup.Seed, ref groupId);
            }

            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupA, CacheGroup.Seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupB, CacheGroup.Seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupC, CacheGroup.Seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupD, CacheGroup.Seed, ref groupId);

            BlockGroup.CreateGroupData(groupId, DataGroup.Groups, true);
        }
    }
}