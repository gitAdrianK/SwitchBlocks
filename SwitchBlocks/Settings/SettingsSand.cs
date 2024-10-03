using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Xml;

namespace SwitchBlocks.Settings
{
    public static class SettingsSand
    {
        /// <summary>
        /// Whether the sand block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; private set; } = false;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the sand block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the sand lever can be activated from.
        /// </summary>
        public static HashSet<Directions> LeverDirections { get; private set; } = new HashSet<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            Dictionary<string, int> dictionarySand = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionarySand, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionarySand, block);
        }
    }
}
