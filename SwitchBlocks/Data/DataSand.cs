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
        /// <summary>Singleton instance.</summary>
        private static DataSand instance;
        /// <summary>
        /// Returns the instance should it already exist.
        /// If it doesn't exist loads it from file.
        /// </summary>
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

        /// <summary>
        /// Sets the singleton instance to null.
        /// </summary>
        public void Reset() => instance = null;

        /// <summary>
        /// Private ctor.
        /// </summary>
        private DataSand()
        {
            this.State = false;
            this.HasSwitched = false;
            this.HasEntered = false;
        }

        /// <summary>
        /// Saves the data to file.
        /// </summary>
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

        /// <summary>Current state.</summary>
        public bool State { get; set; }
        /// <summary>Progress is not being saved between play sessions as it is unnecessary.</summary>
        public float Progress { get; set; }
        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }
        /// <summary>Whether the player is currently inside the block.</summary>
        public bool HasEntered { get; set; }
    }
}
