namespace SwitchBlocks.Data
{
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    ///     Contains data relevant for the sand block.
    /// </summary>
    public class DataSand : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataSand instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DataSand()
        {
            this.State = false;
            this.HasSwitched = false;
            this.HasEntered = false;
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
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
                    ModConstants.Folder,
                    ModConstants.Saves,
                    $"{ModConstants.PrefixSave}{ModConstants.Sand}{ModConstants.SuffixSav}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataSand();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root is null)
                    {
                        instance = new DataSand();
                        return instance;
                    }

                    instance = new DataSand
                    {
                        State =
                            bool.TryParse(root.Element(ModConstants.SaveState)?.Value, out var boolResult) &&
                            boolResult,
                        HasSwitched =
                            bool.TryParse(root.Element(ModConstants.SaveHasSwitched)?.Value, out boolResult) &&
                            boolResult,
                        HasEntered =
                            bool.TryParse(root.Element(ModConstants.SaveHasEntered)?.Value, out boolResult) &&
                            boolResult
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

        /// <summary>Whether the player is currently inside the block.</summary>
        public bool HasEntered { get; set; }

        /// <inheritdoc />
        public bool State { get; set; }

        /// <summary>Progress is not being saved between play sessions as it is unnecessary.</summary>
        public float Progress { get; set; }

        /// <inheritdoc />
        public int Tick => 0;

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
                new XElement("DataSand",
                    new XElement(ModConstants.SaveState, this.State),
                    new XElement(ModConstants.SaveHasSwitched, this.HasSwitched),
                    new XElement(ModConstants.SaveHasEntered, this.HasEntered)
                )
            );

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSave}{ModConstants.Sand}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
