namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EntityComponent;
    using JumpKing;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Util;

    public static class SetupGroup
    {
        // The Groups cannot be reset on start or end as the factory only runs when a new level is loaded
        // clearing would result in the dict being empty on same level reload.
        public static Dictionary<int, IBlockGroupId> BlocksGroupA { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksGroupB { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksGroupC { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksGroupD { get; private set; } = new Dictionary<int, IBlockGroupId>();

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsGroup.IsUsed)
            {
                return;
            }

            AssignGroupIds();

            ModEntry.Tasks.Add(Task.Run(() =>
            {
                if (LevelDebugState.instance != null)
                {
                    CacheGroup.Instance.SaveToFile();
                }
                CacheGroup.Instance.Reset();
            }));

            _ = EntityGroupPlatforms.Instance;

            if (SettingsGroup.Duration == 0)
            {
                _ = player.m_body.RegisterBlockBehaviour(typeof(BlockGroupA), new BehaviourGroupLeaving());
            }
            else
            {
                _ = player.m_body.RegisterBlockBehaviour(typeof(BlockGroupA), new BehaviourGroupDuration());
            }
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceA), new BehaviourGroupIce());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowA), new BehaviourGroupSnow());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockGroupReset), new BehaviourGroupReset());
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

            SettingsGroup.IsUsed = false;
        }

        private static void AssignGroupIds()
        {
            var groupId = 1;

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
