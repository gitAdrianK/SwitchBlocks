namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Util;

    public static class ParseSettings
    {
        /// <summary>Used to convert time seconds to ticks.</summary>
        private const float DeltaTime = 0.01666667f;

        /// <summary>
        ///     Parses the <see cref="XElement" />s value to its duration in ticks.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <param name="defaultDuration">Default duration if the <see cref="XElement" /> cannot be parsed in ticks.</param>
        /// <returns>Duration in ticks.</returns>
        public static int ParseDuration(XElement element, int defaultDuration)
            => float.TryParse(element?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                ? (int)((result / DeltaTime) + 0.5f)
                : defaultDuration;

        /// <summary>
        ///     Parses the <see cref="XElement" />s value to its duration in ticks.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <param name="defaultDuration">Default duration if the <see cref="XElement" /> cannot be parsed in seconds.</param>
        /// <returns>Duration in ticks.</returns>
        public static int ParseDuration(XElement element, float defaultDuration)
            => ParseDuration(element, (int)((defaultDuration / DeltaTime) + 0.5f));

        /// <summary>
        ///     Parses the <see cref="XElement" />s value to a multiplier.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <returns>Multiplier. <c>1.0f</c> if the value cannot be parsed.</returns>
        public static float ParseMultiplier(XElement element)
            => float.TryParse(element?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                ? result
                : 1.0f;

        /// <summary>
        ///     Parses the <see cref="XElement" />s value to its count.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <param name="defaultCount">Default count if the <see cref="XElement" /> cannot be parsed.</param>
        /// <returns>Count amount.</returns>
        public static int ParseCount(XElement element, int defaultCount)
            => int.TryParse(element?.Value, out var result) ? result : defaultCount;

        /// <summary>
        ///     Parses the <see cref="XElement" />s value into a <see cref="BitVector32" /> representing <see cref="Direction" />s
        ///     that weren't disabled.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <returns><see cref="BitVector32" /> with not disabled <see cref="Direction" />s.</returns>
        public static BitVector32 ParseSideDisable(XElement element)
        {
            if (element == null)
            {
                return new BitVector32((int)Direction.All);
            }

            var directions = new BitVector32((int)Direction.All);
            foreach (var split in element.Value.Split(','))
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
                }
            }

            return directions;
        }

        /// <summary>
        ///     Parses a comma seperated list to an array of integers.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <returns>Integer array.</returns>
        public static int[] ParseIntArray(XElement element) =>
            element == null ? new[] { 0 } : element.Value.Split(',').Select(int.Parse).ToArray();
    }
}
