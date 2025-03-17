namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml;
    using SwitchBlocks.Util;
    using static SwitchBlocks.Util.Directions;

    public static class SettingsSand
    {
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the sand block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the sand lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);

        public static void Parse(XmlNode block)
        {
            var dictionarySand = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionarySand, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionarySand, block);
        }

        public static void Reset()
        {
            Multiplier = 1.0f;
            LeverDirections = new BitVector32((int)Direction.All);
        }
    }
}
