namespace SwitchBlocks
{
    using Behaviours;
    using JumpKing.API;

    /// <summary>
    ///     Constants that are used by the mod. Constants that are used exclusively in a single class may not appear here.
    /// </summary>
    public static class ModConstants
    {
        /// <summary>Modname.</summary>
        public const string Modname = "Zebra.SwitchBlocks";

        /// <summary><see cref="HarmonyLib.Harmony" /> instance name.</summary>
        public const string Harmony = Modname + ".Harmony";

        /// <summary>Root folder name.</summary>
        public const string Folder = "switchBlocksMod";

        /// <summary>Texture folder name.</summary>
        public const string Textures = "textures";

        /// <summary>Saves folder name.</summary>
        public const string Saves = "saves";

        /// <summary>Save prefix.</summary>
        public const string PrefixSave = "save_";

        /// <summary>Cache prefix.</summary>
        public const string PrefixCache = "cache_";

        /// <summary>Seeds prefix.</summary>
        public const string PrefixSeeds = "seeds_";

        /// <summary>Resets prefix.</summary>
        public const string PrefixResets = "resets_";

        /// <summary>All saves file ending.</summary>
        public const string SuffixSav = ".sav";

        /// <summary>Groups name inside .sav files.</summary>
        public const string SaveGroups = "_groups";

        /// <summary>Group name inside .sav files.</summary>
        public const string SaveGroup = "_group";

        /// <summary>Seeds name inside .sav files.</summary>
        public const string SaveSeeds = "_seeds";

        /// <summary>Seed name inside .sav files.</summary>
        public const string SaveSeed = "_seed";

        /// <summary>Resets name inside .sav files.</summary>
        public const string SaveResets = "_resets";

        /// <summary>Reset name inside .sav files.</summary>
        public const string SaveReset = "_reset";

        /// <summary>State name inside .sav files.</summary>
        public const string SaveState = "_state";

        /// <summary>Progress name inside .sav files.</summary>
        public const string SaveProgress = "_progress";

        /// <summary>HasSwitched name inside .sav files.</summary>
        public const string SaveHasSwitched = "_hasSwitched";

        /// <summary>HasEntered name inside .sav files.</summary>
        public const string SaveHasEntered = "_hasEntered";

        /// <summary>CanSwitchSafely name inside .sav files.</summary>
        public const string SaveCss = "_canSwitchSafely";

        /// <summary>SwitchOnceSafe name inside .sav files.</summary>
        public const string SaveSos = "_switchOnceSafe";

        /// <summary>WarnCount name inside .sav files.</summary>
        public const string SaveWarnCount = "_warnCount";

        /// <summary>ResetTick name inside .sav files.</summary>
        public const string SaveResetTick = "_resetTick";

        /// <summary>ActivatedTick name inside .sav files.</summary>
        public const string SaveActivated = "_activatedTick";

        /// <summary>Touched name inside .sav files.</summary>
        public const string SaveTouched = "_touched";

        /// <summary>Active name inside .sav files.</summary>
        public const string SaveActive = "_active";

        /// <summary>Finished name inside .sav files.</summary>
        public const string SaveFinished = "_finished";

        /// <summary>Position name inside .sav files.</summary>
        public const string SavePosition = "_position";

        /// <summary>Id name inside .sav files.</summary>
        public const string SaveId = "_id";

        /// <summary>Auto folder name.</summary>
        public const string Auto = "auto";

        /// <summary>Basic folder name.</summary>
        public const string Basic = "basic";

        /// <summary>Countdown folder name.</summary>
        public const string Countdown = "countdown";

        /// <summary>Group folder name.</summary>
        public const string Group = "group";

        /// <summary>Jump folder name.</summary>
        public const string Jump = "jump";

        /// <summary>Sand folder name.</summary>
        public const string Sand = "sand";

        /// <summary>Sequence folder name.</summary>
        public const string Sequence = "sequence";

        /// <summary><see cref="IBlockBehaviour" /> priority run before all other behaviours. Used for <see cref="BehaviourPre" />.</summary>
        public const float PrioFirst = 3.0f;

        // /// <summary><see cref="IBlockBehaviour" /> priority run before normal behaviours but after the one run first.</summary>
        // public const float PrioEarly = 2.5f;

        /// <summary><see cref="IBlockBehaviour" /> priority run "normally", this is the priority for most.</summary>
        public const float PrioNormal = 2.01f;

        /// <summary><see cref="IBlockBehaviour" /> priority run after normal behaviours but before the one run last.</summary>
        public const float PrioLate = 1.5f;

        /// <summary><see cref="IBlockBehaviour" /> priority run after all other behaviours. Used for <see cref="BehaviourPost" />.</summary>
        public const float PrioLast = 1.0f;

        /// <summary>Used to convert time seconds to ticks.</summary>
        public const float DeltaTime = 0.01666667f;
    }
}
