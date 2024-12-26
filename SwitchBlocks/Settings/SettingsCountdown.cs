using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using static SwitchBlocks.Util.Directions;

namespace SwitchBlocks.Settings
{
    public static class SettingsCountdown
    {
        /// <summary>
        /// Whether the countdown block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = 180;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the countdown block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>
        /// If the countdown state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool ForceSwitch { get; private set; } = false;
        /// <summary>
        /// Amount of times the countdown warn sound is supposed to be played.
        /// </summary>
        public static int WarnCount { get; private set; } = 2;
        /// <summary>
        /// Duration between countdown warn sounds.
        /// </summary>
        public static int WarnDuration { get; private set; } = 60;

        public static void Parse(XmlNode block)
        {
            XmlNodeList childrenCountdown = block.ChildNodes;
            Dictionary<string, int> dictionaryCountdown = Xml.MapNames(childrenCountdown);
            Duration = ParseSettings.ParseDuration(dictionaryCountdown, block);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryCountdown, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryCountdown, block);
            ForceSwitch = ParseSettings.ParseForceSwitch(dictionaryCountdown);
            if (dictionaryCountdown.TryGetValue("Warn", out int value))
            {
                XmlNode rootContdownWarn = childrenCountdown[value];
                Dictionary<string, int> dictionaryCountdownWarn = Xml.MapNames(rootContdownWarn.ChildNodes);
                WarnCount = ParseSettings.ParseWarnCount(dictionaryCountdownWarn, rootContdownWarn);
                WarnDuration = ParseSettings.ParseWarnDuration(dictionaryCountdownWarn, rootContdownWarn);
            }

        }
    }
}
