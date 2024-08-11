namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the countdown block.
    /// </summary>
    static class DataCountdown
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
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public static bool HasSwitched { get; set; }

        /// <summary>
        /// Time before flipping state.
        /// </summary>
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
