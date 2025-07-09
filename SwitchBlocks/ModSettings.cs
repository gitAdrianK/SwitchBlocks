namespace SwitchBlocks
{
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using Settings;
    using Setups;

    /// <summary>
    ///     Collection of settings that are used by the mod and a way to load/reset them.
    /// </summary>
    public static class ModSettings
    {
        /// <summary>
        ///     Loads the settings for blocks with such fields from a blocks.xml file
        ///     placed inside the mods root folder.
        /// </summary>
        public static void Setup()
        {
            var file = Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                "blocks.xml");
            if (!File.Exists(file))
            {
                return;
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;

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

        /// <summary>
        ///     Resets all settings to their default value.
        /// </summary>
        public static void Cleanup()
        {
            if (SetupAuto.IsUsed)
            {
                SettingsAuto.Reset();
            }

            if (SetupBasic.IsUsed)
            {
                SettingsBasic.Reset();
            }

            if (SetupCountdown.IsUsed)
            {
                SettingsCountdown.Reset();
            }

            if (SetupGroup.IsUsed)
            {
                SettingsGroup.Reset();
            }

            if (SetupJump.IsUsed)
            {
                SettingsJump.Reset();
            }

            if (SetupSand.IsUsed)
            {
                SettingsSand.Reset();
            }

            if (SetupSequence.IsUsed)
            {
                SettingsSequence.Reset();
            }
        }
    }
}
