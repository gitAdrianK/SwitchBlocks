namespace SwitchBlocksMod.Data
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
        /// If it has blinked once.
        /// </summary>
        public static bool HasBlinkedOnce { get; set; }

        /// <summary>
        /// If it has blinked twice.
        /// </summary>
        public static bool HasBlinkedTwice { get; set; }
    }
}
