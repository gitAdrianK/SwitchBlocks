namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    ///     Contains data relevant for the basic block.
    /// </summary>
    public class DataBasic : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataBasic instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DataBasic()
        {
            this.State = false;
            this.Progress = 0.0f;
            this.HasSwitched = false;
            this.Tick = 0;
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
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
                    ModConstants.Folder,
                    ModConstants.Saves,
                    $"{ModConstants.PrefixSave}{ModConstants.Basic}{ModConstants.SuffixSav}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataBasic();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root is null)
                    {
                        instance = new DataBasic();
                        return instance;
                    }

                    instance = new DataBasic
                    {
                        State =
                            bool.TryParse(root.Element(ModConstants.SaveState)?.Value, out var boolResult) &&
                            boolResult,
                        Progress =
                            float.TryParse(root.Element(ModConstants.SaveProgress)?.Value, NumberStyles.Float,
                                CultureInfo.InvariantCulture, out var floatResult)
                                ? floatResult
                                : 0.0f,
                        HasSwitched =
                            bool.TryParse(root.Element(ModConstants.SaveHasSwitched)?.Value, out boolResult) &&
                            boolResult,
                        Tick =
                            int.TryParse(root.Element(ModConstants.SaveActivated)?.Value, out var intResult)
                                ? intResult
                                : 0
                    };
                }

                return instance;
            }
        }

        /// <summary>
        ///     Whether the state has switched touching a lever.<br />
        ///     One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }

        /// <inheritdoc />
        public bool State { get; set; }

        /// <inheritdoc />
        public float Progress { get; set; }

        /// <inheritdoc />
        public int Tick { get; set; }

        /// <inheritdoc />
        public bool SwitchOnceSafe => true;

        /// <summary>
        ///     Sets the singleton instance to null.
        /// </summary>
        public static void Reset() => instance = null;

        /// <summary>
        ///     Saves the data to file.
        /// </summary>
        public void SaveToFile()
        {
            var path = Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                ModConstants.Saves);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XDocument(
                new XElement("DataBasic",
                    new XElement(ModConstants.SaveState, this.State),
                    new XElement(ModConstants.SaveProgress, this.Progress),
                    new XElement(ModConstants.SaveHasSwitched, this.HasSwitched),
                    new XElement(ModConstants.SaveActivated, this.Tick)
                )
            );

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSave}{ModConstants.Basic}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
