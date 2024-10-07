using EntityComponent;
using JumpKing.API;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBlocks.Setups
{
    public static class SetupGroup
    {
        // The Groups cannot be reset on start or end as the factory only runs when a new level is loaded
        // clearing would result in the dict being empty on same level reload.
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupA { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupB { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupC { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupD { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsGroup.IsUsed)
            {
                return;
            }

            // Quite frankly I don't need to call them here, but I like it.
            _ = DataGroup.Instance;
            _ = CacheGroup.Instance;

            int groupId = 1;
            AssignGroupIds(ref groupId);
            CreateGroupData(groupId);

            _ = EntityGroupPlatforms.Instance;

            IBlockBehaviour behaviourGroupPlatform;
            if (SettingsGroup.Duration == 0)
            {
                behaviourGroupPlatform = new BehaviourGroupLeaving();
            }
            else
            {
                behaviourGroupPlatform = new BehaviourGroupDuration();
            }
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupA), behaviourGroupPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupB), behaviourGroupPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupC), behaviourGroupPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupD), behaviourGroupPlatform);

            BehaviourGroupIceA behaviourGroupIceA = new BehaviourGroupIceA();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceA), behaviourGroupIceA);
            BehaviourGroupIceB behaviourGroupIceB = new BehaviourGroupIceB();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceB), behaviourGroupIceB);
            BehaviourGroupIceC behaviourGroupIceC = new BehaviourGroupIceC();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceC), behaviourGroupIceC);
            BehaviourGroupIceD behaviourGroupIceD = new BehaviourGroupIceD();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceD), behaviourGroupIceD);

            BehaviourGroupSnow behaviourGroupSnow = new BehaviourGroupSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowA), behaviourGroupSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowB), behaviourGroupSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowC), behaviourGroupSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowD), behaviourGroupSnow);

            BehaviourGroupReset behaviourGroupReset = new BehaviourGroupReset();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupReset), behaviourGroupReset);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupResetSolid), behaviourGroupReset);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsGroup.IsUsed)
            {
                return;
            }

            entityManager.RemoveObject(EntityGroupPlatforms.Instance);
            EntityGroupPlatforms.Instance.Reset();

            DataGroup.Instance.SaveToFile();
            DataGroup.Instance.Reset();

            CacheGroup.Instance.SaveToFile();
            CacheGroup.Instance.Reset();
        }

        private static void AssignGroupIds(ref int groupId)
        {
            AssignGroupIdFromSeed(ref groupId);

            Task taskGroupA = Task.Run(() => BlocksGroupA = BlocksGroupA.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task taskGroupB = Task.Run(() => BlocksGroupB = BlocksGroupB.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task taskGroupC = Task.Run(() => BlocksGroupC = BlocksGroupC.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task taskGroupD = Task.Run(() => BlocksGroupD = BlocksGroupD.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value));
            Task.WaitAll(taskGroupA, taskGroupB, taskGroupC, taskGroupD);

            // Find the largest id already assigned from loaded data.
            if (DataGroup.Groups.Count() > 0)
            {
                int largestDataId = DataGroup.Groups.OrderByDescending(kv => kv.Key).First().Key;
                if (groupId <= largestDataId)
                {
                    groupId = largestDataId + 1;
                }
            }

            AssignGroupIdsConsecutively(BlocksGroupA, ref groupId);
            AssignGroupIdsConsecutively(BlocksGroupB, ref groupId);
            AssignGroupIdsConsecutively(BlocksGroupC, ref groupId);
            AssignGroupIdsConsecutively(BlocksGroupD, ref groupId);
        }

        /// <summary>
        /// Groups up all blocks next to eachother into a group by assigning them the same ID.
        /// The ID is choosen consecutively ascending as new groups get created.
        /// </summary>
        /// <param name="blocks">The coordinates and blocks that are to be grouped</param>
        private static void AssignGroupIdsConsecutively(Dictionary<Vector3, IBlockGroupId> blocks, ref int groupId)
        {
            foreach (KeyValuePair<Vector3, IBlockGroupId> kv in blocks)
            {
                Vector3 position = kv.Key;
                if (PropagateGroupId(blocks, position, groupId))
                {
                    CacheGroup.Seed.Add(position, groupId);
                    groupId++;
                }
            }
        }

        /// <summary>
        /// Groups up all blocks next to eachother into a group by assigning them the same ID.
        /// The ID is choosen by the given value belonging to the position in the cache.
        /// </summary>
        /// <param name="groupId">Reference to the group ID, which will be larger than the largest ID found when finished</param>
        private static void AssignGroupIdFromSeed(ref int groupId)
        {
            List<Vector3> failedPositions = new List<Vector3>();
            foreach (KeyValuePair<Vector3, int> kv in CacheGroup.Seed)
            {
                Vector3 currentPos = kv.Key;
                int cacheId = kv.Value;
                if (groupId <= cacheId)
                {
                    groupId = cacheId + 1;
                }
                bool result = false;
                if (BlocksGroupA.ContainsKey(currentPos))
                {
                    result = PropagateGroupId(BlocksGroupA, currentPos, cacheId);
                }
                else if (BlocksGroupB.ContainsKey(currentPos))
                {
                    result = PropagateGroupId(BlocksGroupB, currentPos, cacheId);
                }
                else if (BlocksGroupC.ContainsKey(currentPos))
                {
                    result = PropagateGroupId(BlocksGroupC, currentPos, cacheId);
                }
                else if (BlocksGroupD.ContainsKey(currentPos))
                {
                    result = PropagateGroupId(BlocksGroupD, currentPos, cacheId);
                }
                if (!result)
                {
                    failedPositions.Add(currentPos);
                }
            }
            foreach (Vector3 pos in failedPositions)
            {
                CacheGroup.Seed.Remove(pos);
            }
        }

        /// <summary>
        /// Assigns the group ID to the block and looks for neighbors of this block that are contained
        /// in the blocks dictionary and propagates the group ID to those neighbor blocks.
        /// </summary>
        /// <param name="startPosition">The position from which the propagation is supposed to start</param>
        /// <param name="groupId">The ID that is to be assigned to all blocks of the group</param>
        private static bool PropagateGroupId(Dictionary<Vector3, IBlockGroupId> blocks, Vector3 startPosition, int groupId)
        {
            if (!blocks.ContainsKey(startPosition) || blocks[startPosition].GroupId != 0)
            {
                return false;
            }
            Queue<Vector3> toVisit = new Queue<Vector3>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count != 0)
            {
                Vector3 currentPos = toVisit.Dequeue();
                blocks[currentPos].GroupId = groupId;

                // Left
                Vector3 left = currentPos + new Vector3(-1, 0, 0);
                if (blocks.ContainsKey(left) && blocks[left].GroupId == 0)
                {
                    toVisit.Enqueue(left);
                }
                // Right
                Vector3 right = currentPos + new Vector3(1, 0, 0);
                if (blocks.ContainsKey(right) && blocks[right].GroupId == 0)
                {
                    toVisit.Enqueue(right);
                }
                // Up
                Vector3 up = currentPos + new Vector3(0, -1, 0);
                if (up.Y == -1)
                {
                    up = new Vector3(currentPos.X, 44, currentPos.Z + 1);
                }
                if (blocks.ContainsKey(up) && blocks[up].GroupId == 0)
                {
                    toVisit.Enqueue(up);
                }
                // Down
                Vector3 down = currentPos + new Vector3(0, 1, 0);
                if (down.Y == 45)
                {
                    down = new Vector3(currentPos.X, 0, currentPos.Z - 1);
                }
                if (blocks.ContainsKey(down) && blocks[down].GroupId == 0)
                {
                    toVisit.Enqueue(down);
                }
            }
            return true;
        }

        /// <summary>
        /// Ensures that there is group data for all IDs up to the given group ID.
        /// </summary>
        /// <param name="groupId">The group ID that data is to be created up to for (excluding)</param>
        private static void CreateGroupData(int groupId)
        {
            for (int i = 1; i < groupId; i++)
            {
                if (!DataGroup.Groups.ContainsKey(i))
                {
                    DataGroup.Groups.Add(i, new BlockGroup(true));
                }
            }
        }
    }
}