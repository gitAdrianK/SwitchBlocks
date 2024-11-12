namespace SwitchBlocks
{
    public static class ModStrings
    {
        public const string MODNAME = "Zebra.SwitchBlocksMod";
        public const string HARMONY = MODNAME + ".Harmony";

        // File related
        public const string FOLDER = "switchBlocksMod";
        public const string LEVERS = "levers";
        public const string PLATFORMS = "platforms";
        public const string TEXTURES = "textures";

        // Xml tags
        public const string XML_LEVERS = "Levers";
        public const string XML_PLATFORMS = "Platforms";
        public const string POSITION = "Position";
        public const string TEXTURE = "Texture";
        public const string SIZE = "Size";
        public const string START_STATE = "StartState";
        public const string ANIMATION = "Animation";
        public const string ANIMATION_OUT = "AnimationOut";

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

        // Blocks, Animation or RGB/XY strings are not here because they are pretty much used in that one class,
        // typos in there are easier to spot
    }
}
