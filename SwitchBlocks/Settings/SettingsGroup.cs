namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using JetBrains.Annotations;

    public class SettingsGroup
    {
        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public SettingsGroup([CanBeNull] XElement element)
        {
            this.Duration = ParseSettings.ParseDuration(element?.Element("Duration"), 0);
            this.Multiplier = ParseSettings.ParseMultiplier(element?.Element("Multiplier"));
            this.LeverDirections = ParseSettings.ParseSideDisable(element?.Element("LeverSideDisable"));
            this.PlatformDirections = ParseSettings.ParseSideDisable(element?.Element("PlatformSideDisable"));
        }

        /// <summary>How long the blocks stay in their state before switching.</summary>
        public int Duration { get; private set; }

        /// <summary>Multiplier of the deltaTime used in the animation of the group block type.</summary>
        public float Multiplier { get; private set; }

        /// <summary>Directions the group lever can be activated from.</summary>
        public BitVector32 LeverDirections { get; private set; }

        /// <summary>Directions the group platform can be activated from.</summary>
        public BitVector32 PlatformDirections { get; private set; }
    }
}
