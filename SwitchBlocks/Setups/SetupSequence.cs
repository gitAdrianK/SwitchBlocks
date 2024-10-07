using EntityComponent;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using SwitchBlocks.Data;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System.Collections.Generic;

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

            // Quite frankly I don't need to call them here, but I like it.
            _ = DataSequence.Instance;
            _ = CacheSequence.Instance;
            /*

                        int SequenceId = 1;
                        AssignSequenceIds(ref SequenceId);
                        CreateSequenceData(SequenceId);

                        _ = EntitySequencePlatforms.Instance;

                        IBlockBehaviour behaviourSequencePlatform;
                        if (SettingsSequence.Duration == 0)
                        {
                            behaviourSequencePlatform = new BehaviourSequenceLeaving();
                        }
                        else
                        {
                            behaviourSequencePlatform = new BehaviourSequenceDuration();
                        }
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
            */
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsSequence.IsUsed)
            {
                return;
            }
            /*

            entityManager.RemoveObject(EntitySequencePlatforms.Instance);
            EntitySequencePlatforms.Instance.Reset();

            DataSequence.Instance.SaveToFile();
            DataSequence.Instance.Reset();

            CacheSequence.Instance.SaveToFile();
            CacheSequence.Instance.Reset();
            */
        }

        private static void AssignSequenceIds(ref int SequenceId)
        {
            /*
            AssignSequenceIdFromSeed(ref SequenceId);

            Task taskSequenceA = Task.Run(() => BlocksSequenceA = BlocksSequenceA.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task taskSequenceB = Task.Run(() => BlocksSequenceB = BlocksSequenceB.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task taskSequenceC = Task.Run(() => BlocksSequenceC = BlocksSequenceC.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task taskSequenceD = Task.Run(() => BlocksSequenceD = BlocksSequenceD.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task.WaitAll(taskSequenceA, taskSequenceB, taskSequenceC, taskSequenceD);

            // Find the largest id already assigned from loaded data.
            if (DataSequence.Sequences.Count() > 0)
            {
                int largestDataId = DataSequence.Sequences.OrderByDescending(kv => kv.Key).First().Key;
                if (SequenceId <= largestDataId)
                {
                    SequenceId = largestDataId + 1;
                }
            }

            AssignSequenceIdsConsecutively(BlocksSequenceA, ref SequenceId);
            AssignSequenceIdsConsecutively(BlocksSequenceB, ref SequenceId);
            AssignSequenceIdsConsecutively(BlocksSequenceC, ref SequenceId);
            AssignSequenceIdsConsecutively(BlocksSequenceD, ref SequenceId);
            */
        }

        /// <summary>
        /// Sequences up all blocks next to eachother into a Sequence by assigning them the same ID.
        /// The ID is choosen consecutively ascending as new Sequences get created.
        /// </summary>
        /// <param name="blocks">The coordinates and blocks that are to be Sequenceed</param>
        private static void AssignSequenceIdsConsecutively(Dictionary<Vector3, IBlockGroupId> blocks, ref int SequenceId)
        {
            /*
            foreach (KeyValuePair<Vector3, IBlockSequenceId> kv in blocks)
            {
                Vector3 position = kv.Key;
                if (PropagateSequenceId(blocks, position, SequenceId))
                {
                    CacheSequence.Seed.Add(position, SequenceId);
                    SequenceId++;
                }
            }
            */
        }

        /// <summary>
        /// Sequences up all blocks next to eachother into a Sequence by assigning them the same ID.
        /// The ID is choosen by the given value belonging to the position in the cache.
        /// </summary>
        /// <param name="SequenceId">Reference to the Sequence ID, which will be larger than the largest ID found when finished</param>
        private static void AssignSequenceIdFromSeed(ref int SequenceId)
        {
            /*
            List<Vector3> failedPositions = new List<Vector3>();
            foreach (KeyValuePair<Vector3, int> kv in CacheSequence.Seed)
            {
                Vector3 currentPos = kv.Key;
                int cacheId = kv.Value;
                if (SequenceId <= cacheId)
                {
                    SequenceId = cacheId + 1;
                }
                bool result = false;
                if (BlocksSequenceA.ContainsKey(currentPos))
                {
                    result = PropagateSequenceId(BlocksSequenceA, currentPos, cacheId);
                }
                else if (BlocksSequenceB.ContainsKey(currentPos))
                {
                    result = PropagateSequenceId(BlocksSequenceB, currentPos, cacheId);
                }
                else if (BlocksSequenceC.ContainsKey(currentPos))
                {
                    result = PropagateSequenceId(BlocksSequenceC, currentPos, cacheId);
                }
                else if (BlocksSequenceD.ContainsKey(currentPos))
                {
                    result = PropagateSequenceId(BlocksSequenceD, currentPos, cacheId);
                }
                if (!result)
                {
                    failedPositions.Add(currentPos);
                }
            }
            foreach (Vector3 pos in failedPositions)
            {
                CacheSequence.Seed.Remove(pos);
            }
            */
        }

        /// <summary>
        /// Assigns the Sequence ID to the block and looks for neighbors of this block that are contained
        /// in the blocks dictionary and propagates the Sequence ID to those neighbor blocks.
        /// </summary>
        /// <param name="startPosition">The position from which the propagation is supposed to start</param>
        /// <param name="SequenceId">The ID that is to be assigned to all blocks of the Sequence</param>
        private static bool PropagateSequenceId(Dictionary<Vector3, IBlockGroupId> blocks, Vector3 startPosition, int SequenceId)
        {
            /*
            if (!blocks.ContainsKey(startPosition) || blocks[startPosition].SequenceId != 0)
            {
                return false;
            }
            Queue<Vector3> toVisit = new Queue<Vector3>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count != 0)
            {
                Vector3 currentPos = toVisit.Dequeue();
                blocks[currentPos].SequenceId = SequenceId;

                // Left
                Vector3 left = currentPos + new Vector3(-1, 0, 0);
                if (blocks.ContainsKey(left) && blocks[left].SequenceId == 0)
                {
                    toVisit.Enqueue(left);
                }
                // Right
                Vector3 right = currentPos + new Vector3(1, 0, 0);
                if (blocks.ContainsKey(right) && blocks[right].SequenceId == 0)
                {
                    toVisit.Enqueue(right);
                }
                // Up
                Vector3 up = currentPos + new Vector3(0, -1, 0);
                if (up.Y == -1)
                {
                    up = new Vector3(currentPos.X, 44, currentPos.Z + 1);
                }
                if (blocks.ContainsKey(up) && blocks[up].SequenceId == 0)
                {
                    toVisit.Enqueue(up);
                }
                // Down
                Vector3 down = currentPos + new Vector3(0, 1, 0);
                if (down.Y == 45)
                {
                    down = new Vector3(currentPos.X, 0, currentPos.Z - 1);
                }
                if (blocks.ContainsKey(down) && blocks[down].SequenceId == 0)
                {
                    toVisit.Enqueue(down);
                }
            }
            */
            return true;
        }

        /// <summary>
        /// Ensures that there is Sequence data for all IDs up to the given Sequence ID.
        /// </summary>
        /// <param name="SequenceId">The Sequence ID that data is to be created up to for (excluding)</param>
        private static void CreateSequenceData(int SequenceId)
        {
            /*
            for (int i = 1; i < SequenceId; i++)
            {
                if (!DataSequence.Sequences.ContainsKey(i))
                {
                    DataSequence.Sequences.Add(i, new DataSequence.Sequence());
                }
            }
            */
        }
    }
}