namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using JetBrains.Annotations;

    public class SettingsSand
    {
        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public SettingsSand([CanBeNull] XElement element)
        {
            this.IsV2 = element?.Element("v2") != null;
            this.Multiplier = ParseSettings.ParseMultiplier(element?.Element("Multiplier"));
            this.LeverDirections = ParseSettings.ParseSideDisable(element?.Element("LeverSideDisable"));
        }

        /// <summary>If the v2 of sand is being used.</summary>
        public bool IsV2 { get; private set; }

        /// <summary>Multiplier of the deltaTime used in the animation of the sand block type.</summary>
        public float Multiplier { get; private set; }

        /// <summary>Directions the sand lever can be activated from.</summary>
        public BitVector32 LeverDirections { get; private set; }
    }
}
