namespace SwitchBlocks
{
    public static class ModConsts
    {
        // Folder that as the root for the other mods folder structure
        public const string FOLDER = "switchBlocksMod";

        // Xml root elements
        public const string XML_LEVERS = "Levers";
        public const string XML_PLATFORMS = "Platforms";

        // Block types
        public const string AUTO = "auto";
        public const string BASIC = "basic";
        public const string COUNTDOWN = "countdown";
        public const string GROUP = "group";
        public const string JUMP = "jump";
        public const string SAND = "sand";
        public const string SEQUENCE = "sequence";

        //Xml element names
        public const string DURATION = "Duration";
        public const string DURATION_OFF = "DurationOff";
        public const string MULTIPLIER = "Multiplier";
        public const string FORCE_STATE_SWITCH = "ForceStateSwitch";
        public const string LEVER_SIDE_DISABLE = "LeverSideDisable";
        public const string PLATFORM_SIDE_DISABLE = "PlatformSideDisable";
        public const string WARN = "Warn";
        public const string COUNT = "Count";
        public const string DISABLE_ON = "DisableOn";
        public const string DISABLE_OFF = "DisableOff";

        // Settings defaults
        public const int DEFAULT_DURATION = 180;
        public const int DEFAULT_NO_DURATION = 0;
        public const float DEFAULT_MULTIPLIER = 1.0f;
        public const bool DEFAULT_FORCE = false;
        public const int DEFAULT_WARN_COUNT = 2;
        public const int DEFAULT_WARN_DURATION = 60;
        public const bool DEFAULT_WARN_DISABLE = false;

        // Blocks, Animation or RGB/XY strings are not here because they are pretty much used in that one class,
        // typos in there are easier to spot
    }
}
