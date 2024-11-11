using System.Collections.Generic;
using System.Xml;
using static SwitchBlocks.Util.Directions;

namespace SwitchBlocks.Settings
{
    public static class SettingsBasic
    {
        /// <summary>
        /// Whether the basic block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the basic block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from.
        /// </summary>
        public static HashSet<Direction> LeverDirections { get; private set; } = new HashSet<Direction>() { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            Dictionary<string, int> dictionaryBasic = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryBasic, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryBasic, block);
        }
    }
}
