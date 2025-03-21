namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Xml.Linq;
    using SwitchBlocks.Util;

    public static class ParseSettings
    {
        /// <summary>
        /// Used to convert time seconds to ticks.
        /// </summary>
        private const float DeltaTime = 0.01666667f;

        public static int ParseDuration(XElement element, int defaultDuration)
            => float.TryParse(element?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? (int)((result / DeltaTime) + 0.5f) : defaultDuration;

        public static int ParseDuration(XElement element, float defaultDuration)
            => ParseDuration(element, (int)((defaultDuration / DeltaTime) + 0.5f));

        public static float ParseMultiplier(XElement element)
            => float.TryParse(element?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : 1.0f;

        public static int ParseCount(XElement element, int defaultCount)
            => int.TryParse(element?.Value, out var result) ? result : defaultCount;

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
                    default:
                        break;
                }
            }
            return directions;
        }
    }
}
