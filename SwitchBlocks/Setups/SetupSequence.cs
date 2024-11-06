using EntityComponent;
using JumpKing.Player;
using Microsoft.Xna.Framework;
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
        public static Dictionary<Vector3, IBlockGroupId> BlocksSequenceA { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksSequenceB { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksSequenceC { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksSequenceD { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsSequence.IsUsed)
            {
                return;
            }

            AssignSequenceIds();

            Task saving = Task.Run(() =>
            {
                CacheSequence.Instance.SaveToFile();
                CacheSequence.Instance.Reset();
            });

            if (DataSequence.Touched == 0)
            {
                DataSequence.SetTick(1, Int32.MaxValue);
            }

            _ = EntitySequencePlatforms.Instance;

            BehaviourSequencePlatform behaviourSequencePlatform = new BehaviourSequencePlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceA), behaviourSequencePlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceB), behaviourSequencePlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceC), behaviourSequencePlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceD), behaviourSequencePlatform);

            BehaviourSequenceIceA behaviourSequenceIceA = new BehaviourSequenceIceA();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceIceA), behaviourSequenceIceA);
            BehaviourSequenceIceB behaviourSequenceIceB = new BehaviourSequenceIceB();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceIceB), behaviourSequenceIceB);
            BehaviourSequenceIceC behaviourSequenceIceC = new BehaviourSequenceIceC();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceIceC), behaviourSequenceIceC);
            BehaviourSequenceIceD behaviourSequenceIceD = new BehaviourSequenceIceD();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceIceD), behaviourSequenceIceD);

            BehaviourSequenceSnow behaviourSequenceSnow = new BehaviourSequenceSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceSnowA), behaviourSequenceSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceSnowB), behaviourSequenceSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceSnowC), behaviourSequenceSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceSnowD), behaviourSequenceSnow);

            BehaviourSequenceReset behaviourSequenceReset = new BehaviourSequenceReset();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceReset), behaviourSequenceReset);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSequenceResetSolid), behaviourSequenceReset);

            saving.Wait();
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
        }
    }
}