namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IResetGroupIds
    {
        int[] ResetIds { get; set; }
    }

    public static class ResetGroupIds
    {
        private static readonly int[] RESET_ALL = { 0 };

        public const int VERTICAL = BlockGroupId.VERTICAL;
        public const int HORIZONTAL = BlockGroupId.HORIZONTAL;
        public const int SCREEN = BlockGroupId.SCREEN;

        public static bool PropagateResetIds(
            Dictionary<int, IResetGroupIds> blocks,
            int startPosition,
            int[] resetIds)
        {
            if (!blocks.TryGetValue(startPosition, out var value) || value.ResetIds.Any())
            {
                return false;
            }
            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Any())
            {
                var currentPos = toVisit.Dequeue();
                blocks[currentPos].ResetIds = resetIds;

                // Left
                var left = currentPos - HORIZONTAL;
                if (blocks.TryGetValue(left, out value) && !value.ResetIds.Any())
                {
                    toVisit.Enqueue(left);
                }
                // Right
                var right = currentPos + HORIZONTAL;
                if (blocks.TryGetValue(right, out value) && !value.ResetIds.Any())
                {
                    toVisit.Enqueue(right);
                }
                // Up
                var up = currentPos % 100 == 0 ? currentPos + SCREEN : currentPos - VERTICAL;
                if (blocks.TryGetValue(up, out value) && !value.ResetIds.Any())
                {
                    toVisit.Enqueue(up);
                }
                // Down
                var down = currentPos % 100 == 44 ? currentPos - SCREEN : currentPos + VERTICAL;
                if (blocks.TryGetValue(down, out value) && !value.ResetIds.Any())
                {
                    toVisit.Enqueue(down);
                }
            }
            return true;
        }

        public static void AssignResetIdsFromSeed(
            Dictionary<int, IResetGroupIds> blocks,
            Dictionary<int, int[]> resets)
        {
            var misses = new List<int>();
            foreach (var kv in resets)
            {
                var currentPos = kv.Key;
                var resetIds = kv.Value;
                if (!PropagateResetIds(blocks, currentPos, resetIds))
                {
                    misses.Add(currentPos);
                }
            }
            foreach (var miss in misses)
            {
                _ = resets.Remove(miss);
            }
        }

        public static void AssignOtherResets(
            Dictionary<int, IResetGroupIds> blocks,
            Dictionary<int, int[]> resets)
        {
            // 0 is used to indicate that the reset block resets all groups / group ids
            foreach (var kv in blocks)
            {
                var position = kv.Key;
                if (PropagateResetIds(blocks, position, RESET_ALL))
                {
                    resets.Add(position, RESET_ALL);
                }
            }
        }
    }
}
