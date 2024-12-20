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
            var path = $"{Game1.instance.contentManager.root}{sep}{ModConsts.FOLDER}{sep}blocks.xml";
            document.Load(path);
            var blocks = document.SelectSingleNode("Blocks");
            if (blocks == null)
            {
                return;
            }
            // I'd imagine SelectSingleNode does not find the node in constant time but goes over all nodes
            // and returns the node if found. So we sweep over all nodes once instead.
            foreach (XmlElement element in blocks)
            {
                switch (element.Name)
                {
                    case "Auto":
                        SettingsAuto.Parse(element);
                        break;
                    case "Basic":
                        SettingsBasic.Parse(element);
                        break;
                    case "Countdown":
                        SettingsCountdown.Parse(element);
                        break;
                    case "Group":
                        SettingsGroup.Parse(element);
                        break;
                    case "Jump":
                        SettingsJump.Parse(element);
                        break;
                    case "Sand":
                        SettingsSand.Parse(element);
                        break;
                    case "Sequence":
                        SettingsSequence.Parse(element);
                        break;
                    default:
                        // Do nothing.
                        break;
                }
            }
        }
    }
}
