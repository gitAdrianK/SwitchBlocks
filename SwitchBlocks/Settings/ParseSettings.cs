namespace SwitchBlocks.Settings
{
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

        /// <summary>
        /// Parses a Duration element number in seconds and return the rounded up integer value of
        /// the specified durationin Jump King ticks.
        /// </summary>
        /// <param name="xmlElement">Duration element</param>
        /// <returns>The duration inside the element in ticks</returns>
        public static int ParseDuration(XmlElement xmlElement)
            => (int)((float.Parse(xmlElement
                .InnerText
                .Replace(",", "."),
                CultureInfo.InvariantCulture) / DeltaTime) + 0.5f);

        /// <summary>
        /// Parses the Multiplier element.
        /// </summary>
        /// <param name="xmlElement">Multiplier element</param>
        /// <returns>The multiplier if found or 1.0f</returns>
        public static float ParseMultiplier(XmlElement xmlElement)
            => float.Parse(xmlElement
                .InnerText
                .Replace(",", "."),
                CultureInfo.InvariantCulture);

        /// <summary>
        /// Looks for a "LeverSideDisable" element and parses the inside to look for directions
        /// "Up", "Down", "Left", "Right" and removes those from the bitvector of possible directions.
        /// </summary>
        /// <param name="xmlElement">Side disable element</param>
        /// <returns>The bitvector with specified sides disabled</returns>
        public static BitVector32 ParseSideDisable(XmlElement xmlElement)
        {
            var inside = xmlElement.InnerText.Trim();
            var directions = new BitVector32((int)Direction.All);
            if (inside == string.Empty)
            {
                return directions;
            }
            var splits = inside.Split(',');
            foreach (var split in splits)
            {
                switch (split.Trim().ToLower())
                {
                    case "up":
                        directions[(int)Direction.Up] = false;
                        break;
                    case "down":
                        directions[(int)Direction.Down] = false;
                        break;
                    case "left":
                        directions[(int)Direction.Left] = false;
                        break;
                    case "right":
                        directions[(int)Direction.Right] = false;
                        break;
                    default:
                        break;
                }
            }
            return directions;
        }

        /// <summary>
        /// Parses a Count element number and returns it as integer.
        /// </summary>
        /// <param name="xmlElement">Warn count element</param>
        /// <returns>The count inside the element</returns>
        public static int ParseWarnCount(XmlElement xmlElement)
            => int.Parse(xmlElement.InnerText);

        /// <summary>
        /// Parses a Duration element number in seconds and returns the rounded up integer value of
        /// the specified duration in Jump King ticks.
        /// </summary>
        /// <param name="xmlElement">Warn duration element</param>
        /// <returns>The duration inside the element in ticks</returns>
        public static int ParseWarnDuration(XmlElement xmlElement)
            => (int)((float.Parse(xmlElement.InnerText
                .Replace(",", "."),
                CultureInfo.InvariantCulture) / DeltaTime) + 0.5f);

    }
}
