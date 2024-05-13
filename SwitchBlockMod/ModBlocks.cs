using JumpKing;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SwitchBlocksMod.Util
{
    /// <summary>
    /// Contains colors used in this mod for its blocks.
    /// </summary>
    public static class ModBlocks
    {
        /// <summary>
        /// Color that represents the auto on block. 
        /// </summary>
        public static readonly Color AUTO_ON = new Color(238, 124, 10);
        /// <summary>
        /// Color that represents the auto off block. 
        /// </summary>
        public static readonly Color AUTO_OFF = new Color(10, 124, 238);
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static float autoDuration = 3.0f;

        /// <summary>
        /// Color that represents the basic on block. 
        /// </summary>
        public static readonly Color BASIC_ON = new Color(238, 124, 20);
        /// <summary>
        /// Color that represents the basic off block. 
        /// </summary>
        public static readonly Color BASIC_OFF = new Color(20, 124, 238);
        /// <summary>
        /// Color that represents the basic solid lever block. 
        /// </summary>
        public static readonly Color BASIC_LEVER = new Color(238, 21, 124);
        /// <summary>
        /// Color that represents the basic lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color BASIC_LEVER_ON = new Color(238, 22, 124);
        /// <summary>
        /// Color that represents the basic lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color BASIC_LEVER_OFF = new Color(238, 23, 124);
        /// <summary>
        /// Color that represents the basic lever block. 
        /// </summary>
        public static readonly Color BASIC_LEVER_SOLID = new Color(238, 24, 124);
        /// <summary>
        /// Color that represents the basic solid lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color BASIC_LEVER_SOLID_ON = new Color(238, 25, 124);
        /// <summary>
        /// Color that represents the basic solid lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color BASIC_LEVER_SOLID_OFF = new Color(238, 26, 124);

        /// <summary>
        /// Color that represents the countdown on block. 
        /// </summary>
        public static readonly Color COUNTDOWN_ON = new Color(238, 124, 30);
        /// <summary>
        /// Color that represents the countdown off block. 
        /// </summary>
        public static readonly Color COUNTDOWN_OFF = new Color(30, 124, 238);
        /// <summary>
        /// Color that represents the countdown lever block. 
        /// </summary>
        public static readonly Color COUNTDOWN_LEVER = new Color(238, 31, 124);
        /// <summary>
        /// Color that represents the countdown solid lever block. 
        /// </summary>
        public static readonly Color COUNTDOWN_LEVER_SOLID = new Color(238, 34, 124);
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static float countdownDuration = 3.0f;

        /// <summary>
        /// Color that represents the jump on block. 
        /// </summary>
        public static readonly Color JUMP_ON = new Color(31, 31, 31);
        /// <summary>
        /// Color that represents the jump off block. 
        /// </summary>
        public static readonly Color JUMP_OFF = new Color(95, 95, 95);

        /// <summary>
        /// Color that represents the sand on block. 
        /// </summary>
        public static readonly Color SAND_ON = new Color(238, 124, 40);
        /// <summary>
        /// Color that represents the sand off block. 
        /// </summary>
        public static readonly Color SAND_OFF = new Color(40, 124, 238);
        /// <summary>
        /// Color that represents the sand lever block. 
        /// </summary>
        public static readonly Color SAND_LEVER = new Color(238, 41, 124);
        /// <summary>
        /// Color that represents the sand lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color SAND_LEVER_ON = new Color(238, 42, 124);
        /// <summary>
        /// Color that represents the sand lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color SAND_LEVER_OFF = new Color(238, 43, 124);
        /// <summary>
        /// Color that represents the sand solid lever block. 
        /// </summary>
        public static readonly Color SAND_LEVER_SOLID = new Color(238, 44, 124);
        /// <summary>
        /// Color that represents the sand solid lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color SAND_LEVER_SOLID_ON = new Color(238, 45, 124);
        /// <summary>
        /// Color that represents the sand solid lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color SAND_LEVER_SOLID_OFF = new Color(238, 46, 124);


        /// <summary>
        /// Loads the duration for blocks with such a field
        /// </summary>
        public static void LoadDuration()
        {
            char sep = Path.DirectorySeparatorChar;
            string path = $"{Game1.instance.contentManager.root}{sep}switchBlocksMod{sep}blocks.xml";
            if (!File.Exists(path))
            {
                return;
            }
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNode blocks = document.LastChild;
            if (blocks.Name != "Blocks")
            {
                return;
            }
            foreach (XmlNode block in blocks)
            {
                switch (block.Name)
                {
                    case "Auto":
                        Dictionary<string, int> dictionaryAuto = Xml.MapNames(block.ChildNodes);
                        autoDuration = ParseDuration(dictionaryAuto, block);
                        break;
                    case "Countdown":
                        Dictionary<string, int> dictionaryCountdown = Xml.MapNames(block.ChildNodes);
                        countdownDuration = ParseDuration(dictionaryCountdown, block);
                        break;
                    default:
                        // Do nothing.
                        break;
                }
            }
        }

        // Looks for a "Duration" node and returns the inside declared float duration or 3.0f.
        private static float ParseDuration(Dictionary<string, int> dictionary, XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            float duration = 3.0f;
            if (dictionary.ContainsKey("Duration"))
            {
                duration = float.Parse(children[dictionary["Duration"]].InnerText);
            }
            return duration;
        }

    }
}