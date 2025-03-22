namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Interface providing the GroupId.</summary>
    public interface IBlockGroupId
    {
        /// <summary>GroupId</summary>
        int GroupId { get; set; }
    }

    /// <summary>
    /// Methods related to the block group id.
    /// </summary>
    public static class BlockGroupId
    {
        // Position stored in a single integer.
        // X and Y can never be a three digit number.
        // Screen can never be a four digit number.
        // As such the integers form is 00...00SSSXXYY.

        // Top left is (0, 0)
        // Bottom right is (59, 44)

        /// <summary>Value representing that a block was not assigned an id.</summary>
        private const int NOT_ASSIGNED = 0;
        /// <summary>
        /// To move 1 up or down is to change the integer by 1.
        /// - is up, + is down.
        /// </summary>
        public const int VERTICAL = 1;
        /// <summary>
        /// To move 1 left or right is to change the integer by 100.
        /// - is left, + is right.
        /// </summary>
        public const int HORIZONTAL = 100;
        /// <summary>
        /// To move 1 screen is to change the integer by 10044.
        /// - is previous, + is next.
        /// </summary>
        public const int SCREEN = 10044;

        /// <summary>
        /// Assigns the group id to the block and looks for neighbors of this block that are contained
        /// in the blocks dictionary and propagates the group id to those neighbor blocks.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign the id to and propagate from.</param>
        /// <param name="startPosition">Position from which the propagation is supposed to start.</param>
        /// <param name="groupId">Id that is to be assigned to all blocks of the group.</param>
        /// <returns><c>true</c> if at least one block was assigned an id, <c>false</c> otherwise.</returns>
        public static bool PropagateGroupId(
            Dictionary<int, IBlockGroupId> blocks,
            int startPosition,
            int groupId)
        {
            if (!blocks.TryGetValue(startPosition, out var value) || value.GroupId != NOT_ASSIGNED)
            {
                return false;
            }
            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count() != 0)
            {
                var currentPos = toVisit.Dequeue();
                blocks[currentPos].GroupId = groupId;

                // Left
                var left = currentPos - HORIZONTAL;
                if (blocks.TryGetValue(left, out value) && value.GroupId == NOT_ASSIGNED)
                {
                    toVisit.Enqueue(left);
                }
                // Right
                var right = currentPos + HORIZONTAL;
                if (blocks.TryGetValue(right, out value) && value.GroupId == NOT_ASSIGNED)
                {
                    toVisit.Enqueue(right);
                }
                // Up
                var up = currentPos % 100 == 0 ? currentPos + SCREEN : currentPos - VERTICAL;
                if (blocks.TryGetValue(up, out value) && value.GroupId == NOT_ASSIGNED)
                {
                    toVisit.Enqueue(up);
                }
                // Down
                var down = currentPos % 100 == 44 ? currentPos - SCREEN : currentPos + VERTICAL;
                if (blocks.TryGetValue(down, out value) && value.GroupId == NOT_ASSIGNED)
                {
                    toVisit.Enqueue(down);
                }
            }
            return true;
        }

        /// <summary>
        /// Assigns group ids to unassigned blocks counting up the id for every created
        /// block group. Successfully created groups are added to the seeds dictionary.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign the id to and propagate from.</param>
        /// <param name="seeds">Seeds to add created block groups to.</param>
        /// <param name="groupId">Id assigned to the groups, counted up for every group created.</param>
        public static void AssignGroupIdsConsecutively(
            Dictionary<int, IBlockGroupId> blocks,
            Dictionary<int, int> seeds,
            ref int groupId)
        {
            foreach (var kv in blocks)
            {
                var position = kv.Key;
                if (PropagateGroupId(blocks, position, groupId))
                {
                    seeds.Add(position, groupId);
                    groupId++;
                }
            }
        }

        /// <summary>
        /// Assigns group ids to unassigned blocks setting the id to be larger than every created
        /// block group. Failures to create groups are removed from the seeds dictionary.
        /// </summary>
        /// <param name="seeds">Seeds to use for id assignment, failing to assign the seed removes it.</param>
        /// <param name="groupId">Id set to be larger than every seed Id.</param>
        /// <param name="allBlocks">Blocks to potentially assign the id to and propagate from.</param>
        public static void AssignGroupIdsFromSeed(
            Dictionary<int, int> seeds,
            ref int groupId,
            params Dictionary<int, IBlockGroupId>[] allBlocks)
        {
            var misses = new List<int>();
            foreach (var kv in seeds)
            {
                var currentPos = kv.Key;
                var currentId = kv.Value;
                if (groupId <= currentId)
                {
                    groupId = currentId + 1;
                }
                var found = false;
                foreach (var blocks in allBlocks)
                {
                    if (PropagateGroupId(blocks, currentPos, currentId))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    misses.Add(currentPos);
                }
            }
            foreach (var miss in misses)
            {
                _ = seeds.Remove(miss);
            }
        }
    }
}
