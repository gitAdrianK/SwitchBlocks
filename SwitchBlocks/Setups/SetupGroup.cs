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

    /// <summary>
    /// Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupGroup
    {
        /// <summary>Whether the group block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>Group A blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupA { get; private set; } = new Dictionary<int, IBlockGroupId>();
        /// <summary>Group B blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupB { get; private set; } = new Dictionary<int, IBlockGroupId>();
        /// <summary>Group C blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupC { get; private set; } = new Dictionary<int, IBlockGroupId>();
        /// <summary>Group D blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupD { get; private set; } = new Dictionary<int, IBlockGroupId>();
        /// <summary>Group Reset blocks.</summary>
        public static Dictionary<int, IResetGroupIds> Resets { get; private set; } = new Dictionary<int, IResetGroupIds>();

        // The Groups cannot be reset on start or end as the factory only runs when a new level is loaded
        // clearing would result in the dict being empty on same level reload.
        // Or can they...

        /// <summary>
        /// Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="player">Player to register block behaviours to.</param>
        public static void Setup(PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            var seeds = SeedsGroup.TryDeserialize();
            var resets = ResetsGroup.TryDeserialize();
            AssignGroupIds(DataGroup.Instance.Groups, seeds.Seeds, resets.Resets);

            if (LevelDebugState.instance != null)
            {
                seeds.SaveToFile();
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

        /// <summary>
        /// Cleans up saving data, resetting fields and does other required actions.
        /// </summary>
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

        /// <summary>
        /// Assigns group ids to all groups blocks.
        /// </summary>
        /// <param name="groups">Block groups to add groups to holding that groups data.</param>
        /// <param name="seeds">Seeds to use for assignment.</param>
        /// <param name="resets">Positions to add reset ids to reset blocks to.</param>
        private static void AssignGroupIds(Dictionary<int, BlockGroup> groups, Dictionary<int, int> seeds, Dictionary<int, int[]> resets)
        {
            var groupId = 1;

            if (seeds.Count() != 0)
            {
                BlockGroupId.AssignGroupIdsFromSeed(
                    seeds,
                    ref groupId,
                    BlocksGroupA,
                    BlocksGroupB,
                    BlocksGroupC,
                    BlocksGroupD);
            }

            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupA, seeds, ref groupId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupB, seeds, ref groupId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupC, seeds, ref groupId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksGroupD, seeds, ref groupId);

            BlockGroup.CreateGroupData(groupId, groups, true);

            if (resets.Count() != 0)
            {
                ResetGroupIds.AssignResetIdsFromSeed(Resets, resets);
            }
            ResetGroupIds.AssignOtherResets(Resets, resets);
        }
    }
}
