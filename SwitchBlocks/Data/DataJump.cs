namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    ///     Contains data relevant for the jump block.
    /// </summary>
    public class DataJump : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataJump instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DataJump()
        {
            this.State = false;
            this.Progress = 0.0f;
            this.CanSwitchSafely = true;
            this.SwitchOnceSafe = false;
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
        /// </summary>
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
                    ModConstants.Folder,
                    ModConstants.Saves,
                    $"{ModConstants.PrefixSave}{ModConstants.Jump}{ModConstants.SuffixSav}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataJump();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root == null)
                    {
                        instance = new DataJump();
                        return instance;
                    }

                    instance = new DataJump
                    {
                        State =
                            bool.TryParse(root.Element(ModConstants.SaveState)?.Value, out var boolResult) &&
                            boolResult,
                        Progress =
                            float.TryParse(root.Element(ModConstants.SaveProgress)?.Value, NumberStyles.Float,
                                CultureInfo.InvariantCulture, out var floatResult)
                                ? floatResult
                                : 0.0f,
                        CanSwitchSafely =
                            bool.TryParse(root.Element(ModConstants.SaveCss)?.Value, out boolResult) && boolResult,
                        SwitchOnceSafe = bool.TryParse(root.Element(ModConstants.SaveSos)?.Value, out boolResult) &&
                                         boolResult
                    };
                }

                return instance;
            }
        }

        /// <summary>If the block can switch safely.</summary>
        public bool CanSwitchSafely { get; set; }

        /// <summary>If the block should switch next opportunity.</summary>
        public bool SwitchOnceSafe { get; set; }

        /// <inheritdoc />
        public bool State { get; set; }

        /// <inheritdoc />
        public float Progress { get; set; }

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
                new XElement("DataJump",
                    new XElement(ModConstants.SaveState, this.State),
                    new XElement(ModConstants.SaveProgress, this.Progress),
                    new XElement(ModConstants.SaveCss, this.CanSwitchSafely),
                    new XElement(ModConstants.SaveSos, this.SwitchOnceSafe)
                )
            );

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSave}{ModConstants.Jump}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
