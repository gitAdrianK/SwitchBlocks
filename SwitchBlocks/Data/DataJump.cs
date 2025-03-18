namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the jump block.
    /// </summary>
    public class DataJump : IDataProvider
    {

        private static DataJump instance;
        public static DataJump Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                var file = Path.Combine(
                    Game1.instance.contentManager.root,
                    ModStrings.FOLDER,
                    ModStrings.SAVES,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.JUMP}{ModStrings.SUFFIX_SAV}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataJump();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    instance = new DataJump
                    {
                        State = bool.Parse(root.Element(ModStrings.SAVE_STATE).Value),
                        Progress = float.Parse(root.Element(ModStrings.SAVE_PROGRESS).Value, CultureInfo.InvariantCulture),
                        CanSwitchSafely = bool.Parse(root.Element(ModStrings.SAVE_CSS).Value),
                        SwitchOnceSafe = bool.Parse(root.Element(ModStrings.SAVE_SOS).Value),
                    };
                }
                return instance;
            }
        }

        public void Reset() => instance = null;

        private DataJump()
        {
            this.State = false;
            this.Progress = 0.0f;
            this.CanSwitchSafely = true;
            this.SwitchOnceSafe = false;
        }

        public void SaveToFile()
        {
            var path = Path.Combine(
                Game1.instance.contentManager.root,
                ModStrings.FOLDER,
                ModStrings.SAVES);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XDocument(
                new XElement("DataJump",
                    new XElement(ModStrings.SAVE_STATE, this.State),
                    new XElement(ModStrings.SAVE_PROGRESS, this.Progress),
                    new XElement(ModStrings.SAVE_CSS, this.CanSwitchSafely),
                    new XElement(ModStrings.SAVE_SOS, this.SwitchOnceSafe)
                )
            );

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.JUMP}{ModStrings.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        /// <summary>
        /// Its current state.
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// Animation progress.
        /// </summary>
        public float Progress { get; set; }

        /// <summary>
        /// If the block can switch safely.
        /// </summary>
        public bool CanSwitchSafely { get; set; }

        /// <summary>
        /// If the block should switch next opportunity.
        /// </summary>
        public bool SwitchOnceSafe { get; set; }
    }
}
