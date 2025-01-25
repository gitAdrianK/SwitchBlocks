namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml;
    using SwitchBlocks.Util;
    using static SwitchBlocks.Util.Directions;

    public static class SettingsSequence
    {
        /// <summary>
        /// Whether the sequence block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = 0;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the sequence block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the sequence lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>
        /// Directions the sequence platform can be activated from.
        /// </summary>
        public static BitVector32 PlatformDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>
        /// If the platform should be disabled when left.
        /// </summary>
        public static bool DisableOnLeave { get; private set; } = false;

        public static void Parse(XmlNode block)
        {
            var childrenGroup = block.ChildNodes;
            var dictionaryGroup = Xml.MapNames(childrenGroup);
            Duration = ParseSettings.ParseDuration(dictionaryGroup, block, 0);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryGroup, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryGroup, block);
            PlatformDirections = ParseSettings.ParsePlatformSideDisable(dictionaryGroup, block);
            DisableOnLeave = ParseSettings.ParseDisableOnLeaving(dictionaryGroup);
        }

        public static void Reset()
        {
            Duration = 0;
            Multiplier = 1.0f;
            LeverDirections = new BitVector32((int)Direction.All);
            PlatformDirections = new BitVector32((int)Direction.All);
            DisableOnLeave = false;
        }
    }
}
