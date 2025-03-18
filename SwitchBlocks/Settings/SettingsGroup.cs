namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using static SwitchBlocks.Util.Directions;

    public static class SettingsGroup
    {
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = 0;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the group block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the group lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>
        /// Directions the group platform can be activated from.
        /// </summary>
        public static BitVector32 PlatformDirections { get; private set; } = new BitVector32((int)Direction.All);

        public static void Parse(XElement element)
        {
            Duration = ParseSettings.ParseDuration(element.Element("Duration"), 0);
            Multiplier = ParseSettings.ParseMultiplier(element.Element("Multiplier"));
            LeverDirections = ParseSettings.ParseSideDisable(element.Element("LeverSideDisable"));
            PlatformDirections = ParseSettings.ParseSideDisable(element.Element("PlatformSideDisable"));
        }

        public static void Reset()
        {
            Duration = 0;
            Multiplier = 1.0f;
            LeverDirections = new BitVector32((int)Direction.All);
            PlatformDirections = new BitVector32((int)Direction.All);
        }
    }
}
