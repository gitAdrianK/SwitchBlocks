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
    using SwitchBlocks.Util;

    /// <summary>
    /// Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupSequence
    {
        /// <summary>Whether the sequence block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>The amount of groups created.</summary>
        public static int SequenceCount { get; private set; }
        /// <summary>Sequence A blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceA { get; private set; } = new Dictionary<int, IBlockGroupId>();
        /// <summary>Sequence B blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceB { get; private set; } = new Dictionary<int, IBlockGroupId>();
        /// <summary>Sequence C blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceC { get; private set; } = new Dictionary<int, IBlockGroupId>();
        /// <summary>Sequence D blocks.</summary>
        public static Dictionary<int, IBlockGroupId> BlocksSequenceD { get; private set; } = new Dictionary<int, IBlockGroupId>();

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

            var instance = DataSequence.Instance;
            var seeds = SeedsSequence.TryDeserialize();
            AssignSequenceIds(instance.Groups, seeds.Seeds);

            if (LevelDebugState.instance != null)
            {
                seeds.SaveToFile();
            }

            if (instance.Touched == 0)
            {
                if (instance.Groups.TryGetValue(1, out var group))
                {
                    group.ActivatedTick = int.MaxValue;
                }
                _ = instance.Active.Add(1);
            }

            var entityLogic = new EntityLogicSequence();
            FactoryDrawablesGroup.CreateDrawables(FactoryDrawablesGroup.BlockType.Sequence, entityLogic);

            var body = player.m_body;
            _ = body.RegisterBlockBehaviour(typeof(BlockSequenceA), new BehaviourSequencePlatform());
            _ = body.RegisterBlockBehaviour(typeof(BlockSequenceIceA), new BehaviourSequenceIce());
            _ = body.RegisterBlockBehaviour(typeof(BlockSequenceSnowA), new BehaviourSequenceSnow());
            _ = body.RegisterBlockBehaviour(typeof(BlockSequenceReset), new BehaviourSequenceReset());
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

            DataSequence.Instance.SaveToFile();
            DataSequence.Instance.Reset();

            IsUsed = false;
        }

        /// <summary>
        /// Assigns sequence ids to all sequence blocks.
        /// </summary>
        /// <param name="groups">Block groups to add groups to holding that groups data.</param>
        /// <param name="seeds">Seeds to use for assignment.</param>
        private static void AssignSequenceIds(Dictionary<int, BlockGroup> groups, Dictionary<int, int> seeds)
        {
            var sequenceId = 1;

            if (seeds.Count() != 0)
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

            // Increased one more time at the end and is thus one too high.
            SequenceCount = sequenceId - 1;
        }
    }
}
