namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml;
    using SwitchBlocks.Util;
    using static SwitchBlocks.Util.Directions;

    public static class SettingsBasic
    {
        /// <summary>
        /// Whether the basic block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the basic block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);

        public static void Parse(XmlNode block)
        {
            var dictionaryBasic = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryBasic, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryBasic, block);
        }

        public static void Reset()
        {
            Multiplier = 1.0f;
            LeverDirections = new BitVector32((int)Direction.All);
        }
    }
}
