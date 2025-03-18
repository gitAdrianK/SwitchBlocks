namespace SwitchBlocks.Data
{
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the sand block.
    /// </summary>
    public class DataSand : IDataProvider
    {

        private static DataSand instance;
        public static DataSand Instance
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
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.SAND}{ModStrings.SUFFIX_SAV}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataSand();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    instance = new DataSand
                    {
                        State = bool.Parse(root.Element(ModStrings.SAVE_STATE).Value),
                        HasSwitched = bool.Parse(root.Element(ModStrings.SAVE_HAS_SWITCHED).Value),
                        HasEntered = bool.Parse(root.Element(ModStrings.SAVE_HAS_ENTERED).Value),
                    };
                }
                return instance;
            }
        }

        public void Reset() => instance = null;

        private DataSand()
        {
            this.State = false;
            this.HasSwitched = false;
            this.HasEntered = false;
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
                new XElement("DataSand",
                    new XElement(ModStrings.SAVE_STATE, this.State),
                    new XElement(ModStrings.SAVE_HAS_SWITCHED, this.HasSwitched),
                    new XElement(ModStrings.SAVE_HAS_ENTERED, this.HasEntered)
                )
            );

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.SAND}{ModStrings.SUFFIX_SAV}"),
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
        /// Progress is not being saved between play sessions as it is unnecessary.
        /// </summary>
        public float Progress { get; set; }

        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }

        /// <summary>
        /// Whether the player is currently inside the block.
        /// </summary>
        public bool HasEntered { get; set; }
    }
}
