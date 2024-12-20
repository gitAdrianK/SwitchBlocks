namespace SwitchBlocks.Settings
{
    using System.Xml;

    public static class SettingsAuto
    {
        /// <summary>
        /// Whether the auto block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; }
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static int DurationOn { get; private set; } = ModConsts.DEFAULT_DURATION;
        /// <summary>
        /// How long the blocks stay in their off state before switching.
        /// </summary>
        public static int DurationOff { get; private set; } = ModConsts.DEFAULT_DURATION;
        /// <summary>
        /// How long a full on off cycle takes. 
        /// </summary>
        public static int DurationCycle { get; private set; } = ModConsts.DEFAULT_DURATION + ModConsts.DEFAULT_DURATION;
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the auto block type.
        /// </summary>
        public static float Multiplier { get; private set; } = ModConsts.DEFAULT_MULTIPLIER;
        /// <summary>
        /// If the auto state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool ForceSwitch { get; private set; } = ModConsts.DEFAULT_FORCE;
        /// <summary>
        /// Amount of times the auto warn sound is supposed to be played.
        /// </summary>
        public static int WarnCount { get; private set; } = ModConsts.DEFAULT_WARN_COUNT;
        /// <summary>
        /// Duration between auto warn sounds.
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
            DurationOn = ModConsts.DEFAULT_DURATION;
            DurationOff = DurationOn;
            Multiplier = ModConsts.DEFAULT_MULTIPLIER;
            ForceSwitch = ModConsts.DEFAULT_FORCE;
            WarnCount = ModConsts.DEFAULT_WARN_COUNT;
            WarnDuration = ModConsts.DEFAULT_WARN_DURATION;
            WarnDisableOn = ModConsts.DEFAULT_WARN_DISABLE;
            WarnDisableOff = ModConsts.DEFAULT_WARN_DISABLE;

            var hasDurationOff = false;
            foreach (XmlElement element in block)
            {
                switch (element.Name)
                {
                    case ModConsts.DURATION:
                        DurationOn = ParseSettings.ParseDuration(element);
                        if (!hasDurationOff)
                        {
                            DurationOff = DurationOn;
                        }
                        break;
                    case ModConsts.DURATION_OFF:
                        DurationOff = ParseSettings.ParseDuration(element);
                        hasDurationOff = true;
                        break;
                    case ModConsts.MULTIPLIER:
                        Multiplier = ParseSettings.ParseMultiplier(element);
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

            DurationCycle = DurationOn + DurationOff;
        }
    }
}
