namespace SwitchBlocks
{
    /// <summary>
    /// Collection of string that are used by the mod. Strings that are used exclusively in a single class may not appear here.
    /// </summary>
    public static class ModConsts
    {
        /// <summary>Modname.</summary>
        public const string MODNAME = "Zebra.SwitchBlocksMod";
        /// <summary>Harmony instance name.</summary>
        public const string HARMONY = MODNAME + ".Harmony";

        /// <summary>Root folder name.</summary>
        public const string FOLDER = "switchBlocksMod";
        /// <summary>Texture folder name.</summary>
        public const string TEXTURES = "textures";
        /// <summary>Saves folder name.</summary>
        public const string SAVES = "saves";

        /// <summary>Save prefix.</summary>
        public const string PREFIX_SAVE = "save_";
        /// <summary>Cache prefix.</summary>
        public const string PREFIX_CACHE = "cache_";
        /// <summary>Seeds prefix.</summary>
        public const string PREFIX_SEEDS = "seeds_";
        /// <summary>Resets prefix.</summary>
        public const string PREFIX_RESETS = "resets_";
        /// <summary>All saves file ending.</summary>
        public const string SUFFIX_SAV = ".sav";

        /// <summary>Groups name inside .sav files.</summary>
        public const string SAVE_GROUPS = "_groups";
        /// <summary>Group name inside .sav files.</summary>
        public const string SAVE_GROUP = "_group";
        /// <summary>Seeds name inside .sav files.</summary>
        public const string SAVE_SEEDS = "_seeds";
        /// <summary>Seed name inside .sav files.</summary>
        public const string SAVE_SEED = "_seed";
        /// <summary>Resets name inside .sav files.</summary>
        public const string SAVE_RESETS = "_resets";
        /// <summary>Reset name inside .sav files.</summary>
        public const string SAVE_RESET = "_reset";
        /// <summary>State name inside .sav files.</summary>
        public const string SAVE_STATE = "_state";
        /// <summary>Progress name inside .sav files.</summary>
        public const string SAVE_PROGRESS = "_progress";
        /// <summary>HasSwitched name inside .sav files.</summary>
        public const string SAVE_HAS_SWITCHED = "_hasSwitched";
        /// <summary>HasEntered name inside .sav files.</summary>
        public const string SAVE_HAS_ENTERED = "_hasEntered";
        /// <summary>CanSwitchSafely name inside .sav files.</summary>
        public const string SAVE_CSS = "_canSwitchSafely";
        /// <summary>SwitchOnceSafe name inside .sav files.</summary>
        public const string SAVE_SOS = "_switchOnceSafe";
        /// <summary>WarnCount name inside .sav files.</summary>
        public const string SAVE_WARN_COUNT = "_warnCount";
        /// <summary>ResetTick name inside .sav files.</summary>
        public const string SAVE_RESET_TICK = "_resetTick";
        /// <summary>ActivatedTick name inside .sav files.</summary>
        public const string SAVE_ACTIVATED = "_activatedTick";
        /// <summary>Touched name inside .sav files.</summary>
        public const string SAVE_TOUCHED = "_touched";
        /// <summary>Active name inside .sav files.</summary>
        public const string SAVE_ACTIVE = "_active";
        /// <summary>Finished name inside .sav files.</summary>
        public const string SAVE_FINISHED = "_finished";
        /// <summary>Position name inside .sav files.</summary>
        public const string SAVE_POSITION = "_position";
        /// <summary>Id name inside .sav files.</summary>
        public const string SAVE_ID = "_id";

        /// <summary>Auto folder name.</summary>
        public const string AUTO = "auto";
        /// <summary>Basic folder name.</summary>
        public const string BASIC = "basic";
        /// <summary>Countdown folder name.</summary>
        public const string COUNTDOWN = "countdown";
        /// <summary>Group folder name.</summary>
        public const string GROUP = "group";
        /// <summary>Jump folder name.</summary>
        public const string JUMP = "jump";
        /// <summary>Sand folder name.</summary>
        public const string SAND = "sand";
        /// <summary>Sequence folder name.</summary>
        public const string SEQUENCE = "sequence";

        /// <summary>Behaviour priority run before all other behaviours. Used for BehaviourPre.</summary>
        public const float PRIO_FIRST = 3.0f;
        /// <summary>Behaviour priority run before normal behaviours but after the one run first.</summary>
        public const float PRIO_EARLY = 2.5f;
        /// <summary>Behaviour priority run "normally", this is the priority for most.</summary>
        public const float PRIO_NORMAL = 2.0f;
        /// <summary>Behaviour priority run after normal behaviours but before the one run last.</summary>
        public const float PRIO_LATE = 1.5f;
        /// <summary>Behaviour priority run after all other behaviours. Used for BehaviourPost.</summary>
        public const float PRIO_LAST = 1.0f;
    }
}
