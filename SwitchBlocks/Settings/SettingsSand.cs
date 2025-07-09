namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml.Linq;
    using Util;

    public static class SettingsSand
    {
        /// <summary>If the v2 of sand is being used.</summary>
        public static bool IsV2 { get; private set; }

        /// <summary>Multiplier of the deltaTime used in the animation of the sand block type.</summary>
        public static float Multiplier { get; private set; } = 1.0f;

        /// <summary>Directions the sand lever can be activated from.</summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);

        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public static void Parse(XElement element)
        {
            IsV2 = element.Element("v2") != null;
            Multiplier = ParseSettings.ParseMultiplier(element.Element("Multiplier"));
            LeverDirections = ParseSettings.ParseSideDisable(element.Element("LeverSideDisable"));
        }

        /// <summary>
        ///     Resets all settings to their default values.
        /// </summary>
        public static void Reset()
        {
            IsV2 = false;
            Multiplier = 1.0f;
            LeverDirections = new BitVector32((int)Direction.All);
        }
    }
}
