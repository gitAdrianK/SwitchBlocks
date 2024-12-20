namespace SwitchBlocks.Settings
{
    using System.Xml;

    public static class SettingsJump
    {
        /// <summary>
        /// Whether the jump block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; }
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the jump block type.
        /// </summary>
        public static float Multiplier { get; private set; } = ModConsts.DEFAULT_MULTIPLIER;
        /// <summary>
        /// If the jump state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool ForceSwitch { get; private set; } = ModConsts.DEFAULT_FORCE;

        public static void Parse(XmlElement block)
        {
            Multiplier = ModConsts.DEFAULT_MULTIPLIER;
            ForceSwitch = ModConsts.DEFAULT_FORCE;

            foreach (XmlElement element in block)
            {
                switch (element.Name)
                {
                    case ModConsts.MULTIPLIER:
                        Multiplier = ParseSettings.ParseMultiplier(element);
                        break;
                    case ModConsts.FORCE_STATE_SWITCH:
                        ForceSwitch = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
