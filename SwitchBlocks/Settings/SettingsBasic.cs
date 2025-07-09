namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using Util;

    public static class SettingsBasic
    {
        /// <summary>Multiplier of the deltaTime used in the animation of the basic block type.</summary>
        public static float Multiplier { get; private set; } = 1.0f;

        /// <summary>Directions the basic lever can be activated from.</summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);

        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public static void Parse(XElement element)
        {
            Multiplier = ParseSettings.ParseMultiplier(element.Element("Multiplier"));
            LeverDirections = ParseSettings.ParseSideDisable(element.Element("LeverSideDisable"));
        }

        /// <summary>
        ///     Resets all settings to their default values.
        /// </summary>
        public static void Reset()
        {
            Multiplier = 1.0f;
            LeverDirections = new BitVector32((int)Direction.All);
        }
    }
}
