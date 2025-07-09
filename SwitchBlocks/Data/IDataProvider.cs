namespace SwitchBlocks.Data
{
    /// <summary>
    ///     Interface giving access to a state and progress.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>The current state.</summary>
        bool State { get; }

        /// <summary>The current progress.</summary>
        float Progress { get; set; }
    }
}
