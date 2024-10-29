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
        public static int DurationOn { get; private set; } = 180;
        /// <summary>
        /// How long the blocks stay in their off state before switching.
        /// </summary>
        public static int DurationOff { get; private set; } = 180;
        /// <summary>
        /// How long a full on off cycle takes. 
        /// </summary>
        public static int DurationCycle { get; private set; } = 360;

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
        /// <summary>
        /// If the warn sound is disabled for the on state.
        /// </summary>
        public static bool WarnDisableOn { get; private set; } = false;
        /// <summary>
        /// If the warn sound is disabled for the off state.
        /// </summary>
        public static bool WarnDisableOff { get; private set; } = false;

        public static void Parse(XmlNode block)
        {
            IsUsed = true;
            XmlNodeList childrenAuto = block.ChildNodes;
            Dictionary<string, int> dictionaryAuto = Xml.MapNames(childrenAuto);
            DurationOn = ParseSettings.ParseDuration(dictionaryAuto, block);
            DurationOff = ParseSettings.ParseDuration("DurationOff", dictionaryAuto, block, DurationOn);
            DurationCycle = DurationOn + DurationOff;
            Multiplier = ParseSettings.ParseMultiplier(dictionaryAuto, block);
            ForceSwitch = ParseSettings.ParseForceSwitch(dictionaryAuto);
            if (dictionaryAuto.ContainsKey("Warn"))
            {
                XmlNode rootAutoWarn = childrenAuto[dictionaryAuto["Warn"]];
                Dictionary<string, int> dictionaryAutoWarn = Xml.MapNames(rootAutoWarn.ChildNodes);
                WarnCount = ParseSettings.ParseWarnCount(dictionaryAutoWarn, rootAutoWarn);
                WarnDuration = ParseSettings.ParseWarnDuration(dictionaryAutoWarn, rootAutoWarn);
                WarnDisableOn = ParseSettings.ParseWarnDisableOn(dictionaryAutoWarn);
                WarnDisableOff = ParseSettings.ParseWarnDisableOff(dictionaryAutoWarn);
            }
        }
    }
}
