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
        /// Used to convert time seconds to ticks.
        /// </summary>
        private const float deltaTime = 0.01666667f;

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
        /// Color that represents the auto ice on block. 
        /// </summary>
        public static readonly Color AUTO_ICE_ON = new Color(238, 124, 11);
        /// <summary>
        /// Color that represents the auto ice off block. 
        /// </summary>
        public static readonly Color AUTO_ICE_OFF = new Color(11, 124, 238);
        /// <summary>
        /// Color that represents the auto snow on block. 
        /// </summary>
        public static readonly Color AUTO_SNOW_ON = new Color(238, 124, 12);
        /// <summary>
        /// Color that represents the auto snow off block. 
        /// </summary>
        public static readonly Color AUTO_SNOW_OFF = new Color(12, 124, 238);
        /// <summary>
        /// Color that represents the auto reset block. 
        /// </summary>
        public static readonly Color AUTO_RESET = new Color(238, 11, 124);
        /// <summary>
        /// Color that represents the auto reset block. 
        /// </summary>
        public static readonly Color AUTO_RESET_FULL = new Color(238, 12, 124);
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int AutoDuration { get; private set; } = 180;
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
        /// Duration between auto warn sounds.
        /// </summary>
        public static int AutoWarnDuration { get; private set; } = 60;

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
        /// Color that represents the basic ice on block. 
        /// </summary>
        public static readonly Color BASIC_ICE_ON = new Color(238, 124, 21);
        /// <summary>
        /// Color that represents the basic ice off block. 
        /// </summary>
        public static readonly Color BASIC_ICE_OFF = new Color(21, 124, 238);
        /// <summary>
        /// Color that represents the basic snow on block. 
        /// </summary>
        public static readonly Color BASIC_SNOW_ON = new Color(238, 124, 22);
        /// <summary>
        /// Color that represents the basic snow off block. 
        /// </summary>
        public static readonly Color BASIC_SNOW_OFF = new Color(22, 124, 238);
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
        /// Directions the basic lever can be activated from.
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
        /// Color that represents the countdown ice on block. 
        /// </summary>
        public static readonly Color COUNTDOWN_ICE_ON = new Color(238, 124, 31);
        /// <summary>
        /// Color that represents the countdown ice off block. 
        /// </summary>
        public static readonly Color COUNTDOWN_ICE_OFF = new Color(31, 124, 238);
        /// <summary>
        /// Color that represents the countdown snow on block. 
        /// </summary>
        public static readonly Color COUNTDOWN_SNOW_ON = new Color(238, 124, 32);
        /// <summary>
        /// Color that represents the countdown snow off block. 
        /// </summary>
        public static readonly Color COUNTDOWN_SNOW_OFF = new Color(32, 124, 238);
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
        public static int CountdownDuration { get; private set; } = 180;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the countdown block type.
        /// </summary>
        public static float CountdownMultiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the basic lever can be activated from.
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
        /// Duration between countdown warn sounds.
        /// </summary>
        public static int CountdownWarnDuration { get; private set; } = 60;

        /// <summary>
        /// Whether the group block is inside the blocks.xml and counts as "used/enabled"
        /// </summary>
        public static bool IsGroupUsed { get; private set; } = false;
        /// <summary>
        /// Color that represents the group A block. 
        /// </summary>
        public static readonly Color GROUP_A = new Color(238, 124, 50);
        /// <summary>
        /// Color that represents the group B block. 
        /// </summary>
        public static readonly Color GROUP_B = new Color(50, 124, 238);
        /// <summary>
        /// Color that represents the group C block. 
        /// </summary>
        public static readonly Color GROUP_C = new Color(124, 238, 50);
        /// <summary>
        /// Color that represents the group D block. 
        /// </summary>
        public static readonly Color GROUP_D = new Color(50, 238, 124);
        /// <summary>
        /// Color that represents the group ice A block. 
        /// </summary>
        public static readonly Color GROUP_ICE_A = new Color(238, 124, 51);
        /// <summary>
        /// Color that represents the group ice B block. 
        /// </summary>
        public static readonly Color GROUP_ICE_B = new Color(51, 124, 238);
        /// <summary>
        /// Color that represents the group ice C block. 
        /// </summary>
        public static readonly Color GROUP_ICE_C = new Color(124, 238, 51);
        /// <summary>
        /// Color that represents the group ice D block. 
        /// </summary>
        public static readonly Color GROUP_ICE_D = new Color(51, 238, 124);
        /// <summary>
        /// Color that represents the group snow A block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_A = new Color(238, 124, 52);
        /// <summary>
        /// Color that represents the group snow B block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_B = new Color(52, 124, 238);
        /// <summary>
        /// Color that represents the group snow C block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_C = new Color(124, 238, 52);
        /// <summary>
        /// Color that represents the group snow D block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_D = new Color(52, 238, 124);
        /// <summary>
        /// Color that represents the group reset block. 
        /// </summary>
        public static readonly Color GROUP_RESET = new Color(238, 51, 124);
        /// <summary>
        /// Color that represents the countdown solid reset block. 
        /// </summary>
        public static readonly Color GROUP_RESET_SOLID = new Color(238, 54, 124);
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int GroupDuration { get; private set; } = 0;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the group block type.
        /// </summary>
        public static float GroupMultiplier { get; private set; } = 1.0f;
        /// <summary>
        /// Directions the group lever can be activated from.
        /// </summary>
        public static HashSet<Directions> GroupDirections { get; private set; } = new HashSet<Directions>() { Directions.Up, Directions.Down, Directions.Left, Directions.Right };

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
        /// Color that represents the jump ice on block. 
        /// </summary>
        public static readonly Color JUMP_ICE_ON = new Color(31, 32, 31);
        /// <summary>
        /// Color that represents the jump ice off block. 
        /// </summary>
        public static readonly Color JUMP_ICE_OFF = new Color(95, 96, 95);
        /// <summary>
        /// Color that represents the jump snow on block. 
        /// </summary>
        public static readonly Color JUMP_SNOW_ON = new Color(31, 33, 31);
        /// <summary>
        /// Color that represents the jump snow off block. 
        /// </summary>
        public static readonly Color JUMP_SNOW_OFF = new Color(95, 97, 95);
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
        /// Directions the sand lever can be activated from.
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
                        XmlNodeList childrenAuto = block.ChildNodes;
                        Dictionary<string, int> dictionaryAuto = Xml.MapNames(childrenAuto);
                        AutoDuration = ParseDuration(dictionaryAuto, block);
                        AutoMultiplier = ParseMultiplier(dictionaryAuto, block);
                        AutoForceSwitch = ParseForceSwitch(dictionaryAuto);
                        if (dictionaryAuto.ContainsKey("Warn"))
                        {
                            XmlNode rootAutoWarn = childrenAuto[dictionaryAuto["Warn"]];
                            Dictionary<string, int> dictionaryAutoWarn = Xml.MapNames(rootAutoWarn.ChildNodes);
                            AutoWarnCount = ParseWarnCount(dictionaryAutoWarn, rootAutoWarn);
                            AutoWarnDuration = ParseWarnDuration(dictionaryAutoWarn, rootAutoWarn);
                        }
                        break;
                    case "Basic":
                        IsBasicUsed = true;
                        Dictionary<string, int> dictionaryBasic = Xml.MapNames(block.ChildNodes);
                        BasicMultiplier = ParseMultiplier(dictionaryBasic, block);
                        BasicDirections = ParseLeverSideDisable(dictionaryBasic, block);
                        break;
                    case "Countdown":
                        IsCountdownUsed = true;
                        XmlNodeList childrenCountdown = block.ChildNodes;
                        Dictionary<string, int> dictionaryCountdown = Xml.MapNames(childrenCountdown);
                        CountdownDuration = ParseDuration(dictionaryCountdown, block);
                        CountdownMultiplier = ParseMultiplier(dictionaryCountdown, block);
                        CountdownDirections = ParseLeverSideDisable(dictionaryCountdown, block);
                        CountdownForceSwitch = ParseForceSwitch(dictionaryCountdown);
                        if (dictionaryCountdown.ContainsKey("Warn"))
                        {
                            XmlNode rootContdownWarn = childrenCountdown[dictionaryCountdown["Warn"]];
                            Dictionary<string, int> dictionaryCountdownWarn = Xml.MapNames(rootContdownWarn.ChildNodes);
                            CountdownWarnCount = ParseWarnCount(dictionaryCountdownWarn, rootContdownWarn);
                            CountdownWarnDuration = ParseWarnDuration(dictionaryCountdownWarn, rootContdownWarn);
                        }
                        break;
                    case "Group":
                        IsGroupUsed = true;
                        XmlNodeList childrenGroup = block.ChildNodes;
                        Dictionary<string, int> dictionaryGroup = Xml.MapNames(childrenGroup);
                        GroupDuration = ParseDuration(dictionaryGroup, block, 0);
                        GroupMultiplier = ParseMultiplier(dictionaryGroup, block);
                        GroupDirections = ParseLeverSideDisable(dictionaryGroup, block);
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

        private static int ParseDuration(Dictionary<string, int> dictionary, XmlNode root)
        {
            return ParseDuration(dictionary, root, 3);
        }

        // Looks for a "Duration" node and returns the inside declared float duration in ticks or default duration. Rounded up.
        private static int ParseDuration(Dictionary<string, int> dictionary, XmlNode root, float defaultDuration)
        {
            XmlNodeList children = root.ChildNodes;
            float duration = defaultDuration;
            if (dictionary.ContainsKey("Duration"))
            {
                duration = float.Parse(children[dictionary["Duration"]].InnerText, CultureInfo.InvariantCulture);
            }
            return (int)((duration / deltaTime) + 0.5f);
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
            if (!dictionary.ContainsKey("Count"))
            {
                return count;
            }
            count = int.Parse(children[dictionary["Count"]].InnerText);
            return count;
        }

        private static int ParseWarnDuration(Dictionary<string, int> dictionary, XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            float duration = 1.0f;
            if (!dictionary.ContainsKey("Duration"))
            {
                return (int)((duration / deltaTime) + 0.5f);
            }
            string dur = children[dictionary["Duration"]].InnerText;
            dur = dur.Trim();
            dur = dur.Replace(",", ".");
            duration = float.Parse(dur, CultureInfo.InvariantCulture);
            return (int)((duration / deltaTime) + 0.5f);
        }
    }
}
