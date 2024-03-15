namespace SwitchBlocksMod.Data
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
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public static bool HasSwitched { get; set; }
        /// <summary>
        /// Time before flipping state.
        /// </summary>
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
