namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Linq;

    // This is the third time I implement basically the same thing. I REALLY need to come up with a solution.
    // Well the solutions I thought of are not possible in net4.5

    /// <summary>Interface providing the Duration.</summary>
    public interface IBlockDuration
    {
        // ReSharper disable ArrangeTypeMemberModifiers
        /// <summary>Duration</summary>
        int Duration { get; set; }
    }

    /// <summary>
    ///     Methods related to the block group duration.
    /// </summary>
    public static class BlockDuration
    {
        /// <summary>Value representing that a block was not assigned a duration.</summary>
        public const int NotAssigned = 0;

        /// <summary>Default duration to assign blocks to when not assigned from seed.</summary>
        private const float DefaultDuration = 3;

        /// <summary>Default duration in ticks.</summary>
        private const int DefaultTicks = (int)((DefaultDuration / ModConstants.DeltaTime) + 0.5f);

        /// <summary>
        ///     To move 1 up or down is to change the integer by 1.
        ///     - is up, + is down.
        /// </summary>
        public const int Vertical = BlockGroupId.Vertical;

        /// <summary>
        ///     To move 1 left or right is to change the integer by 100.
        ///     - is left, + is right.
        /// </summary>
        public const int Horizontal = BlockGroupId.Horizontal;

        /// <summary>
        ///     To move 1 screen is to change the integer by 10044.
        ///     - is previous, + is next.
        /// </summary>
        public const int Screen = BlockGroupId.Screen;

        /// <summary>
        ///     Assigns the duration to the block and looks for neighbors of this block that are contained
        ///     in the blocks dictionary and propagates the duration to those neighbor blocks.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign the duration to and propagate from.</param>
        /// <param name="startPosition">Position from which the propagation is supposed to start.</param>
        /// <param name="duration">Duration that is to be assigned to all blocks of the group.</param>
        /// <returns><c>true</c> if at least one block was assigned a duration, <c>false</c> otherwise.</returns>
        private static bool PropagateDuration(
            Dictionary<int, IBlockDuration> blocks,
            int startPosition,
            int duration)
        {
            if (duration == NotAssigned
                || !blocks.TryGetValue(startPosition, out var value)
                || value.Duration != NotAssigned)
            {
                return false;
            }

            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count != 0)
            {
                var currentPos = toVisit.Dequeue();
                blocks[currentPos].Duration = duration;

                // Left
                var left = currentPos - Horizontal;
                if (blocks.TryGetValue(left, out value) && value.Duration == NotAssigned)
                {
                    toVisit.Enqueue(left);
                }

                // Right
                var right = currentPos + Horizontal;
                if (blocks.TryGetValue(right, out value) && value.Duration == NotAssigned)
                {
                    toVisit.Enqueue(right);
                }

                // Up
                var up = currentPos % 100 == 0 ? currentPos + Screen : currentPos - Vertical;
                if (blocks.TryGetValue(up, out value) && value.Duration == NotAssigned)
                {
                    toVisit.Enqueue(up);
                }

                // Down
                var down = currentPos % 100 == 44 ? currentPos - Screen : currentPos + Vertical;
                if (blocks.TryGetValue(down, out value) && value.Duration == NotAssigned)
                {
                    toVisit.Enqueue(down);
                }
            }

            return true;
        }

        /// <summary>
        ///     Assigns durations to unassigned blocks. Failures to create groups are removed from the "seeds" dictionary.
        /// </summary>
        /// <param name="seeds">Seeds to use for id assignment, failing to assign the seed removes it.</param>
        /// <param name="allBlocks">Blocks to potentially assign the id to and propagate from.</param>
        public static void AssignDurationsFromSeed(
            Dictionary<int, float> seeds,
            params Dictionary<int, IBlockDuration>[] allBlocks)
        {
            var misses = new List<int>();
            foreach (var kv in seeds)
            {
                var currentPos = kv.Key;
                var duration = (int)((kv.Value / ModConstants.DeltaTime) + 0.5f);

                var found = allBlocks.Any(blocks => PropagateDuration(blocks, currentPos, duration));
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

        /// <summary>
        ///     Assigns durations to unassigned blocks. Successfully created groups are added to the "seeds" dictionary.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign the id to and propagate from.</param>
        /// <param name="seeds">Seeds to add created block groups to.</param>
        public static void AssignOtherDurations(
            Dictionary<int, IBlockDuration> blocks,
            Dictionary<int, float> seeds)
        {
            foreach (var position in blocks.Select(kv => kv.Key))
            {
                if (!PropagateDuration(blocks, position, DefaultTicks))
                {
                    continue;
                }

                seeds[position] = DefaultDuration;
            }
        }
    }
}
