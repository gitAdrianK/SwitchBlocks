namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using JetBrains.Annotations;

    public class SettingsCountdown
    {
        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public SettingsCountdown([CanBeNull] XElement element)
        {
            this.Duration = ParseSettings.ParseDuration(element?.Element("Duration"), 3.0f);
            this.Multiplier = ParseSettings.ParseMultiplier(element?.Element("Multiplier"));
            this.LeverDirections = ParseSettings.ParseSideDisable(element?.Element("LeverSideDisable"));
            this.ForceSwitch = element?.Element("ForceStateSwitch") != null;
            this.SingleUseReset = element?.Element("SingleUseReset") != null;

            var warnElement = element?.Element("Warn");
            this.WarnCount = ParseSettings.ParseCount(warnElement?.Element("Count"), 2);
            this.WarnDuration = ParseSettings.ParseDuration(warnElement?.Element("Duration"), 1.0f);
        }

        /// <summary>How long the blocks stay in their state before switching.</summary>
        public int Duration { get; private set; }

        /// <summary>Multiplier of the deltaTime used in the animation of the countdown block type.</summary>
        public float Multiplier { get; private set; }

        /// <summary>Directions the basic lever can be activated from.</summary>
        public BitVector32 LeverDirections { get; private set; }

        /// <summary>If the countdown state switch is supposed to be forced, ignoring the safe switch.</summary>
        public bool ForceSwitch { get; private set; }

        ///<summary>If the single use countdown blocks reset when the timer ends.</summary>
        public bool SingleUseReset { get; private set; }

        /// <summary>Amount of times the countdown warn sound is supposed to be played.</summary>
        public int WarnCount { get; private set; }

        /// <summary>Duration between countdown warn sounds.</summary>
        public int WarnDuration { get; private set; }
    }
}
