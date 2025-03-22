namespace SwitchBlocks.Data
{
    /// <summary>
    /// Interface giving access to a state and progress by id of a group.
    /// </summary>
    public interface IGroupDataProvider
    {
        /// <summary>
        /// Get groups state.
        /// </summary>
        /// <param name="id">Id of the group.</param>
        /// <returns>State of the group.</returns>
        bool GetState(int id);
        /// <summary>
        /// Get groups progress.
        /// </summary>
        /// <param name="id">Id of the group.</param>
        /// <returns>Progress of the group.</returns>
        float GetProgress(int id);
    }
}
