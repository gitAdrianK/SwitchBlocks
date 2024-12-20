namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml;
    using static SwitchBlocks.Util.Directions;

    public static class SettingsSequence
    {
        /// <summary>
        /// Whether the sequence block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; }
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = ModConsts.DEFAULT_NO_DURATION;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the sequence block type.
        /// </summary>
        public static float Multiplier { get; private set; } = ModConsts.DEFAULT_MULTIPLIER;
        /// <summary>
        /// Directions the sequence lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>
        /// Directions the sequence platform can be activated from.
        /// </summary>
        public static BitVector32 PlatformDirections { get; private set; } = new BitVector32((int)Direction.All);

        public static void Parse(XmlElement block)
        {
            Duration = ModConsts.DEFAULT_NO_DURATION;
            Multiplier = ModConsts.DEFAULT_MULTIPLIER;
            LeverDirections = new BitVector32((int)Direction.All);
            PlatformDirections = new BitVector32((int)Direction.All);

            foreach (XmlElement element in block)
            {
                switch (element.Name)
                {
                    case ModConsts.DURATION:
                        Duration = ParseSettings.ParseDuration(element);
                        break;
                    case ModConsts.MULTIPLIER:
                        Multiplier = ParseSettings.ParseMultiplier(element);
                        break;
                    case ModConsts.LEVER_SIDE_DISABLE:
                        LeverDirections = ParseSettings.ParseSideDisable(element);
                        break;
                    case ModConsts.PLATFORM_SIDE_DISABLE:
                        PlatformDirections = ParseSettings.ParseSideDisable(element);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
