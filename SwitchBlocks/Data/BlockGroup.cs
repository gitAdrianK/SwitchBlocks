namespace SwitchBlocks.Data
{
    using System.Collections.Generic;

    public class BlockGroup : IDataProvider
    {
        public bool State { get; set; }
        public float Progress { get; set; }
        public int ActivatedTick { get; set; }

        public BlockGroup() : this(true)
        {
        }

        public BlockGroup(bool isEnabled)
        {
            this.State = isEnabled;
            this.Progress = isEnabled ? 1.0f : 0.0f;
            this.ActivatedTick = isEnabled ? int.MaxValue : int.MinValue;
        }

        /// <summary>
        /// Ensures that there is group data for all IDs up to the given group ID.
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
