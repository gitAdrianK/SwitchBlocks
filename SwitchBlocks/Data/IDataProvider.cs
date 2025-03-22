namespace SwitchBlocks.Data
{
    /// <summary>
    /// Interface giving access to a state and progress.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>State.</summary>
        bool State { get; set; }
        /// <summary>Progress.</summary>
        float Progress { get; set; }
    }
}
