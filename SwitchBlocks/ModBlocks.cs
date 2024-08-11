using JumpKing;
using Microsoft.Xna.Framework;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace SwitchBlocks
{
    /// <summary>
    /// Contains colors used in this mod for its blocks as well as various other properties
    /// that can be set for the types of blocks, like durations and animation speed multiplier.
    /// </summary>
    public static class ModBlocks
    {
        /// <summary>
        /// Whether the auto block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsAutoUsed { get; private set; } = false;
        /// <summary>
        /// Color that represents the auto on block. 
        /// </summary>
        public static readonly Color AUTO_ON = new Color(238, 124, 10);
        /// <summary>
        /// Color that represents the auto off block. 
        /// </summary>
        public static readonly Color AUTO_OFF = new Color(10, 124, 238);
        /// <summary>
        /// Color that represents the auto reset block. 
        /// </summary>
        public static readonly Color AUTO_RESET = new Color(238, 11, 124);
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static float AutoDuration { get; private set; } = 3.0f;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the auto block type.
        /// </summary>
        public static float AutoMultiplier { get; private set; } = 1.0f;
        /// <summary>
        /// If the auto state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool AutoForceSwitch { get; private set; } = false;
        /// <summary>
        /// Amount of times the auto warn sound is supposed to be played.
        /// </summary>
        public static int AutoWarnCount { get; private set; } = 2;

        /// <summary>
        /// Whether the basic block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsBasicUsed { get; private set; } = false;
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
        public static float BasicMultiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from
        /// </summary>
        public static HashSet<Directions> BasicDirections { get; private set; } = new HashSet<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };

        /// <summary>
        /// Whether the countdown block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsCountdownUsed { get; private set; } = false;
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
        public static float CountdownDuration { get; private set; } = 3.0f;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the countdown block type.
        /// </summary>
        public static float CountdownMultiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from
        /// </summary>
        public static HashSet<Directions> CountdownDirections { get; private set; } = new HashSet<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };
        /// <summary>
        /// If the countdown state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool CountdownForceSwitch { get; private set; } = false;
        /// <summary>
        /// Amount of times the countdown warn sound is supposed to be played.
        /// </summary>
        public static int CountdownWarnCount { get; private set; } = 2;

        /// <summary>
        /// Whether the jump block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsJumpUsed { get; private set; } = false;
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
        public static float JumpMultiplier { get; private set; } = 1.0f;

        /// <summary>
        /// Whether the sand block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsSandUsed { get; private set; } = false;
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
        public static float SandMultiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the sand lever can be activated from
        /// </summary>
        public static HashSet<Directions> SandDirections { get; private set; } = new HashSet<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };


        /// <summary>
        /// Loads the properties for blocks with such fields
        /// </summary>
        public static void LoadProperties()
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
                        IsAutoUsed = true;
                        Dictionary<string, int> dictionaryAuto = Xml.MapNames(block.ChildNodes);
                        AutoDuration = ParseDuration(dictionaryAuto, block);
                        AutoMultiplier = ParseMultiplier(dictionaryAuto, block);
                        AutoForceSwitch = ParseForceSwitch(dictionaryAuto);
                        AutoWarnCount = ParseWarnCount(dictionaryAuto, block);
                        break;
                    case "Basic":
                        IsBasicUsed = true;
                        Dictionary<string, int> dictionaryBasic = Xml.MapNames(block.ChildNodes);
                        BasicMultiplier = ParseMultiplier(dictionaryBasic, block);
                        BasicDirections = ParseLeverSideDisable(dictionaryBasic, block);
                        break;
                    case "Countdown":
                        IsCountdownUsed = true;
                        Dictionary<string, int> dictionaryCountdown = Xml.MapNames(block.ChildNodes);
                        CountdownDuration = ParseDuration(dictionaryCountdown, block);
                        CountdownMultiplier = ParseMultiplier(dictionaryCountdown, block);
                        CountdownDirections = ParseLeverSideDisable(dictionaryCountdown, block);
                        CountdownForceSwitch = ParseForceSwitch(dictionaryCountdown);
                        CountdownWarnCount = ParseWarnCount(dictionaryCountdown, block);
                        break;
                    case "Jump":
                        IsJumpUsed = true;
                        Dictionary<string, int> dictionaryJump = Xml.MapNames(block.ChildNodes);
                        JumpMultiplier = ParseMultiplier(dictionaryJump, block);
                        break;
                    case "Sand":
                        IsSandUsed = true;
                        Dictionary<string, int> dictionarySand = Xml.MapNames(block.ChildNodes);
                        SandMultiplier = ParseMultiplier(dictionarySand, block);
                        SandDirections = ParseLeverSideDisable(dictionarySand, block);
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
                duration = float.Parse(children[dictionary["Duration"]].InnerText, CultureInfo.InvariantCulture);
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
                string multi = children[dictionary["Multiplier"]].InnerText;
                multi = multi.Trim();
                multi = multi.Replace(",", ".");
                multiplier = float.Parse(multi, CultureInfo.InvariantCulture);
            }
            return multiplier;
        }

        // Looks for a "LeverSideDisable" node and parses the inside to look for directions
        // "Up", "Down", "Left", "Right" and removes those from the hash set of possible directions.
        private static HashSet<Directions> ParseLeverSideDisable(Dictionary<string, int> dictionary, XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            HashSet<Directions> directions = new HashSet<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };
            if (!dictionary.ContainsKey("LeverSideDisable"))
            {
                return directions;
            }
            string inside = children[dictionary["LeverSideDisable"]].InnerText.Trim();
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

        private static bool ParseForceSwitch(Dictionary<string, int> dictionary)
        {
            return dictionary.ContainsKey("ForceStateSwitch");
        }

        private static int ParseWarnCount(Dictionary<string, int> dictionary, XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            int count = 2;
            if (dictionary.ContainsKey("WarnCount"))
            {
                count = int.Parse(children[dictionary["WarnCount"]].InnerText);
            }
            return count;
        }
    }
}