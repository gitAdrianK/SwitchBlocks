using JumpKing;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Util;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SwitchBlocksMod
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
        /// Multiplier of the deltaTime used in the animation of the auto block type.
        /// </summary>
        public static float autoMultiplier = 1.0f;

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
        /// Multiplier of the deltaTime used in the animation of the basic block type.
        /// </summary>
        public static float basicMultiplier = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from
        /// </summary>
        public static List<Directions> basicDirections = new List<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };

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
        /// Multiplier of the deltaTime used in the animation of the countdown block type.
        /// </summary>
        public static float countdownMultiplier = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from
        /// </summary>
        public static List<Directions> countdownDirections = new List<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };

        /// <summary>
        /// Color that represents the jump on block. 
        /// </summary>
        public static readonly Color JUMP_ON = new Color(31, 31, 31);
        /// <summary>
        /// Color that represents the jump off block. 
        /// </summary>
        public static readonly Color JUMP_OFF = new Color(95, 95, 95);
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the jump block type.
        /// </summary>
        public static float jumpMultiplier = 1.0f;

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
        /// Multiplier of the deltaTime used in the animation of the sand block type.
        /// </summary>
        public static float sandMultiplier = 1.0f;
        /// <summary>
        /// Directions the sand lever can be activated from
        /// </summary>
        public static List<Directions> sandDirections = new List<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };


        /// <summary>
        /// Loads the duration for blocks with such a field
        /// </summary>
        public static void LoadDuration()
        {
            char sep = Path.DirectorySeparatorChar;
            string path = $"{Game1.instance.contentManager.root}{sep}{ModStrings.FOLDER}{sep}blocks.xml";
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
                        autoMultiplier = ParseMultiplier(dictionaryAuto, block);
                        break;
                    case "Basic":
                        Dictionary<string, int> dictionaryBasic = Xml.MapNames(block.ChildNodes);
                        basicMultiplier = ParseMultiplier(dictionaryBasic, block);
                        basicDirections = ParseLeverSideDisable(dictionaryBasic, block);
                        break;
                    case "Countdown":
                        Dictionary<string, int> dictionaryCountdown = Xml.MapNames(block.ChildNodes);
                        countdownDuration = ParseDuration(dictionaryCountdown, block);
                        countdownDirections = ParseLeverSideDisable(dictionaryCountdown, block);
                        break;
                    case "Jump":
                        Dictionary<string, int> dictionaryJump = Xml.MapNames(block.ChildNodes);
                        jumpMultiplier = ParseMultiplier(dictionaryJump, block);
                        break;
                    case "Sand":
                        Dictionary<string, int> dictionarySand = Xml.MapNames(block.ChildNodes);
                        sandMultiplier = ParseMultiplier(dictionarySand, block);
                        sandDirections = ParseLeverSideDisable(dictionarySand, block);
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

        // Looks for a "Multiplier" node and returns the inside declared float multiplier or 1.0f.
        private static float ParseMultiplier(Dictionary<string, int> dictionary, XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            float multiplier = 1.0f;
            if (dictionary.ContainsKey("Multiplier"))
            {
                multiplier = float.Parse(children[dictionary["Multiplier"]].InnerText);
            }
            return multiplier;
        }

        // Looks for a "LeverSideDisable" node and parses the inside to look for directions
        // "Up", "Down", "Left", "Right" and removes those from the list of possible directions.
        private static List<Directions> ParseLeverSideDisable(Dictionary<string, int> dictionary, XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            List<Directions> directions = new List<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };
            if (!dictionary.ContainsKey("LeverSideDisable"))
            {
                return directions;
            }
            string inside = children[dictionary["LeverSideDisable"]].InnerText;
            if (inside == string.Empty)
            {
                return directions;
            }
            string[] split = inside.Split(',');
            foreach (string s in split)
            {
                string trim = s.Trim();
                if (trim.Equals("Up"))
                {
                    directions.Remove(Directions.Up);
                }
                if (trim.Equals("Down"))
                {
                    directions.Remove(Directions.Down);
                }
                if (trim.Equals("Left"))
                {
                    directions.Remove(Directions.Left);
                }
                if (trim.Equals("Right"))
                {
                    directions.Remove(Directions.Right);
                }
            }
            return directions;
        }

    }
}