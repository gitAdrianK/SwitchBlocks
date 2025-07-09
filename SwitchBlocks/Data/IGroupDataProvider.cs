namespace SwitchBlocks.Data
{
    using System.Collections.Generic;

    /// <summary>
    ///     Interface giving access to a state and progress by id of a group.
    /// </summary>
    public interface IGroupDataProvider
    {
        /// <summary>
        ///     Mappings of <see cref="BlockGroup" />s to their id.
        /// </summary>
        Dictionary<int, BlockGroup> Groups { get; }


        /// <summary>
        ///     Ids considered active, that is, the progress is not in an
        ///     end-state and needs to be updated.
        /// </summary>
        HashSet<int> Active { get; }

        /// <summary>
        ///     Ids considered finished, that is, the progress is in the opposite
        ///     end-state to the state created with.
        /// </summary>
        HashSet<int> Finished { get; }
    }
}
