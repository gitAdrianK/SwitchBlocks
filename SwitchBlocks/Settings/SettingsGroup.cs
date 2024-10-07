using System.Collections.Generic;
using System.Xml;
using static SwitchBlocks.Util.Directions;

namespace SwitchBlocks.Settings
{
    public static class SettingsGroup
    {
        /// <summary>
        /// Whether the group block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; private set; } = false;
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = 0;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the group block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the group lever can be activated from.
        /// </summary>
        public static HashSet<Direction> LeverDirections { get; private set; } = new HashSet<Direction>() { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            XmlNodeList childrenGroup = block.ChildNodes;
            Dictionary<string, int> dictionaryGroup = Xml.MapNames(childrenGroup);
            Duration = ParseSettings.ParseDuration(dictionaryGroup, block, 0);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryGroup, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryGroup, block);
        }
    }
}
