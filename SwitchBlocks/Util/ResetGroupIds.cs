namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Interface providing the ResetIds.</summary>
    public interface IResetGroupIds
    {
        /// <summary>Reset ids</summary>
        int[] ResetIds { get; set; }
    }

    /// <summary>
    /// Methods related to the reset ids.
    /// </summary>
    public static class ResetGroupIds
    {
        /// <summary>Value representing that a reset block resets all ids.</summary>
        private static readonly int[] RESET_ALL = { 0 };
        /// <summary>
        /// To move 1 up or down is to change the integer by 1.
        /// - is up, + is down.
        /// </summary>
        public const int VERTICAL = BlockGroupId.VERTICAL;
        /// <summary>
        /// To move 1 left or right is to change the integer by 100.
        /// - is left, + is right.
        /// </summary>
        public const int HORIZONTAL = BlockGroupId.HORIZONTAL;
        /// <summary>
        /// To move 1 screen is to change the integer by 10044.
        /// - is previous, + is next.
        /// </summary>
        public const int SCREEN = BlockGroupId.SCREEN;

        /// <summary>
        /// Assigns the reset ids to the block and looks for neighbors of this block that are contained
        /// in the blocks dictionary and propagates the reset ids to those neighbor blocks.
        /// </summary>
        /// <param name="blocks">Dictionary of blocks to potentially assign the ids to and propagate from.</param>
        /// <param name="startPosition">Position from which the propagation is supposed to start.</param>
        /// <param name="resetIds">Ids that are to be assigned to all blocks of the group.</param>
        /// <returns><c>true</c> if at least one block was assigned reset ids, <c>false</c> otherwise.</returns>
        public static bool PropagateResetIds(
            Dictionary<int, IResetGroupIds> blocks,
            int startPosition,
            int[] resetIds)
        {
            if (!blocks.TryGetValue(startPosition, out var value) || value.ResetIds.Count() != 0)
            {
                return false;
            }
            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count() != 0)
            {
                var currentPos = toVisit.Dequeue();
                blocks[currentPos].ResetIds = resetIds;

                // Left
                var left = currentPos - HORIZONTAL;
                if (blocks.TryGetValue(left, out value) && value.ResetIds.Count() == 0)
                {
                    toVisit.Enqueue(left);
                }
                // Right
                var right = currentPos + HORIZONTAL;
                if (blocks.TryGetValue(right, out value) && value.ResetIds.Count() == 0)
                {
                    toVisit.Enqueue(right);
                }
                // Up
                var up = currentPos % 100 == 0 ? currentPos + SCREEN : currentPos - VERTICAL;
                if (blocks.TryGetValue(up, out value) && value.ResetIds.Count() == 0)
                {
                    toVisit.Enqueue(up);
                }
                // Down
                var down = currentPos % 100 == 44 ? currentPos - SCREEN : currentPos + VERTICAL;
                if (blocks.TryGetValue(down, out value) && value.ResetIds.Count() == 0)
                {
                    toVisit.Enqueue(down);
                }
            }
            return true;
        }

        /// <summary>
        /// Assigns reset ids to unassigned blocks.
        /// Failures to create groups are removed from the resets dictionary.
        /// </summary>
        /// <param name="blocks">Blocks to potentially propagate to.</param>
        /// <param name="resets">Resets to use for ids assignment, failing to assign the seed removes it.</param>
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

        /// <summary>
        /// Assigns the reset ids to "reset all" to unassigned blocks.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign to.</param>
        /// <param name="resets">Resets to add unassigned reset blocks positions to.</param>
        public static void AssignOtherResets(
            Dictionary<int, IResetGroupIds> blocks,
            Dictionary<int, int[]> resets)
        {
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
