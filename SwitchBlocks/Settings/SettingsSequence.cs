﻿using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using static SwitchBlocks.Util.Directions;

namespace SwitchBlocks.Settings
{
    public static class SettingsSequence
    {
        /// <summary>
        /// Whether the sequence block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = 0;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the sequence block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the sequence lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>
        /// Directions the sequence platform can be activated from.
        /// </summary>
        public static BitVector32 PlatformDirections { get; private set; } = new BitVector32((int)Direction.All);

        public static void Parse(XmlNode block)
        {
            XmlNodeList childrenGroup = block.ChildNodes;
            Dictionary<string, int> dictionaryGroup = Xml.MapNames(childrenGroup);
            Duration = ParseSettings.ParseDuration(dictionaryGroup, block, 0);
            Multiplier = ParseSettings.ParseMultiplier(dictionaryGroup, block);
            LeverDirections = ParseSettings.ParseLeverSideDisable(dictionaryGroup, block);
            PlatformDirections = ParseSettings.ParsePlatformSideDisable(dictionaryGroup, block);
        }
    }
}
