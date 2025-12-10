// ReSharper disable InvertIf

namespace SwitchBlocks.Settings
{
    using System.Xml.Linq;
    using JetBrains.Annotations;

    public class SettingsAuto
    {
        /// <summary>
        ///     Parse the <see cref="XElement" /> to create the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public SettingsAuto([CanBeNull] XElement element)
        {
            this.DurationOn = ParseSettings.ParseDuration(element?.Element("Duration"), 3.0f);
            this.DurationOff = ParseSettings.ParseDuration(element?.Element("DurationOff"), this.DurationOn);
            this.DurationCycle = this.DurationOn + this.DurationOff;
            this.Multiplier = ParseSettings.ParseMultiplier(element?.Element("Multiplier"));
            this.ForceSwitch = element?.Element("ForceStateSwitch") != null;

            var warnElement = element?.Element("Warn");
            this.WarnCount = ParseSettings.ParseCount(warnElement?.Element("Count"), 2);
            this.WarnDuration = ParseSettings.ParseDuration(warnElement?.Element("Duration"), 1.0f);
            this.WarnDisableOn = warnElement?.Element("DisableOn") != null;
            this.WarnDisableOff = warnElement?.Element("DisableOff") != null;
        }

        /// <summary>How long the blocks stay in their state before switching.</summary>
        public int DurationOn { get; }

        /// <summary>How long the blocks stay in their off state before switching.</summary>
        public int DurationOff { get; }

        /// <summary>How long a full on off cycle takes. </summary>
        public int DurationCycle { get; private set; } // DurationOn + DurationOff

        /// <summary>Multiplier of the deltaTime used in the animation of the auto block type.</summary>
        public float Multiplier { get; private set; }

        /// <summary>If the auto state switch is supposed to be forced, ignoring the safe switch.</summary>
        public bool ForceSwitch { get; private set; }

        /// <summary>Amount of times the auto warn sound is supposed to be played.</summary>
        public int WarnCount { get; private set; }

        /// <summary>Duration between auto warn sounds.</summary>
        public int WarnDuration { get; private set; }

        /// <summary>If the warn sound is disabled for the on state.</summary>
        public bool WarnDisableOn { get; private set; }

        /// <summary>If the warn sound is disabled for the off state.</summary>
        public bool WarnDisableOff { get; private set; }
    }
}
