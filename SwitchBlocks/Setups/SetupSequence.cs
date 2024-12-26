using EntityComponent;
using JumpKing;
using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBlocks.Setups
{
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

            ModEntry.tasks.Add(Task.Run(() =>
            {
                if (LevelDebugState.instance != null)
                {
                    CacheSequence.Instance.SaveToFile();
                }
                CacheSequence.Instance.Reset();
            }));

            if (DataSequence.Touched == 0)
            {
                DataSequence.SetTick(1, Int32.MaxValue);
                DataSequence.Active.Add(1);
            }

            _ = EntitySequencePlatforms.Instance;

            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceA), new BehaviourSequencePlatform());
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceIceA), new BehaviourSequenceIce());
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceSnowA), new BehaviourSequenceSnow());
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceReset), new BehaviourSequenceReset());
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
            int sequenceId = 1;

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
