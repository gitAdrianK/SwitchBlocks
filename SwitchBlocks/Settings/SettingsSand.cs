using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using static SwitchBlocks.Util.Directions;

namespace SwitchBlocks.Settings
{
    public static class SettingsSand
    {
        /// <summary>
        /// Whether the sand block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; set; } = false;
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
            Dictionary<string, int> dictionarySand = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionarySand, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionarySand, block);
        }
    }
}
