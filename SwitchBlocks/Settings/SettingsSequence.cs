using System.Collections.Generic;
using System.Xml;
using static SwitchBlocks.Util.Directions;

namespace SwitchBlocks.Settings
{
    public static class SettingsSequence
    {
        /// <summary>
        /// Whether the sequence block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; private set; } = false;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the sequence block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the sequence lever can be activated from.
        /// </summary>
        public static HashSet<Direction> LeverDirections { get; private set; } = new HashSet<Direction>() { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            XmlNodeList childrenGroup = block.ChildNodes;
            Dictionary<string, int> dictionaryGroup = Xml.MapNames(childrenGroup);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryGroup, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryGroup, block);
        }
    }
}
