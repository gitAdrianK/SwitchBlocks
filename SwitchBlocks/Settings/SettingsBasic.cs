namespace SwitchBlocks.Settings
{
    using System.Xml.Linq;
    using JetBrains.Annotations;
    using Util;

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
            this.SaveCarriesOver = XmlHelper.ParseElementBool(element, "SaveCarriesOver");
        }

        /// <summary>Multiplier of the deltaTime used in the animation of the basic block type.</summary>
        public float Multiplier { get; private set; }

        /// <summary>Directions the basic lever can be activated from.</summary>
        public Direction LeverDirections { get; private set; }

        /// <summary>If the save carries over when starting a new game.</summary>
        public bool SaveCarriesOver { get; private set; }
    }
}
