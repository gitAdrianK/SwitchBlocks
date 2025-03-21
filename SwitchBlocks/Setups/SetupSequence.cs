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

    public static class SetupSequence
    {
        /// <summary>
        /// Whether the sequence block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;

        public static Dictionary<int, IBlockGroupId> BlocksSequenceA { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksSequenceB { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksSequenceC { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksSequenceD { get; private set; } = new Dictionary<int, IBlockGroupId>();

        public static int SequenceCount { get; private set; }

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
                instance.SetTick(1, int.MaxValue);
                _ = instance.Active.Add(1);
            }

            var entityLogic = new EntityLogicSequence();
            FactoryDrawablesGroup.CreateDrawables(FactoryDrawablesGroup.BlockType.Sequence, entityLogic);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceA), new BehaviourSequencePlatform());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceIceA), new BehaviourSequenceIce());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceSnowA), new BehaviourSequenceSnow());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceReset), new BehaviourSequenceReset());
        }

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

        private static void AssignSequenceIds(Dictionary<int, BlockGroup> groups, Dictionary<int, int> seeds)
        {
            var sequenceId = 1;

            if (seeds.Any())
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

            // The sequence id, that is how many groups got created,
            // is increased one more time at the end and is thus one too high.
            SequenceCount = sequenceId - 1;
        }
    }
}
