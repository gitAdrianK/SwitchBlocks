namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the sand block.
    /// </summary>
    static class DataSand
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
        /// Whether the player is currently inside the block.
        /// </summary>
        public static bool HasEntered { get; set; }
    }
}
