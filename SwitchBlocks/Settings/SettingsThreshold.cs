namespace SwitchBlocks.Settings
{
    using System;
    using System.Xml.Linq;
    using JetBrains.Annotations;
    using Util;

    public class SettingsThreshold
    {
        /// <summary>
        ///     Parse the <see cref="XElement" /> to set the settings.
        /// </summary>
        /// <param name="element"><see cref="XElement" /> settings are to be taken from.</param>
        public SettingsThreshold([CanBeNull] XElement element)
        {
            this.Stat = Enum.TryParse(element?.Element("Stat")?.Value, true, out Stat parsed)
                ? parsed
                : Stat.Falls;
            this.Count = this.Stat != Stat.Time
                ? ParseSettings.ParseCount(element?.Element("Count"), 1) - 1
                : ParseSettings.ParseDuration(element?.Element("Count"), 60.0f);
            this.Multiplier = ParseSettings.ParseMultiplier(element?.Element("Multiplier"));
            this.ForceSwitch = XmlHelper.ParseElementBool(element, "ForceStateSwitch");
        }

        /// <summary>The stat to check for.</summary>
        public Stat Stat { get; }

        /// <summary>The threshold to check for.</summary>
        public int Count { get; }

        /// <summary>Multiplier of the deltaTime used in the animation of the threshold block type.</summary>
        public float Multiplier { get; }

        /// <summary>If the threshold state switch is supposed to be forced, ignoring the safe switch.</summary>
        public bool ForceSwitch { get; }
    }
}
