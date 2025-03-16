namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml;
    using SwitchBlocks.Util;
    using static SwitchBlocks.Util.Directions;

    public class SettingsWind
    {
        /// <summary>
        /// Whether the wind block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>
        /// Directions the basic lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);

        public static void Parse(XmlNode block)
        {
            var dictionaryBasic = Xml.MapNames(block.ChildNodes);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryBasic, block);
        }

        public static void Reset() => LeverDirections = new BitVector32((int)Direction.All);
    }
}
