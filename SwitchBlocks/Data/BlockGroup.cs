namespace SwitchBlocks.Data
{
    using System.Collections.Generic;

    /// <summary>
    ///     Data for groups of blocks created by group and sequence block types.
    /// </summary>
    public class BlockGroup : IDataProvider
    {
        /// <summary>
        ///     Ctor. Creates a BlockGroup set to be enabled.
        /// </summary>
        public BlockGroup() : this(true)
        {
        }

        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="isEnabled">If this is enabled by default.</param>
        private BlockGroup(bool isEnabled)
        {
            this.State = isEnabled;
            this.Progress = isEnabled ? 1.0f : 0.0f;
            this.ActivatedTick = isEnabled ? int.MaxValue : 0;
        }

        /// <summary>The current tick this block group has been activated.</summary>
        public int ActivatedTick { get; set; }

        /// <inheritdoc />
        public bool State { get; set; }

        /// <inheritdoc />
        public float Progress { get; set; }

        /// <inheritdoc />
        public int Tick => this.ActivatedTick;

        /// <inheritdoc />
        public bool SwitchOnceSafe => true;

        /// <summary>
        ///     Ensures that there is group data for all IDs up to the given group ID.
        /// </summary>
        /// <param name="groupId">The group ID that data is to be created up to for (excluding)</param>
        /// <param name="groups">The dictionary containing groups that is to be added to</param>
        /// <param name="startState">The start state of the added platforms</param>
        public static void CreateGroupData(int groupId, Dictionary<int, BlockGroup> groups, bool startState)
        {
            for (var i = 1; i < groupId; i++)
            {
                if (!groups.ContainsKey(i))
                {
                    groups.Add(i, new BlockGroup(startState));
                }
            }
        }
    }
}
