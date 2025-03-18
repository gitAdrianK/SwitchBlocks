namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using System.Linq;
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
        /// <summary>
        /// Whether the group block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;

        // The Groups cannot be reset on start or end as the factory only runs when a new level is loaded
        // clearing would result in the dict being empty on same level reload.
        public static Dictionary<int, IBlockGroupId> BlocksGroupA { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksGroupB { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksGroupC { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksGroupD { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IResetGroupIds> Resets { get; private set; } = new Dictionary<int, IResetGroupIds>();

        public static void Setup(PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            var seed = SeedsGroup.TryDeserialize();
            var resets = ResetsGroup.TryDeserialize();
            AssignGroupIds(seed, resets);

            if (LevelDebugState.instance != null)
            {
                seed.SaveToFile();
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
            if (!IsUsed)
            {
                return;
            }

            DataGroup.Instance.SaveToFile();
            DataGroup.Instance.Reset();

            IsUsed = false;
        }

        private static void AssignGroupIds(SeedsGroup seeds, ResetsGroup resets)
        {
            var groupId = 1;

            var seed = seeds.Seeds;
            if (seed.Any())
            {
                BlockGroupId.AssignGroupIdsFromSeed(
                    seed,
                    ref groupId,
                    BlocksGroupA,
                    BlocksGroupB,
                    BlocksGroupC,
                    BlocksGroupD);
            }

            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupA, seed, ref groupId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupB, seed, ref groupId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupC, seed, ref groupId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupD, seed, ref groupId);

            BlockGroup.CreateGroupData(groupId, DataGroup.Instance.Groups, true);

            var rseed = resets.Resets;
            if (rseed.Any())
            {
                ResetGroupIds.AssignResetIdsFromSeed(Resets, rseed);
            }
            ResetGroupIds.AssignOtherResets(Resets, rseed);
        }
    }
}
