namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using SwitchBlocks.Util;

    public static class SettingsCountdown
    {
        /// <summary>How long the blocks stay in their state before switching.</summary>
        public static int Duration { get; private set; } = 180;
        /// <summary>Multiplier of the deltaTime used in the animation of the countdown block type.</summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>Directions the basic lever can be activated from.</summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>If the countdown state switch is supposed to be forced, ignoring the safe switch.</summary>
        public static bool ForceSwitch { get; private set; } = false;
        /// <summary>Amount of times the countdown warn sound is supposed to be played.</summary>
        public static int WarnCount { get; private set; } = 2;
        /// <summary>Duration between countdown warn sounds.</summary>
        public static int WarnDuration { get; private set; } = 60;

        /// <summary>
        /// Parse the <see cref="XElement"/> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement"/> settings are to be taken from.</param>
        public static void Parse(XElement element)
        {
            Duration = ParseSettings.ParseDuration(element.Element("Duration"), 3.0f);
            Multiplier = ParseSettings.ParseMultiplier(element.Element("Multiplier"));
            LeverDirections = ParseSettings.ParseSideDisable(element.Element("LeverSideDisable"));
            ForceSwitch = element.Element("ForceStateSwitch") != null;
            var warnElement = element.Element("Warn");
            if (warnElement != null)
            {
                WarnCount = ParseSettings.ParseCount(warnElement.Element("Count"), 2);
                WarnDuration = ParseSettings.ParseDuration(warnElement.Element("Duration"), 1.0f);
            }
        }

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        public static void Reset()
        {
            Duration = 180;
            Multiplier = 1.0f;
            LeverDirections = new BitVector32((int)Direction.All);
            ForceSwitch = false;
            WarnCount = 2;
            WarnDuration = 60;
        }
    }
}
