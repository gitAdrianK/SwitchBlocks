namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using JetBrains.Annotations;

    public class SettingsBasic
    {
        /// <summary>
        ///     Parse the <see cref="XElement" /> to create the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public SettingsBasic([CanBeNull] XElement element)
        {
            this.Multiplier = ParseSettings.ParseMultiplier(element?.Element("Multiplier"));
            this.LeverDirections = ParseSettings.ParseSideDisable(element?.Element("LeverSideDisable"));
        }

        /// <summary>Multiplier of the deltaTime used in the animation of the basic block type.</summary>
        public float Multiplier { get; private set; }

        /// <summary>Directions the basic lever can be activated from.</summary>
        public BitVector32 LeverDirections { get; private set; }
    }
}
