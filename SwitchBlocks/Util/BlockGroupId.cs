namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IBlockGroupId
    {
        int GroupId { get; set; }
    }

    public static class BlockGroupId
    {
        // Position stored in a single integer.
        // X and Y can never be a three digit number.
        // Screen can never be a four digit number.
        // As such the integers form is 00...00SSSXXYY.

        // Top left is (0, 0)
        // Bottom right is (59, 44)

        // To move 1 up or down is to change the integer by 1
        // - is up, + is down
        public const int VERTICAL = 1;
        // To move 1 left or right is to change the integer by 100
        // - is left, + is right
        public const int HORIZONTAL = 100;
        // To move 1 screen is to change the integer by 10044
        // - is previous, + is next
        public const int SCREEN = 10044;

        /// <summary>
        /// Assigns the group ID to the block and looks for neighbors of this block that are contained
        /// in the blocks dictionary and propagates the group ID to those neighbor blocks.
        /// </summary>
        /// <param name="startPosition">The position from which the propagation is supposed to start</param>
        /// <param name="groupId">The ID that is to be assigned to all blocks of the group</param>
        public static bool PropagateGroupId(
            Dictionary<int, IBlockGroupId> blocks,
            int startPosition,
            int groupId)
        {
            if (!blocks.TryGetValue(startPosition, out var value) || value.GroupId != 0)
            {
                return false;
            }
            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Any())
            {
                var currentPos = toVisit.Dequeue();
                blocks[currentPos].GroupId = groupId;

                // Left
                var left = currentPos - HORIZONTAL;
                if (blocks.TryGetValue(left, out value) && value.GroupId == 0)
                {
                    toVisit.Enqueue(left);
                }
                // Right
                var right = currentPos + HORIZONTAL;
                if (blocks.TryGetValue(right, out value) && value.GroupId == 0)
                {
                    toVisit.Enqueue(right);
                }
                // Up
                var up = currentPos % 100 == 0 ? currentPos + SCREEN : currentPos - VERTICAL;
                if (blocks.TryGetValue(up, out value) && value.GroupId == 0)
                {
                    toVisit.Enqueue(up);
                }
                // Down
                var down = currentPos % 100 == 44 ? currentPos - SCREEN : currentPos + VERTICAL;
                if (blocks.TryGetValue(down, out value) && value.GroupId == 0)
                {
                    toVisit.Enqueue(down);
                }
            }
            return true;
        }

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
