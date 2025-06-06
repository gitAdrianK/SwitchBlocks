namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using SwitchBlocks.Util;

    public static class SettingsSequence
    {
        /// <summary>How long the blocks stay in their state before switching.</summary>
        public static int Duration { get; private set; } = 0;
        /// <summary>Multiplier of the deltaTime used in the animation of the sequence block type.</summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>Directions the sequence lever can be activated from.</summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>Directions the sequence platform can be activated from.</summary>
        public static BitVector32 PlatformDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>If the platform should be disabled when left.</summary>
        public static bool DisableOnLeave { get; private set; } = false;

        /// <summary>
        /// Parse the <see cref="XElement"/> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement"/> settings are to be taken from.</param>
        public static void Parse(XElement element)
        {
            Duration = ParseSettings.ParseDuration(element.Element("Duration"), 0);
            Multiplier = ParseSettings.ParseMultiplier(element.Element("Multiplier"));
            LeverDirections = ParseSettings.ParseSideDisable(element.Element("LeverSideDisable"));
            PlatformDirections = ParseSettings.ParseSideDisable(element.Element("PlatformSideDisable"));
            DisableOnLeave = element.Element("DisableOnLeaving") != null;
        }

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
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
