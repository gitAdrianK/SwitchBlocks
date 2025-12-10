namespace SwitchBlocks
{
    using System.IO;
    using System.Xml.Linq;
    using JetBrains.Annotations;
    using JumpKing;
    using Settings;
    using Setups;

    /// <summary>
    ///     Collection of settings that are used by the mod.
    /// </summary>
    public class ModSettings
    {
        /// <summary>
        ///     Creates settings used by the mod. If a block type is not used it remains null and will not be set.
        /// </summary>
        public ModSettings()
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

                if (SetupAuto.IsUsed)
                {
                    this.SettingsAuto = new SettingsAuto(root?.Element("Auto"));
                }

                if (SetupBasic.IsUsed)
                {
                    this.SettingsBasic = new SettingsBasic(root?.Element("Basic"));
                }

                if (SetupCountdown.IsUsed)
                {
                    this.SettingsCountdown = new SettingsCountdown(root?.Element("Countdown"));
                }

                if (SetupGroup.IsUsed)
                {
                    this.SettingsGroup = new SettingsGroup(root?.Element("Group"));
                }

                if (SetupJump.IsUsed)
                {
                    this.SettingsJump = new SettingsJump(root?.Element("Jump"));
                }

                if (SetupSand.IsUsed)
                {
                    this.SettingsSand = new SettingsSand(root?.Element("Sand"));
                }

                if (SetupSequence.IsUsed)
                {
                    this.SettingsSequence = new SettingsSequence(root?.Element("Sequence"));
                }
            }
        }

        /// <summary>Settings for the auto block type.</summary>
        [CanBeNull]
        public SettingsAuto SettingsAuto { get; }

        /// <summary>Settings for the basic block type.</summary>
        [CanBeNull]
        public SettingsBasic SettingsBasic { get; }

        /// <summary>Settings for the countdown block type.</summary>
        [CanBeNull]
        public SettingsCountdown SettingsCountdown { get; }

        /// <summary>Settings for the group block type.</summary>
        [CanBeNull]
        public SettingsGroup SettingsGroup { get; }

        /// <summary>Settings for the jump block type.</summary>
        [CanBeNull]
        public SettingsJump SettingsJump { get; }

        /// <summary>Settings for the sand block type.</summary>
        [CanBeNull]
        public SettingsSand SettingsSand { get; }

        /// <summary>Settings for the sequence block type.</summary>
        [CanBeNull]
        public SettingsSequence SettingsSequence { get; }
    }
}
