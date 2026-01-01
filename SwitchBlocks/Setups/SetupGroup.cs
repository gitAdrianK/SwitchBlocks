namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using Behaviours;
    using Blocks;
    using Data;
    using Entities;
    using Factories;
    using JumpKing;
    using JumpKing.Player;
    using Settings;
    using Util;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupGroup
    {
        /// <summary>Whether the group block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        // The Dictionaries are static because the setup step is after the block factories have run.
        // So we can't contain them to the setup step.

        /// <summary>Group A blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupA { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Group B blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupB { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Group C blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupC { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Group D blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksGroupD { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Group Reset blocks.</summary>
        public static Dictionary<int, IResetGroupIds> Resets { get; } = new Dictionary<int, IResetGroupIds>();

        // The Groups cannot be reset on start or end as the factory only runs when a new level is loaded
        // clearing would result in the dict being empty on same level reload.
        // Or can they...

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="settings">Settings of the group type.</param>
        /// <param name="player">Player to register block behaviours to.</param>
        public static void Setup(SettingsGroup settings, PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            var seeds = SeedsGroup.TryDeserialize();
            var resets = ResetsGroup.TryDeserialize();
            AssignGroupIds(DataGroup.Instance.Groups, seeds.Seeds, resets.Resets);

            var entityLogic = new EntityLogicGroup(settings);
            FactoryDrawablesGroup.CreateDrawables(FactoryDrawablesGroup.BlockType.Group, entityLogic);

            if (LevelDebugState.instance == null)
            {
                BlocksGroupA.Clear();
                BlocksGroupB.Clear();
                BlocksGroupC.Clear();
                BlocksGroupD.Clear();
                Resets.Clear();
            }
            else
            {
                seeds.SaveToFile();
                resets.SaveToFile();
            }

            var body = player.m_body;
            _ = settings.Duration == 0
                ? body.RegisterBlockBehaviour(typeof(BlockGroupA),
                    new BehaviourGroupLeaving(settings.PlatformDirections))
                : body.RegisterBlockBehaviour(typeof(BlockGroupA),
                    new BehaviourGroupDuration(settings.Duration, settings.PlatformDirections));

            _ = body.RegisterBlockBehaviour(typeof(BlockGroupIceA), new BehaviourGroupIce());
            _ = body.RegisterBlockBehaviour(typeof(BlockGroupSnowA), new BehaviourGroupSnow());
            _ = body.RegisterBlockBehaviour(typeof(BlockGroupReset), new BehaviourGroupReset(settings.LeverDirections));
        }

        /// <summary>
        ///     Cleans up saving data, resetting fields and does other required actions.
        /// </summary>
        public static void Cleanup()
        {
            if (!IsUsed)
            {
                return;
            }

            DataGroup.Instance.SaveToFile();
            DataGroup.Reset();

            IsUsed = false;
        }

        /// <summary>
        ///     Assigns group IDs to all groups blocks.
        /// </summary>
        /// <param name="groups">Block groups to add groups to holding that groups data.</param>
        /// <param name="seeds">Seeds to use for assignment.</param>
        /// <param name="resets">Positions to add reset IDs to reset blocks to.</param>
        private static void AssignGroupIds(Dictionary<int, BlockGroup> groups, Dictionary<int, int> seeds,
            Dictionary<int, int[]> resets)
        {
            var groupId = 1;

            if (seeds.Count != 0)
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

            if (resets.Count != 0)
            {
                ResetGroupIds.AssignResetIdsFromSeed(Resets, resets);
            }

            ResetGroupIds.AssignOtherResets(Resets, resets);
        }
    }
}
