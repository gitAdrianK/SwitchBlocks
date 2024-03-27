namespace SwitchBlocksMod.Data
{
    /// <summary>
    /// Contains data relevant for the basic block.
    /// </summary>
    static class DataBasic
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
    }
}
