using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Xml;

namespace SwitchBlocks.Settings
{
    public static class SettingsJump
    {
        /// <summary>
        /// Whether the jump block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the jump block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// If the jump state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool ForceSwitch { get; private set; } = false;

        public static void Parse(XmlNode block)
        {
            Dictionary<string, int> dictionaryJump = Xml.MapNames(block.ChildNodes);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryJump, block);
            ForceSwitch = ParseSettings.ParseForceSwitch(dictionaryJump);
        }
    }
}
