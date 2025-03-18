namespace SwitchBlocks
{
    public static class ModStrings
    {
        public const string MODNAME = "Zebra.SwitchBlocksMod";
        public const string HARMONY = MODNAME + ".Harmony";

        // File related
        public const string FOLDER = "switchBlocksMod";
        public const string TEXTURES = "textures";
        public const string SAVES = "saves";

        // Save related (to keep with the old saves we use underscores)
        public const string PREFIX_SAVE = "save_";
        public const string PREFIX_CACHE = "cache_";
        public const string PREFIX_SEEDS = "seeds_";
        public const string PREFIX_RESETS = "resets_";
        public const string SUFFIX_SAV = ".sav";

        public const string SAVE_GROUPS = "_groups";
        public const string SAVE_GROUP = "_group";
        public const string SAVE_SEEDS = "_seeds";
        public const string SAVE_SEED = "_seed";
        public const string SAVE_RESETS = "_resets";
        public const string SAVE_RESET = "_reset";

        public const string SAVE_STATE = "_state";
        public const string SAVE_PROGRESS = "_progress";
        public const string SAVE_HAS_SWITCHED = "_hasSwitched";
        public const string SAVE_HAS_ENTERED = "_hasEntered";
        public const string SAVE_CSS = "_canSwitchSafely";
        public const string SAVE_SOS = "_switchOnceSafe";
        public const string SAVE_WARN_COUNT = "_warnCount";
        public const string SAVE_RESET_TICK = "_resetTick";
        public const string SAVE_ACTIVATED = "_activatedTick";
        public const string SAVE_TOUCHED = "_touched";
        public const string SAVE_ACTIVE = "_active";
        public const string SAVE_FINISHED = "_finished";
        public const string SAVE_POSITION = "_position";
        public const string SAVE_ID = "_id";

        // Xml tags
        public const string POSITION = "Position";
        public const string TEXTURE = "Texture";
        public const string SIZE = "Size";
        public const string START_STATE = "StartState";
        public const string ANIMATION = "Animation";
        public const string ANIMATION_OUT = "AnimationOut";
        public const string SPRITES = "Sprites";
        public const string CELLS = "Cells";
        public const string FRAMES = "Frames";
        public const string FPS = "FPS";
        public const string OFFSET = "RandomOffset";

        // Relevant for sand Xml
        public const string BACKGROUND = "Background";
        public const string SCROLLING = "Scrolling";
        public const string FOREGROUND = "Foreground";

        // Relevant for group Xml
        public const string LINK_POSITION = "Link";

        // Block types
        public const string AUTO = "auto";
        public const string BASIC = "basic";
        public const string COUNTDOWN = "countdown";
        public const string GROUP = "group";
        public const string JUMP = "jump";
        public const string SAND = "sand";
        public const string SEQUENCE = "sequence";
    }
}
