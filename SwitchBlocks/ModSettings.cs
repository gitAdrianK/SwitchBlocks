namespace SwitchBlocks
{
    using System.IO;
    using System.Xml;
    using JumpKing;
    using SwitchBlocks.Settings;

    public static class ModSettings
    {
        /// <summary>
        /// Loads the settings for blocks with such fields
        /// </summary>
        public static void Load()
        {
            var document = new XmlDocument();
            var sep = Path.DirectorySeparatorChar;
            var path = $"{Game1.instance.contentManager.root}{sep}{ModStrings.FOLDER}{sep}blocks.xml";
            document.Load(path);
            var blocks = document.LastChild;
            if (blocks == null || blocks.Name != "Blocks")
            {
                return;
            }
            foreach (XmlNode block in blocks)
            {
                switch (block.Name)
                {
                    case "Auto":
                        SettingsAuto.Parse(block);
                        break;
                    case "Basic":
                        SettingsBasic.Parse(block);
                        break;
                    case "Countdown":
                        SettingsCountdown.Parse(block);
                        break;
                    case "Group":
                        SettingsGroup.Parse(block);
                        break;
                    case "Jump":
                        SettingsJump.Parse(block);
                        break;
                    case "Sand":
                        SettingsSand.Parse(block);
                        break;
                    case "Sequence":
                        SettingsSequence.Parse(block);
                        break;
                    default:
                        // Do nothing.
                        break;
                }
            }
        }
    }
}
