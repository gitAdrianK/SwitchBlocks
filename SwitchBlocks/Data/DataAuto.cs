namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    ///     Contains data relevant for the auto block.
    /// </summary>
    public class DataAuto : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataAuto instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DataAuto()
        {
            this.State = false;
            this.Progress = 0.0f;
            this.CanSwitchSafely = true;
            this.SwitchOnceSafe = false;
            this.WarnCount = 0;
            this.ResetTick = 0;
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
        /// </summary>
        public static DataAuto Instance
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
                    $"{ModConstants.PrefixSave}{ModConstants.Auto}{ModConstants.SuffixSav}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataAuto();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root == null)
                    {
                        instance = new DataAuto();
                        return instance;
                    }

                    instance = new DataAuto
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
                        SwitchOnceSafe =
                            bool.TryParse(root.Element(ModConstants.SaveSos)?.Value, out boolResult) && boolResult,
                        WarnCount =
                            int.TryParse(root.Element(ModConstants.SaveWarnCount)?.Value, out var intResult)
                                ? intResult
                                : 0,
                        ResetTick = int.TryParse(root.Element(ModConstants.SaveResetTick)?.Value, out intResult)
                            ? intResult
                            : 0
                    };
                }

                return instance;
            }
        }

        /// <summary>If the block can switch safely.</summary>
        public bool CanSwitchSafely { get; set; }

        /// <summary>If the block should switch next opportunity.</summary>
        public bool SwitchOnceSafe { get; set; }

        /// <summary>The amount of times the warning sound has been played.</summary>
        public int WarnCount { get; set; }

        /// <summary>Tick the auto block has been reset.</summary>
        public int ResetTick { get; set; }

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
                new XElement("DataAuto",
                    new XElement(ModConstants.SaveState, this.State),
                    new XElement(ModConstants.SaveProgress, this.Progress),
                    new XElement(ModConstants.SaveCss, this.CanSwitchSafely),
                    new XElement(ModConstants.SaveSos, this.SwitchOnceSafe),
                    new XElement(ModConstants.SaveWarnCount, this.WarnCount),
                    new XElement(ModConstants.SaveResetTick, this.ResetTick)
                )
            );

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSave}{ModConstants.Auto}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
