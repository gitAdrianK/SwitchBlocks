using System;
using System.Collections.Generic;

namespace SwitchBlocks.Util
{
    [Serializable]
    public class BlockGroup
    {
        // Position stored in a single integer.
        // X and Y can never be a three digit number.
        // Screen can never be a four digit number.
        // As such the integers form is 00...00SSSXXYY.

        // Top left is (0, 0)
        // Bottom right is (59, 44)

        // To move 1 up or down is to change the integer by 1
        // - is up, + is down
        private const int VERTICAL = 1;
        // To move 1 left or right is to change the integer by 100
        // - is left, + is right
        private const int HORIZONTAL = 100;
        // To move 1 screen is to change the integer by 10044
        // - is previous, + is next
        private const int SCREEN = 10044;

        public bool State { get; set; }
        public float Progress { get; set; }
        public int ActivatedTick { get; set; }

        public BlockGroup() : this(true)
        {
        }

        public BlockGroup(bool isEnabled)
        {
            State = isEnabled;
            Progress = isEnabled ? 1.0f : 0.0f;
            ActivatedTick = isEnabled ? Int32.MaxValue : Int32.MinValue;
        }

        /// <summary>
        /// Assigns the group ID to the block and looks for neighbors of this block that are contained
        /// in the blocks dictionary and propagates the group ID to those neighbor blocks.
        /// </summary>
        /// <param name="startPosition">The position from which the propagation is supposed to start</param>
        /// <param name="groupId">The ID that is to be assigned to all blocks of the group</param>
        public static bool PropagateGroupId(Dictionary<int, IBlockGroupId> blocks, int startPosition, int groupId)
        {
            if (!blocks.TryGetValue(startPosition, out IBlockGroupId value) || value.GroupId != 0)
            {
                return false;
            }
            Queue<int> toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count != 0)
            {
                int currentPos = toVisit.Dequeue();
                blocks[currentPos].GroupId = groupId;

                // Left
                int left = currentPos - HORIZONTAL;
                if (blocks.TryGetValue(left, out value) && value.GroupId == 0)
                {
                    toVisit.Enqueue(left);
                }
                // Right
                int right = currentPos + HORIZONTAL;
                if (blocks.TryGetValue(right, out value) && value.GroupId == 0)
                {
                    toVisit.Enqueue(right);
                }
                // Up
                int up;
                if (currentPos % 100 == 0)
                {
                    up = currentPos + SCREEN;
                }
                else
                {
                    up = currentPos - VERTICAL;
                }
                if (blocks.TryGetValue(up, out value) && value.GroupId == 0)
                {
                    toVisit.Enqueue(up);
                }
                // Down
                int down;
                if (currentPos % 100 == 44)
                {
                    down = currentPos - SCREEN;
                }
                else
                {
                    down = currentPos + VERTICAL;
                }
                if (blocks.TryGetValue(down, out value) && value.GroupId == 0)
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
        /// <param name="groups">The dictionary containing groups that is to be added to</param>
        /// <param name="startState">The start state of the added platforms</param>
        public static void CreateGroupData(int groupId, SerializableDictionary<int, BlockGroup> groups, bool startState)
        {
            for (int i = 1; i < groupId; i++)
            {
                if (!groups.ContainsKey(i))
                {
                    groups.Add(i, new BlockGroup(startState));
                }
            }
        }

        // The caching is maybe too much for one function, but I don't care until I have to
        /// <summary>
        /// Groups up all blocks next to each other into a group by assigning them the same ID.
        /// The ID is chosen consecutively ascending as new groups get created.
        /// </summary>
        /// <param name="blocks">The coordinates and blocks that are to be grouped</param>
        /// <param name="seed">A cache that one position for a created block group is saved to</param>
        /// <param name="groupId">The group ID that is increased as new groups get created</param>
        public static void AssignGroupIdsConsecutively(Dictionary<int, IBlockGroupId> blocks, SerializableDictionary<int, int> seed, ref int groupId)
        {
            foreach (KeyValuePair<int, IBlockGroupId> kv in blocks)
            {
                int position = kv.Key;
                if (PropagateGroupId(blocks, position, groupId))
                {
                    seed.Add(position, groupId);
                    groupId++;
                }
            }
        }

        /// <summary>
        /// Groups up all blocks next to each other into a group by assigning them the same ID.
        /// The ID is chosen by the given value belonging to the position in the cache.
        /// </summary>
        /// <param name="blocks">Dictionary containing blocks with their positions as key</param>
        /// <param name="seed">A cache that one position for a created block group is removed from if not found</param>
        /// <param name="groupId">Reference to the group ID, which will be larger than the largest ID found when finished</param>
        public static void AssignGroupIdsFromSeed(Dictionary<int, IBlockGroupId> blocks, SerializableDictionary<int, int> seed, ref int groupId)
        {
            foreach (KeyValuePair<int, int> kv in seed)
            {
                int currentPos = kv.Key;
                int cacheId = kv.Value;
                if (groupId <= cacheId)
                {
                    groupId = cacheId + 1;
                }
                PropagateGroupId(blocks, currentPos, cacheId);
            }
        }
    }
}
