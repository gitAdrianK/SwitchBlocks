namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Interface providing multiple ids.</summary>
    public interface IMultipleGroupIds
    {
        // ReSharper disable ArrangeTypeMemberModifiers
        /// <summary>IDs</summary>
        int[] Ids { get; set; }
    }

    /// <summary>
    ///     Methods related to blocks providing multiple IDs.
    /// </summary>
    public static class MultipleGroupIds
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

        /// <summary>Value representing the default value for a block with multiple IDs.</summary>
        public static readonly int[] DefaultMultipleIds = { 0 };

        /// <summary>
        ///     Assigns the IDs to the block and looks for neighbors of this block that are contained
        ///     in the blocks dictionary and propagates the IDs to those neighbor blocks.
        /// </summary>
        /// <param name="blocks">Dictionary of blocks to potentially assign the IDs to and propagate from.</param>
        /// <param name="startPosition">Position from which the propagation is supposed to start.</param>
        /// <param name="resetIds">IDs that are to be assigned to all blocks of the group.</param>
        /// <returns><c>true</c> if at least one block was assigned IDs, <c>false</c> otherwise.</returns>
        private static bool PropagateMultipleIds(
            Dictionary<int, IMultipleGroupIds> blocks,
            int startPosition,
            int[] resetIds)
        {
            if (!blocks.TryGetValue(startPosition, out var value) || value.Ids.Length != 0)
            {
                return false;
            }

            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count != 0)
            {
                var currentPos = toVisit.Dequeue();
                blocks[currentPos].Ids = resetIds;

                // Left
                var left = currentPos - Horizontal;
                if (blocks.TryGetValue(left, out value) && value.Ids.Length == 0)
                {
                    toVisit.Enqueue(left);
                }

                // Right
                var right = currentPos + Horizontal;
                if (blocks.TryGetValue(right, out value) && value.Ids.Length == 0)
                {
                    toVisit.Enqueue(right);
                }

                // Up
                var up = currentPos % 100 == 0 ? currentPos + Screen : currentPos - Vertical;
                if (blocks.TryGetValue(up, out value) && value.Ids.Length == 0)
                {
                    toVisit.Enqueue(up);
                }

                // Down
                var down = currentPos % 100 == 44 ? currentPos - Screen : currentPos + Vertical;
                if (blocks.TryGetValue(down, out value) && value.Ids.Length == 0)
                {
                    toVisit.Enqueue(down);
                }
            }

            return true;
        }

        /// <summary>
        ///     Assigns multiple IDs to unassigned blocks.
        ///     Failures to create groups are removed from the seed's dictionary.
        /// </summary>
        /// <param name="blocks">Blocks to potentially propagate to.</param>
        /// <param name="seeds">Seeds to use for IDs assignment, failing to assign the seed removes it.</param>
        public static void AssignMultipleIdsFromSeed(
            Dictionary<int, IMultipleGroupIds> blocks,
            Dictionary<int, int[]> seeds)
        {
            var misses =
                (from kv in seeds
                    let currentPos = kv.Key
                    let resetIds = kv.Value
                    where !PropagateMultipleIds(blocks, currentPos, resetIds)
                    select currentPos).ToList();

            foreach (var miss in misses)
            {
                _ = seeds.Remove(miss);
            }
        }

        /// <summary>
        ///     Assigns multiple IDs to "default" to unassigned blocks.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign to.</param>
        /// <param name="seeds">Seeds to add unassigned reset blocks positions to.</param>
        public static void AssignOtherMultipleIds(
            Dictionary<int, IMultipleGroupIds> blocks,
            Dictionary<int, int[]> seeds)
        {
            foreach (var position in blocks.Select(kv => kv.Key)
                         .Where(position => PropagateMultipleIds(blocks, position, DefaultMultipleIds)))
            {
                seeds.Add(position, DefaultMultipleIds);
            }
        }
    }
}
