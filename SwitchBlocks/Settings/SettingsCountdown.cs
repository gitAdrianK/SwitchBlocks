namespace SwitchBlocks.Settings
{
    using System.Collections.Specialized;
    using System.Xml;
    using static SwitchBlocks.Util.Directions;

    public static class SettingsCountdown
    {
        /// <summary>
        /// Whether the countdown block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; }
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int Duration { get; private set; } = ModConsts.DEFAULT_DURATION;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the countdown block type.
        /// </summary>
        public static float Multiplier { get; private set; } = ModConsts.DEFAULT_MULTIPLIER;
        /// <summary>
        /// Directions the basic lever can be activated from.
        /// </summary>
        public static BitVector32 LeverDirections { get; private set; } = new BitVector32((int)Direction.All);
        /// <summary>
        /// If the countdown state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool ForceSwitch { get; private set; } = ModConsts.DEFAULT_FORCE;
        /// <summary>
        /// Amount of times the countdown warn sound is supposed to be played.
        /// </summary>
        public static int WarnCount { get; private set; } = ModConsts.DEFAULT_WARN_COUNT;
        /// <summary>
        /// Duration between countdown warn sounds.
        /// </summary>
        public static int WarnDuration { get; private set; } = ModConsts.DEFAULT_WARN_DURATION;
        /// <summary>
        /// If the warn sound is disabled for the on state.
        /// </summary>
        public static bool WarnDisableOn { get; private set; } = ModConsts.DEFAULT_WARN_DISABLE;
        /// <summary>
        /// If the warn sound is disabled for the off state.
        /// </summary>
        public static bool WarnDisableOff { get; private set; } = ModConsts.DEFAULT_WARN_DISABLE;

        public static void Parse(XmlElement block)
        {
            Duration = ModConsts.DEFAULT_DURATION;
            Multiplier = ModConsts.DEFAULT_MULTIPLIER;
            LeverDirections = new BitVector32((int)Direction.All);
            ForceSwitch = ModConsts.DEFAULT_FORCE;
            WarnCount = ModConsts.DEFAULT_WARN_COUNT;
            WarnDuration = ModConsts.DEFAULT_WARN_DURATION;
            WarnDisableOn = ModConsts.DEFAULT_WARN_DISABLE;
            WarnDisableOff = ModConsts.DEFAULT_WARN_DISABLE;

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
                    case ModConsts.FORCE_STATE_SWITCH:
                        ForceSwitch = true;
                        break;
                    case ModConsts.WARN:
                        foreach (XmlElement warnElement in element)
                        {
                            switch (warnElement.Name)
                            {
                                case ModConsts.COUNT:
                                    WarnCount = ParseSettings.ParseWarnCount(warnElement);
                                    break;
                                case ModConsts.DURATION:
                                    WarnDuration = ParseSettings.ParseWarnDuration(warnElement);
                                    break;
                                case ModConsts.DISABLE_ON:
                                    WarnDisableOn = true;
                                    break;
                                case ModConsts.DISABLE_OFF:
                                    WarnDisableOff = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
