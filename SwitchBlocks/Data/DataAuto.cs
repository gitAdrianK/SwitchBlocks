namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the auto block.
    /// </summary>
    static class DataAuto
    {
        /// <summary>
        /// Its current state.
        /// </summary>
        public static bool State { get; set; }

        /// <summary>
        /// Animation progress.
        /// </summary>
        public static float Progress { get; set; }

        /// <summary>
        /// Time before flipping state.
        /// </summary
        public static float RemainingTime { get; set; }

        /// <summary>
        /// If the block can switch safely.
        /// </summary>
        public static bool CanSwitchSafely { get; set; }

        /// <summary>
        /// If the block should switch next opportunity.
        /// </summary>
        public static bool SwitchOnceSafe { get; set; }

        /// <summary>
        /// The amount of times the warning sound has been played.
        /// </summary>
        public static int WarnCount { get; set; }
    }
}
