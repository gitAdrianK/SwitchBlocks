using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Xml;

namespace SwitchBlocks.Settings
{
    public static class SettingsBasic
    {
        /// <summary>
        /// Whether the basic block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; private set; } = false;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the basic block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from.
        /// </summary>
        public static HashSet<Directions> LeverDirections { get; private set; } = new HashSet<Directions>() { Util.Directions.Up, Util.Directions.Down, Util.Directions.Left, Util.Directions.Right };

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            Dictionary<string, int> dictionaryBasic = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryBasic, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryBasic, block);
        }
    }
}
