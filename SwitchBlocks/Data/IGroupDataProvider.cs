namespace SwitchBlocks.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface giving access to a state and progress by id of a group.
    /// </summary>
    public interface IGroupDataProvider
    {
        /// <summary>
        /// Mappings of <see cref="BlockGroup"/>s to their id.
        /// </summary>
        Dictionary<int, BlockGroup> Groups { get; set; }


        /// <summary>
        /// Ids considered active, that is, the progress is not in an
        /// endstate and needs to be updated.
        /// </summary>
        HashSet<int> Active { get; set; }

        /// <summary>
        /// Ids considered finished, that is, the progress is in the opposite
        /// endstate to the state created with.
        /// </summary>
        HashSet<int> Finished { get; set; }
    }
}
