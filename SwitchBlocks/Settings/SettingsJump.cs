namespace SwitchBlocks.Settings
{
    using System.Xml.Linq;

    public static class SettingsJump
    {
        /// <summary>
        /// Multiplier of the deltaTime used in the animation of the jump block type.
        /// </summary>
        public static float Multiplier { get; private set; } = 1.0f;
        /// <summary>
        /// If the jump state switch is supposed to be forced, ignoring the safe switch.
        /// </summary>
        public static bool ForceSwitch { get; private set; } = false;

        public static void Parse(XElement element)
        {
            Multiplier = ParseSettings.ParseMultiplier(element.Element("Multiplier"));
            ForceSwitch = element.Element("ForceStateSwitch") != null;
        }

        public static void Reset()
        {
            Multiplier = 1.0f;
            ForceSwitch = false;
        }
    }
}
