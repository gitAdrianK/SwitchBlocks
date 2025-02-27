namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using JumpKing;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;
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
        public static Dictionary<int, IResetGroupIds> Resets { get; private set; } = new Dictionary<int, IResetGroupIds>();

        public static void Setup(PlayerEntity player)
        {
            if (!SettingsGroup.IsUsed)
            {
                return;
            }

            var cache = CacheGroup.TryDeserialize();
            var resets = ResetsGroup.TryDeserialize();
            AssignGroupIds(cache, resets);

            if (LevelDebugState.instance != null)
            {
                cache.SaveToFile();
                resets.SaveToFile();
            }

            var entityLogic = new EntityLogicGroup();
            FactoryDrawablesGroup.CreateDrawables(FactoryDrawablesGroup.BlockType.Group, entityLogic);

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

        public static void Cleanup()
        {
            if (!SettingsGroup.IsUsed)
            {
                return;
            }

            DataGroup.Instance.SaveToFile();
            DataGroup.Instance.Reset();

            SettingsGroup.IsUsed = false;
        }

        private static void AssignGroupIds(CacheGroup cache, ResetsGroup resets)
        {
            var groupId = 1;

            var seed = cache.Seed;
            if (seed.Count > 0)
            {
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupA, seed, ref groupId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupB, seed, ref groupId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupC, seed, ref groupId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksGroupD, seed, ref groupId);
            }

            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupA, seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupB, seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupC, seed, ref groupId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksGroupD, seed, ref groupId);

            BlockGroup.CreateGroupData(groupId, DataGroup.Instance.Groups, true);

            var rseed = resets.Seed;
            if (rseed.Count > 0)
            {
                BlockGroup.AssignResetIdsFromSeed(Resets, rseed);
            }
            BlockGroup.CreateResetsData(Resets, rseed);
        }
    }
}
