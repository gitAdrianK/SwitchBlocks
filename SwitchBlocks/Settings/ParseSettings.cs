namespace SwitchBlocks.Settings
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Xml;
    using static SwitchBlocks.Util.Directions;

    public static class ParseSettings
    {
        /// <summary>
        /// Used to convert time seconds to ticks.
        /// </summary>
        private const float DeltaTime = 0.01666667f;

        public static int ParseDuration(Dictionary<string, int> dictionary, XmlNode root)
            => ParseDuration(dictionary, root, 3);

        // Looks for a "Duration" node and returns the inside declared float duration in ticks or default duration. Rounded up.
        public static int ParseDuration(Dictionary<string, int> dictionary, XmlNode root, float defaultDuration)
            => ParseDuration("Duration", dictionary, root, defaultDuration);

        // Looks for a "Duration" node and returns the inside declared float duration in ticks or default duration. Rounded up.
        public static int ParseDuration(string tagName, Dictionary<string, int> dictionary, XmlNode root, float defaultDuration)
            => ParseDuration(tagName, dictionary, root, (int)((defaultDuration / DeltaTime) + 0.5f));

        public static int ParseDuration(string tagName, Dictionary<string, int> dictionary, XmlNode root, int defaultDuration)
        {
            var children = root.ChildNodes;
            var duration = defaultDuration;
            if (dictionary.TryGetValue(tagName, out var value))
            {
                duration = (int)((float.Parse(children[value].InnerText, CultureInfo.InvariantCulture) / DeltaTime) + 0.5f);
            }
            return duration;
        }

        // Looks for a "Multiplier" node and returns the inside declared float multiplier or 1.0f.
        public static float ParseMultiplier(Dictionary<string, int> dictionary, XmlNode root)
        {
            var children = root.ChildNodes;
            var multiplier = 1.0f;
            if (dictionary.TryGetValue("Multiplier", out var value))
            {
                var multi = children[value]
                    .InnerText
                    .Trim()
                    .Replace(",", ".");
                multiplier = float.Parse(multi, CultureInfo.InvariantCulture);
            }
            return multiplier;
        }

        private static BitVector32 ParseSideDisable(string tag, Dictionary<string, int> dictionary, XmlNode root)
        {
            var children = root.ChildNodes;
            var directions = new BitVector32((int)Direction.All);
            if (!dictionary.TryGetValue(tag, out var value))
            {
                return directions;
            }
            var inside = children[value]
                .InnerText
                .Trim();
            if (inside == string.Empty)
            {
                return directions;
            }
            var splits = inside.Split(',');
            foreach (var split in splits)
            {
                var trim = split
                    .Trim()
                    .ToLower();
                if (trim.Equals("up"))
                {
                    directions[(int)Direction.Up] = false;
                }
                else if (trim.Equals("down"))
                {
                    directions[(int)Direction.Down] = false;
                }
                else if (trim.Equals("left"))
                {
                    directions[(int)Direction.Left] = false;
                }
                else if (trim.Equals("right"))
                {
                    directions[(int)Direction.Right] = false;
                }
            }
            return directions;

        }

        // Looks for a "LeverSideDisable" node and parses the inside to look for directions
        // "Up", "Down", "Left", "Right" and removes those from the hash set of possible directions.
        public static BitVector32 ParseLeverSideDisable(Dictionary<string, int> dictionary, XmlNode root)
            => ParseSideDisable("LeverSideDisable", dictionary, root);

        // Looks for a "PlatformSideDisable" node and parses the inside to look for directions
        // "Up", "Down", "Left", "Right" and removes those from the hash set of possible directions.
        public static BitVector32 ParsePlatformSideDisable(Dictionary<string, int> dictionary, XmlNode root)
            => ParseSideDisable("PlatformSideDisable", dictionary, root);

        public static bool ParseForceSwitch(Dictionary<string, int> dictionary)
            => dictionary.ContainsKey("ForceStateSwitch");

        public static int ParseWarnCount(Dictionary<string, int> dictionary, XmlNode root)
        {
            var children = root.ChildNodes;
            var count = 2;
            if (dictionary.TryGetValue("Count", out var value))
            {
                count = int.Parse(children[value].InnerText);
            }
            return count;
        }

        public static int ParseWarnDuration(Dictionary<string, int> dictionary, XmlNode root)
        {
            var children = root.ChildNodes;
            var duration = 1.0f;
            if (dictionary.TryGetValue("Duration", out var value))
            {
                var dur = children[value]
                    .InnerText
                    .Trim()
                    .Replace(",", ".");
                duration = float.Parse(dur, CultureInfo.InvariantCulture);
            }
            return (int)((duration / DeltaTime) + 0.5f);
        }

        public static bool ParseWarnDisableOn(Dictionary<string, int> dictionary)
            => dictionary.ContainsKey("DisableOn");

        public static bool ParseWarnDisableOff(Dictionary<string, int> dictionary)
            => dictionary.ContainsKey("DisableOff");
    }
}
