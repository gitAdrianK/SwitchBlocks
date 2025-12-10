namespace SwitchBlocks.Settings
{
    using System.Xml.Linq;
    using JetBrains.Annotations;

    public class SettingsJump
    {
        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public SettingsJump([CanBeNull] XElement element)
        {
            this.Multiplier = ParseSettings.ParseMultiplier(element?.Element("Multiplier"));
            this.ForceSwitch = element?.Element("ForceStateSwitch") != null;
        }

        /// <summary>Multiplier of the deltaTime used in the animation of the jump block type.</summary>
        public float Multiplier { get; private set; }

        /// <summary>If the jump state switch is supposed to be forced, ignoring the safe switch.</summary>
        public bool ForceSwitch { get; private set; }
    }
}
