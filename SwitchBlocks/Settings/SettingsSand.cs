namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml;
    using static SwitchBlocks.Util.Directions;

    public static class SettingsSand
    {
        /// <summary>
        /// Whether the sand block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; }
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the sand block type.
        /// </summary>
        public static float Multiplier { get; private set; } = ModConsts.DEFAULT_MULTIPLIER;
        /// <summary>
        /// Directions the sand lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);

        public static void Parse(XmlElement block)
        {
            Multiplier = ModConsts.DEFAULT_MULTIPLIER;
            LeverDirections = new BitVector32((int)Direction.All);

            foreach (XmlElement element in block)
            {
                switch (element.Name)
                {
                    case ModConsts.MULTIPLIER:
                        Multiplier = ParseSettings.ParseMultiplier(element);
                        break;
                    case ModConsts.LEVER_SIDE_DISABLE:
                        LeverDirections = ParseSettings.ParseSideDisable(element);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
