// ReSharper disable ArrangeTypeMemberModifiers

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

        /// <summary>The current progress, not limited to the range from 0 to 1. Basically never saved.</summary>
        float ProgressUnclamped { get; set; }

        /// <summary> Tick that relates to the function of the block type </summary>
        int Tick { get; }

        /// <summary> If the switch should happen once it is safe to do so.</summary>
        bool SwitchOnceSafe { get; }
    }
}
