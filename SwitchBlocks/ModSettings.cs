using JumpKing;
using SwitchBlocks.Settings;
using System.IO;
using System.Xml;

namespace SwitchBlocks
{
    public static class ModSettings
    {
        /// <summary>
        /// Loads the settings for blocks with such fields
        /// </summary>
        public static void Load()
        {
            char sep = Path.DirectorySeparatorChar;
            string path = $"{Game1.instance.contentManager.root}{sep}{ModStrings.FOLDER}{sep}blocks.xml";
            if (!File.Exists(path))
            {
                return;
            }
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNode blocks = document.LastChild;
            if (blocks.Name != "Blocks")
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
