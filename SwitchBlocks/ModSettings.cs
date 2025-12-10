namespace SwitchBlocks
{
    using System.IO;
    using System.Xml.Linq;
    using JetBrains.Annotations;
    using JumpKing;
    using Settings;
    using Setups;

    /// <summary>
    ///     Collection of settings that are used by the mod and a way to load/reset them.
    /// </summary>
    public class ModSettings
    {
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

        [CanBeNull] public SettingsAuto SettingsAuto { get; }
        [CanBeNull] public SettingsBasic SettingsBasic { get; }
        [CanBeNull] public SettingsCountdown SettingsCountdown { get; }
        [CanBeNull] public SettingsGroup SettingsGroup { get; }
        [CanBeNull] public SettingsJump SettingsJump { get; }
        [CanBeNull] public SettingsSand SettingsSand { get; }
        [CanBeNull] public SettingsSequence SettingsSequence { get; }
    }
}
