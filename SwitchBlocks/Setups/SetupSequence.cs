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

    public static class SetupSequence
    {
        public static Dictionary<int, IBlockGroupId> BlocksSequenceA { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksSequenceB { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksSequenceC { get; private set; } = new Dictionary<int, IBlockGroupId>();
        public static Dictionary<int, IBlockGroupId> BlocksSequenceD { get; private set; } = new Dictionary<int, IBlockGroupId>();

        public static int SequenceCount { get; private set; }

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsSequence.IsUsed)
            {
                return;
            }

            AssignSequenceIds();

            ModEntry.Tasks.Add(Task.Run(() =>
            {
                if (LevelDebugState.instance != null)
                {
                    CacheSequence.Instance.SaveToFile();
                }
                CacheSequence.Instance.Reset();
            }));

            if (DataSequence.Touched == 0)
            {
                DataSequence.SetTick(1, int.MaxValue);
                _ = DataSequence.Active.Add(1);
            }

            _ = EntitySequencePlatforms.Instance;

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceA), new BehaviourSequencePlatform());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceIceA), new BehaviourSequenceIce());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceSnowA), new BehaviourSequenceSnow());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceReset), new BehaviourSequenceReset());
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsSequence.IsUsed)
            {
                return;
            }

            entityManager.RemoveObject(EntitySequencePlatforms.Instance);
            EntitySequencePlatforms.Instance.Reset();

            DataSequence.Instance.SaveToFile();
            DataSequence.Instance.Reset();

            SettingsSequence.IsUsed = false;
        }

        private static void AssignSequenceIds()
        {
            var sequenceId = 1;

            if (CacheSequence.Seed.Count > 0)
            {
                BlockGroup.AssignGroupIdsFromSeed(BlocksSequenceA, CacheSequence.Seed, ref sequenceId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksSequenceB, CacheSequence.Seed, ref sequenceId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksSequenceC, CacheSequence.Seed, ref sequenceId);
                BlockGroup.AssignGroupIdsFromSeed(BlocksSequenceD, CacheSequence.Seed, ref sequenceId);
            }

            BlockGroup.AssignGroupIdsConsecutively(BlocksSequenceA, CacheSequence.Seed, ref sequenceId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksSequenceB, CacheSequence.Seed, ref sequenceId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksSequenceC, CacheSequence.Seed, ref sequenceId);
            BlockGroup.AssignGroupIdsConsecutively(BlocksSequenceD, CacheSequence.Seed, ref sequenceId);

            BlockGroup.CreateGroupData(sequenceId, DataSequence.Groups, false);

            SequenceCount = sequenceId - 1;
        }
    }
}
