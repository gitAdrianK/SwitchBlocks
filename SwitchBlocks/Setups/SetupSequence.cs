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
    using JumpKing.SaveThread;
    using Settings;
    using Util;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupSequence
    {
        /// <summary>Whether the sequence block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>Sequence A blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceA { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Sequence B blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceB { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Sequence C blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceC { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Sequence D blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceD { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Group Reset blocks.</summary>
        public static Dictionary<int, IResetGroupIds> Resets { get; } = new Dictionary<int, IResetGroupIds>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="settings">Settings of the sequence type.</param>
        /// <param name="player">Player to register block behaviours to.</param>
        public static void Setup(SettingsSequence settings, PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            var instance = DataSequence.Instance;
            var seeds = SeedsSequence.TryDeserialize();
            var resets = ResetsSequence.TryDeserialize();
            AssignSequenceIds(instance.Groups, seeds.Seeds, resets.Resets);

            if (SaveManager.instance.IsNewGame)
            {
                foreach (var defaultId in settings.DefaultActive)
                {
                    if (instance.Groups.TryGetValue(defaultId, out var group))
                    {
                        group.ActivatedTick = int.MaxValue;
                    }

                    _ = instance.Active.Add(defaultId);
                }
            }

            if (LevelDebugState.instance != null)
            {
                seeds.SaveToFile();
                resets.SaveToFile();
            }

            var entityLogic = new EntityLogicSequence(settings);
            FactoryDrawablesGroup.CreateDrawables(FactoryDrawablesGroup.BlockType.Sequence, entityLogic);

            var body = player.m_body;
            _ = settings.Duration == 0
                ? body.RegisterBlockBehaviour(typeof(BlockSequenceA),
                    new BehaviourSequenceTouching(settings.DisableOnLeaving, settings.PlatformDirections))
                : body.RegisterBlockBehaviour(typeof(BlockSequenceA),
                    new BehaviourSequenceDuration(settings.Duration, settings.PlatformDirections));

            _ = body.RegisterBlockBehaviour(typeof(BlockSequenceIceA), new BehaviourSequenceIce());
            _ = body.RegisterBlockBehaviour(typeof(BlockSequenceSnowA), new BehaviourSequenceSnow());
            _ = body.RegisterBlockBehaviour(typeof(BlockSequenceReset),
                new BehaviourSequenceReset(settings.DefaultActive, settings.LeverDirections));
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

            DataSequence.Instance.SaveToFile();
            DataSequence.Reset();

            IsUsed = false;
        }

        /// <summary>
        ///     Assigns sequence IDs to all sequence blocks.
        /// </summary>
        /// <param name="groups">Block groups to add groups to holding that groups data.</param>
        /// <param name="seeds">Seeds to use for assignment.</param>
        /// <param name="resets">Positions to add reset IDs to reset blocks to.</param>
        private static void AssignSequenceIds(Dictionary<int, BlockGroup> groups, Dictionary<int, int> seeds,
            Dictionary<int, int[]> resets)
        {
            var sequenceId = 1;

            if (seeds.Count != 0)
            {
                BlockGroupId.AssignGroupIdsFromSeed(
                    seeds,
                    ref sequenceId,
                    BlocksSequenceA,
                    BlocksSequenceB,
                    BlocksSequenceC,
                    BlocksSequenceD);
            }

            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceA, seeds, ref sequenceId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceB, seeds, ref sequenceId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceC, seeds, ref sequenceId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceD, seeds, ref sequenceId);

            BlockGroup.CreateGroupData(sequenceId, groups, false);

            if (resets.Count != 0)
            {
                ResetGroupIds.AssignResetIdsFromSeed(Resets, resets);
            }

            ResetGroupIds.AssignOtherResets(Resets, resets);
        }
    }
}
