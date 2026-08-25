namespace SwitchBlocks.Grouping
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     A class containing all grouping blocks related methods.
    /// </summary>
    public static class Grouping
    {
        // Position stored in a single integer.
        // X and Y can never be a three-digit number.
        // Screen can never be a four-digit number.
        // As such the integers form is 00...00SSSXXYY.

        // Top left is (0, 0)
        // Bottom right is (59, 44)

        /// <summary>
        ///     To move 1 up or down is to change the integer by 1.
        ///     - is up, + is down.
        /// </summary>
        public const int Vertical = 1;

        /// <summary>
        ///     To move 1 left or right is to change the integer by 100.
        ///     - is left, + is right.
        /// </summary>
        public const int Horizontal = 100;

        /// <summary>
        ///     To move 1 screen is to change the integer by 10044.
        ///     - is previous, + is next.
        /// </summary>
        public const int Screen = 10044;

        /// <summary>
        ///     Assigns the group id to the block and looks for neighbors of this block that are contained
        ///     in the blocks dictionary and propagates the group id to those neighbor blocks.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign the id to and propagate from.</param>
        /// <param name="startPosition">Position from which the propagation is supposed to start.</param>
        /// <param name="value">Value that is to be assigned to all blocks of the group.</param>
        /// <returns><c>true</c> if at least one block was assigned an id, <c>false</c> otherwise.</returns>
        private static bool PropagateValue<TValue, TGroupable>(
            Dictionary<int, TGroupable> blocks,
            int startPosition,
            TValue value)
            where TGroupable : class, IGroupable<TValue>
        {
            if (!blocks.TryGetValue(startPosition, out var startBlock) || startBlock.IsSet)
            {
                return false;
            }

            var toVisit = new Queue<int>();
            toVisit.Enqueue(startPosition);
            while (toVisit.Count != 0)
            {
                var currentPos = toVisit.Dequeue();
                if (!blocks.TryGetValue(currentPos, out var block) || block.IsSet)
                {
                    continue;
                }

                blocks[currentPos].Value = value;

                toVisit.Enqueue(currentPos - Horizontal);
                toVisit.Enqueue(currentPos + Horizontal);
                toVisit.Enqueue(currentPos % 100 == 0 ? currentPos + Screen : currentPos - Vertical);
                toVisit.Enqueue(currentPos % 100 == 44 ? currentPos - Screen : currentPos + Vertical);
            }

            return true;
        }

        /// <summary>
        ///     Assigns a value to unassigned blocks. Failures to create groups are removed from the "seeds" dictionary.
        /// </summary>
        /// <param name="seeds">Seeds to use for value assignment, failing to assign the seed removes it.</param>
        /// <param name="allBlocks">Blocks to potentially assign the value to and propagate from.</param>
        public static void AssignFromSeed<TValue, TGroupable>(
            Dictionary<int, TValue> seeds,
            params Dictionary<int, TGroupable>[] allBlocks)
            where TGroupable : class, IGroupable<TValue>
        {
            var misses = new List<int>();
            foreach (var kv in seeds)
            {
                var currentPos = kv.Key;

                var found = allBlocks.Any(blocks => PropagateValue(blocks, currentPos, kv.Value));
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
        ///     Assigns group IDs to unassigned blocks setting the id to be larger than every created
        ///     block group. Failures to create groups are removed from the "seeds" dictionary.
        /// </summary>
        /// <param name="seeds">Seeds to use for id assignment, failing to assign the seed removes it.</param>
        /// <param name="groupId">ID set to be larger than every seed ID.</param>
        /// <param name="allBlocks">Blocks to potentially assign the id to and propagate from.</param>
        public static void AssignIdsFromSeed(
            IdSeeds seeds,
            ref int groupId,
            params Dictionary<int, IGroupId>[] allBlocks)
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

                var found = allBlocks.Any(blocks => PropagateValue(blocks, currentPos, currentId));

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
        ///     Assigns group IDs to unassigned blocks counting up the id for every created
        ///     block group. Successfully created groups are added to the "seeds" dictionary.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign the id to and propagate from.</param>
        /// <param name="seeds">Seeds to add created block groups to.</param>
        /// <param name="groupId">ID assigned to the groups, counted up for every group created.</param>
        public static void AssignIdsConsecutively(
            Dictionary<int, IGroupId> blocks,
            IdSeeds seeds,
            ref int groupId)
        {
            foreach (var position in blocks.Select(kv => kv.Key))
            {
                if (!PropagateValue(blocks, position, groupId))
                {
                    continue;
                }

                seeds[position] = groupId;
                groupId++;
            }
        }

        /// <summary>
        ///     Assigns values to unassigned blocks. Successfully created groups are added to the "seeds" dictionary.
        /// </summary>
        /// <param name="blocks">Blocks to potentially assign the value to and propagate from.</param>
        /// <param name="seeds">Seeds to add created block groups to.</param>
        public static void AssignDefaultToUnassigned<TValue, TGroupable>(
            Dictionary<int, TGroupable> blocks,
            Dictionary<int, TValue> seeds)
            where TGroupable : class, IGroupable<TValue>
        {
            foreach (var keyValuePair in blocks)
            {
                var position = keyValuePair.Key;
                var block = keyValuePair.Value;

                if (PropagateValue(blocks, position, block.DefaultValue))
                {
                    seeds[position] = block.Value;
                }
            }
        }
    }
}
