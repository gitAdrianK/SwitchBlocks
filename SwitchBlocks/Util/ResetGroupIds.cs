namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Interface providing the ResetIDs.</summary>
    public interface IResetGroupIds
    {
        /// <summary>ResetIDs</summary>
        int[] ResetIDs { get; set; }
    }

    /// <summary>
    ///     Methods related to the ResetIDs.
    /// </summary>
    public static class ResetGroupIds
    {
        /// <summary>
        ///     To move 1 up or down is to change the integer by 1.
        ///     - is up, + is down.
        /// </summary>
        private const int Vertical = BlockGroupId.Vertical;

        /// <summary>
        ///     To move 1 left or right is to change the integer by 100.
        ///     - is left, + is right.
        /// </summary>
        private const int Horizontal = BlockGroupId.Horizontal;

        /// <summary>
        ///     To move 1 screen is to change the integer by 10044.
        ///     - is previous, + is next.
        /// </summary>
        private const int Screen = BlockGroupId.Screen;

        /// <summary>Value representing that a reset block resets all IDs to default.</summary>
        private static readonly int[] ResetDefault = { 0 };

        /// <summary>
        ///     Assigns the ResetIDs to the block and looks for neighbors of this block that are contained
        ///     in the blocks dictionary and propagates the ResetIDs to those neighbor blocks.
        /// </summary>
        /// <param name="blocks">Dictionary of blocks to potentially assign the IDs to and propagate from.</param>
        /// <param name="startPosition">Position from which the propagation is supposed to start.</param>
        /// <param name="resetIds">IDs that are to be assigned to all blocks of the group.</param>
        /// <returns><c>true</c> if at least one block was assigned reset IDs, <c>false</c> otherwise.</returns>
        private static bool PropagateResetIds(
            Dictionary<int, IResetGroupIds> blocks,
            int startPosition,
            int[] resetIds)
        {
            if (!blocks.TryGetValue(startPosition, out var value) || value.ResetIDs.Length != 0)
            {
                return false;
            }

            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count() != 0)
            {
                var currentPos = toVisit.Dequeue();
                blocks[currentPos].ResetIDs = resetIds;

                // Left
                var left = currentPos - Horizontal;
                if (blocks.TryGetValue(left, out value) && value.ResetIDs.Length == 0)
                {
                    toVisit.Enqueue(left);
                }

                // Right
                var right = currentPos + Horizontal;
                if (blocks.TryGetValue(right, out value) && value.ResetIDs.Length == 0)
                {
                    toVisit.Enqueue(right);
                }

                // Up
                var up = currentPos % 100 == 0 ? currentPos + Screen : currentPos - Vertical;
                if (blocks.TryGetValue(up, out value) && value.ResetIDs.Length == 0)
                {
                    toVisit.Enqueue(up);
                }

                // Down
                var down = currentPos % 100 == 44 ? currentPos - Screen : currentPos + Vertical;
                if (blocks.TryGetValue(down, out value) && value.ResetIDs.Length == 0)
                {
                    toVisit.Enqueue(down);
                }
            }

            return true;
        }

        /// <summary>
        ///     Assigns ResetIDs to unassigned blocks.
        ///     Failures to create groups are removed from the reset's dictionary.
        /// </summary>
        /// <param name="blocks">Blocks to potentially propagate to.</param>
        /// <param name="resets">Resets to use for IDs assignment, failing to assign the seed removes it.</param>
        public static void AssignResetIdsFromSeed(
            Dictionary<int, IResetGroupIds> blocks,
            Dictionary<int, int[]> resets)
        {
            var misses =
                (from kv in resets
                    let currentPos = kv.Key
                    let resetIds = kv.Value
                    where !PropagateResetIds(blocks, currentPos, resetIds)
                    select currentPos).ToList();

            foreach (var miss in misses)
            {
                _ = resets.Remove(miss);
            }
        }

        /// <summary>
        ///     Assigns the ResetIDs to "reset to default" to unassigned blocks.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign to.</param>
        /// <param name="resets">Resets to add unassigned reset blocks positions to.</param>
        public static void AssignOtherResets(
            Dictionary<int, IResetGroupIds> blocks,
            Dictionary<int, int[]> resets)
        {
            foreach (var position in blocks.Select(kv => kv.Key)
                         .Where(position => PropagateResetIds(blocks, position, ResetDefault)))
            {
                resets.Add(position, ResetDefault);
            }
        }
    }
}
