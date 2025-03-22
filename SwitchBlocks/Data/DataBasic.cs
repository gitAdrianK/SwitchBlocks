namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the basic block.
    /// </summary>
    public class DataBasic : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataBasic instance;
        /// <summary>
        /// Returns the instance should it already exist.
        /// If it doesn't exist loads it from file.
        /// </summary>
        public static DataBasic Instance
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
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.BASIC}{ModStrings.SUFFIX_SAV}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataBasic();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;

                    instance = new DataBasic
                    {
                        State = bool.Parse(root.Element(ModStrings.SAVE_STATE).Value),
                        Progress = float.Parse(root.Element(ModStrings.SAVE_PROGRESS).Value, CultureInfo.InvariantCulture),
                        HasSwitched = bool.Parse(root.Element(ModStrings.SAVE_HAS_SWITCHED).Value),
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
        private DataBasic()
        {
            this.State = false;
            this.Progress = 0.0f;
            this.HasSwitched = false;
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
                new XElement("DataBasic",
                    new XElement(ModStrings.SAVE_STATE, this.State),
                    new XElement(ModStrings.SAVE_PROGRESS, this.Progress),
                    new XElement(ModStrings.SAVE_HAS_SWITCHED, this.HasSwitched)
                )
            );

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.BASIC}{ModStrings.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        /// <summary>Current state</summary>
        public bool State { get; set; }
        /// <summary>Animation progress.</summary>
        public float Progress { get; set; }
        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }
    }
}
