using System.Collections.Generic;
using System.Xml;

namespace SwitchBlocks.Settings
{
    public static class SettingsJump
    {
        /// <summary>
        /// Whether the jump block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; private set; } = false;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the jump block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            Dictionary<string, int> dictionaryJump = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryJump, block);
        }
    }
}
