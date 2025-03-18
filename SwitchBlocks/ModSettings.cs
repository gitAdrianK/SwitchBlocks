namespace SwitchBlocks
{
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Setups;

    public static class ModSettings
    {
        /// <summary>
        /// Loads the settings for blocks with such fields
        /// </summary>
        public static void Load()
        {
            SettingsAuto.Reset();
            SettingsBasic.Reset();
            SettingsCountdown.Reset();
            SettingsGroup.Reset();
            SettingsJump.Reset();
            SettingsSand.Reset();
            SettingsSequence.Reset();

            var file = Path.Combine(
                Game1.instance.contentManager.root,
                ModStrings.FOLDER,
                "blocks.xml");
            if (!File.Exists(file))
            {
                return;
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;
                if (root?.Name != "Blocks")
                {
                    return;
                }

                XElement xel;
                if (SetupAuto.IsUsed && (xel = root.Element("Auto")) != null)
                {
                    SettingsAuto.Parse(xel);
                }
                if (SetupBasic.IsUsed && (xel = root.Element("Basic")) != null)
                {
                    SettingsBasic.Parse(xel);
                }
                if (SetupCountdown.IsUsed && (xel = root.Element("Countdown")) != null)
                {
                    SettingsCountdown.Parse(xel);
                }
                if (SetupGroup.IsUsed && (xel = root.Element("Group")) != null)
                {
                    SettingsGroup.Parse(xel);
                }
                if (SetupJump.IsUsed && (xel = root.Element("Jump")) != null)
                {
                    SettingsJump.Parse(xel);
                }
                if (SetupSand.IsUsed && (xel = root.Element("Sand")) != null)
                {
                    SettingsSand.Parse(xel);
                }
                if (SetupSequence.IsUsed && (xel = root.Element("Sequence")) != null)
                {
                    SettingsSequence.Parse(xel);
                }
            }
        }
    }
}
