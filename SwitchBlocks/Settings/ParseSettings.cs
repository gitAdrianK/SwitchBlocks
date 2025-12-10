namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Util;

    public static class ParseSettings
    {
        /// <summary>
        ///     Parses the <see cref="XElement" />s value to its duration in ticks.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <param name="defaultDuration">Default duration if the <see cref="XElement" /> cannot be parsed in ticks.</param>
        /// <returns>Duration in ticks.</returns>
        public static int ParseDuration(XElement element, int defaultDuration)
            => float.TryParse(element?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                ? (int)((result / ModConstants.DeltaTime) + 0.5f)
                : defaultDuration;

        /// <summary>
        ///     Parses the <see cref="XElement" />s value to its duration in ticks.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <param name="defaultDuration">Default duration if the <see cref="XElement" /> cannot be parsed in seconds.</param>
        /// <returns>Duration in ticks.</returns>
        public static int ParseDuration(XElement element, float defaultDuration)
            => ParseDuration(element, (int)((defaultDuration / ModConstants.DeltaTime) + 0.5f));

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
        /// <returns><see cref="Direction" /> bit flags.</returns>
        public static Direction ParseSideDisable(XElement element)
        {
            if (element is null)
            {
                return Direction.All;
            }

            var directions = Direction.All;
            foreach (var split in element.Value.Split(','))
            {
                switch (split.Trim().ToLower(CultureInfo.InvariantCulture))
                {
                    case "up":
                        directions &= ~Direction.Up;
                        break;
                    case "down":
                        directions &= ~Direction.Down;
                        break;
                    case "left":
                        directions &= ~Direction.Left;
                        break;
                    case "right":
                        directions &= ~Direction.Right;
                        break;
                }
            }

            return directions;
        }

        /// <summary>
        ///     Parses a comma seperated list to an array of integers.
        ///     If the element doesn't exist an array with the only entry being 1 is returned.
        /// </summary>
        /// <param name="element"><see cref="XElement" />.</param>
        /// <returns>Integer array.</returns>
        public static int[] ParseIntArray(XElement element) =>
            element?.Value.Split(',').Select(int.Parse).ToArray() ?? new[] { 1 };
    }
}
