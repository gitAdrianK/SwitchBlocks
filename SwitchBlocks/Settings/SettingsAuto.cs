// ReSharper disable InvertIf

namespace SwitchBlocks.Settings
{
    using System.Xml.Linq;

    public static class SettingsAuto
    {
        /// <summary>How long the blocks stay in their state before switching.</summary>
        public static int DurationOn { get; private set; } = 180;

        /// <summary>How long the blocks stay in their off state before switching.</summary>
        public static int DurationOff { get; private set; } = 180;

        /// <summary>How long a full on off cycle takes. </summary>
        public static int DurationCycle { get; private set; } = 360;

        /// <summary>Multiplier of the deltaTime used in the animation of the auto block type.</summary>
        public static float Multiplier { get; private set; } = 1.0f;

        /// <summary>If the auto state switch is supposed to be forced, ignoring the safe switch.</summary>
        public static bool ForceSwitch { get; private set; }

        /// <summary>Amount of times the auto warn sound is supposed to be played.</summary>
        public static int WarnCount { get; private set; } = 2;

        /// <summary>Duration between auto warn sounds.</summary>
        public static int WarnDuration { get; private set; } = 60;

        /// <summary>If the warn sound is disabled for the on state.</summary>
        public static bool WarnDisableOn { get; private set; }

        /// <summary>If the warn sound is disabled for the off state.</summary>
        public static bool WarnDisableOff { get; private set; }

        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public static void Parse(XElement element)
        {
            DurationOn = ParseSettings.ParseDuration(element.Element("Duration"), 3.0f);
            DurationOff = ParseSettings.ParseDuration(element.Element("DurationOff"), DurationOn);
            DurationCycle = DurationOn + DurationOff;
            Multiplier = ParseSettings.ParseMultiplier(element.Element("Multiplier"));
            ForceSwitch = element.Element("ForceStateSwitch") != null;
            var warnElement = element.Element("Warn");
            if (warnElement != null)
            {
                WarnCount = ParseSettings.ParseCount(warnElement.Element("Count"), 2);
                WarnDuration = ParseSettings.ParseDuration(warnElement.Element("Duration"), 1.0f);
                WarnDisableOn = element.Element("DisableOn") != null;
                WarnDisableOff = element.Element("DisableOff") != null;
            }
        }

        /// <summary>
        ///     Resets all settings to their default values.
        /// </summary>
        public static void Reset()
        {
            DurationOn = 180;
            DurationOff = 180;
            DurationCycle = 360;
            Multiplier = 1.0f;
            ForceSwitch = false;
            WarnCount = 2;
            WarnDuration = 60;
            WarnDisableOn = false;
            WarnDisableOff = false;
        }
    }
}
