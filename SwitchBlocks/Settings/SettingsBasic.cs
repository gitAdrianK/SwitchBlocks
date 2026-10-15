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
            this.ForceSwitch = XmlHelper.ParseElementBool(element, "ForceStateSwitch");
            this.LeverDirections = ParseSettings.ParseSideDisable(element?.Element("LeverSideDisable"));
            this.SaveCarriesOver = XmlHelper.ParseElementBool(element, "SaveCarriesOver");
            this.CanSwitchOnPress = XmlHelper.ParseElementBool(element, "CanSwitchOnPress");
        }

        /// <summary>Multiplier of the deltaTime used in the animation of the block type.</summary>
        public float Multiplier { get; }

        /// <summary>If the state switch is supposed to be forced, ignoring the safe switch.</summary>
        public bool ForceSwitch { get; }

        /// <summary>Directions the basic lever can be activated from.</summary>
        public Direction LeverDirections { get; }

        /// <summary>If the save carries over when starting a new game.</summary>
        public bool SaveCarriesOver { get; }

        ///<summary>If the state can be switched on button press.</summary>
        public bool CanSwitchOnPress { get; }
    }
}
