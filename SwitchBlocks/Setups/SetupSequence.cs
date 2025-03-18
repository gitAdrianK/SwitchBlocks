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
            AssignSequenceIds(instance, seeds);

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

            var instance = DataSequence.Instance;
            instance.SaveToFile();
            instance.Reset();

            IsUsed = false;
        }

        private static void AssignSequenceIds(DataSequence instance, SeedsSequence seeds)
        {
            var sequenceId = 1;
            var seed = seeds.Seeds;

            if (seed.Any())
            {
                BlockGroupId.AssignGroupIdsFromSeed(
                    seed,
                    ref sequenceId,
                    BlocksSequenceA,
                    BlocksSequenceB,
                    BlocksSequenceC,
                    BlocksSequenceD);
            }

            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceA, seed, ref sequenceId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceB, seed, ref sequenceId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceC, seed, ref sequenceId);
            BlockGroupId.AssignGroupIdsConsecutively(BlocksSequenceD, seed, ref sequenceId);

            BlockGroup.CreateGroupData(sequenceId, instance.Groups, false);

            // The sequence id, that is how many groups got created,
            // is increased one more time at the end and is thus one too high.
            SequenceCount = sequenceId - 1;
        }
    }
}
