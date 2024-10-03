using System.Collections.Generic;
using System.Xml;

namespace SwitchBlocks.Settings
{
    public static class SettingsAuto
    {
        /// <summary>
        /// Whether the auto block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; private set; } = false;
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = 180;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the auto block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// If the auto state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool ForceSwitch { get; private set; } = false;
        /// <summary>
        /// Amount of times the auto warn sound is supposed to be played.
        /// </summary>
        public static int WarnCount { get; private set; } = 2;
        /// <summary>
        /// Duration between auto warn sounds.
        /// </summary>
        public static int WarnDuration { get; private set; } = 60;

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            XmlNodeList childrenAuto = block.ChildNodes;
            Dictionary<string, int> dictionaryAuto = Xml.MapNames(childrenAuto);
            Duration = ParseSettings.ParseDuration(dictionaryAuto, block);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryAuto, block);
            ForceSwitch = ParseSettings.ParseForceSwitch(dictionaryAuto);
            if (dictionaryAuto.ContainsKey("Warn"))
            {
                XmlNode rootAutoWarn = childrenAuto[dictionaryAuto["Warn"]];
                Dictionary<string, int> dictionaryAutoWarn = Xml.MapNames(rootAutoWarn.ChildNodes);
                WarnCount = ParseSettings.ParseWarnCount(dictionaryAutoWarn, rootAutoWarn);
                WarnDuration = ParseSettings.ParseWarnDuration(dictionaryAutoWarn, rootAutoWarn);
            }
        }
    }
}
