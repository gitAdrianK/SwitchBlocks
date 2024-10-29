using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using static SwitchBlocks.Util.Directions;

namespace SwitchBlocks.Settings
{
    public static class ParseSettings
    {
        /// <summary>
        /// Used to convert time seconds to ticks.
        /// </summary>
        private const float deltaTime = 0.01666667f;

        public static int ParseDuration(Dictionary<string, int> dictionary, XmlNode root)
        {
            return ParseDuration(dictionary, root, 3);
        }

        // Looks for a "Duration" node and returns the inside declared float duration in ticks or default duration. Rounded up.
        public static int ParseDuration(Dictionary<string, int> dictionary, XmlNode root, float defaultDuration)
        {
            return ParseDuration("Duration", dictionary, root, defaultDuration);
        }

        // Looks for a "Duration" node and returns the inside declared float duration in ticks or default duration. Rounded up.
        public static int ParseDuration(string TagName, Dictionary<string, int> dictionary, XmlNode root, float defaultDuration)
        {
            return ParseDuration(TagName, dictionary, root, (int)((defaultDuration / deltaTime) + 0.5f));
        }

        public static int ParseDuration(string TagName, Dictionary<string, int> dictionary, XmlNode root, int defaultDuration)
        {
            XmlNodeList children = root.ChildNodes;
            int duration = defaultDuration;
            if (dictionary.ContainsKey(TagName))
            {
                duration = (int)((float.Parse(children[dictionary[TagName]].InnerText, CultureInfo.InvariantCulture) / deltaTime) + 0.5f);
            }
            return duration;
        }

        // Looks for a "Multiplier" node and returns the inside declared float multiplier or 1.0f.
        public static float ParseMultiplier(Dictionary<string, int> dictionary, XmlNode root)
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

        private static HashSet<Direction> ParseSideDisable(string tag, Dictionary<string, int> dictionary, XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            HashSet<Direction> directions = new HashSet<Direction>() { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
            if (!dictionary.ContainsKey(tag))
            {
                return directions;
            }
            string inside = children[dictionary[tag]].InnerText.Trim();
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
                    directions.Remove(Direction.Up);
                }
                if (trim.Equals("Down"))
                {
                    directions.Remove(Direction.Down);
                }
                if (trim.Equals("Left"))
                {
                    directions.Remove(Direction.Left);
                }
                if (trim.Equals("Right"))
                {
                    directions.Remove(Direction.Right);
                }
            }
            return directions;

        }

        // Looks for a "LeverSideDisable" node and parses the inside to look for directions
        // "Up", "Down", "Left", "Right" and removes those from the hash set of possible directions.
        public static HashSet<Direction> ParseLeverSideDisable(Dictionary<string, int> dictionary, XmlNode root)
        {
            return ParseSideDisable("LeverSideDisable", dictionary, root);
        }

        // Looks for a "PlatformSideDisable" node and parses the inside to look for directions
        // "Up", "Down", "Left", "Right" and removes those from the hash set of possible directions.
        public static HashSet<Direction> ParsePlatformSideDisable(Dictionary<string, int> dictionary, XmlNode root)
        {
            return ParseSideDisable("PlatformSideDisable", dictionary, root);
        }

        public static bool ParseForceSwitch(Dictionary<string, int> dictionary)
        {
            return dictionary.ContainsKey("ForceStateSwitch");
        }

        public static int ParseWarnCount(Dictionary<string, int> dictionary, XmlNode root)
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

        public static int ParseWarnDuration(Dictionary<string, int> dictionary, XmlNode root)
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

        public static bool ParseWarnDisableOn(Dictionary<string, int> dictionary)
        {
            return dictionary.ContainsKey("DisableOn");
        }

        public static bool ParseWarnDisableOff(Dictionary<string, int> dictionary)
        {
            return dictionary.ContainsKey("DisableOff");
        }
    }
}
