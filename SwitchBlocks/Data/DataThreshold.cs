namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    ///     Contains data relevant for the threshold block.
    /// </summary>
    public class DataThreshold : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataThreshold instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DataThreshold()
        {
            this.State = false;
            this.Progress = 0.0f;
            this.ProgressUnclamped = 0.0f;
            this.CanSwitchSafely = true;
            this.SwitchOnceSafe = false;
            this.ResetTick = 0;
            this.ResetCount = 0;
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
        /// </summary>
        public static DataThreshold Instance
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
                    $"{ModConstants.PrefixSave}{ModConstants.Threshold}{ModConstants.SuffixSav}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataThreshold();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root == null)
                    {
                        instance = new DataThreshold();
                        return instance;
                    }

                    instance = new DataThreshold
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
                        ResetTick = int.TryParse(root.Element(ModConstants.SaveResetTick)?.Value, out var intResult)
                            ? intResult
                            : 0,
                        ResetCount = int.TryParse(root.Element(ModConstants.SaveResetCount)?.Value, out intResult)
                            ? intResult
                            : 0,
                    };
                }

                return instance;
            }
        }

        /// <summary>Stat count the threshold block has been reset at.</summary>
        public int ResetCount { get; set; }

        /// <summary>If the block can switch safely.</summary>
        public bool CanSwitchSafely { get; set; }

        /// <summary>Tick the threshold block has been reset.</summary>
        public int ResetTick { get; set; }

        /// <summary>If the block should switch next opportunity.</summary>
        public bool SwitchOnceSafe { get; set; }

        /// <inheritdoc />
        public bool State { get; set; }

        /// <inheritdoc />
        public float Progress { get; set; }

        /// <inheritdoc />
        public float ProgressUnclamped { get; set; }

        /// <inheritdoc />
        public int Tick => this.ResetTick;

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
                new XElement("DataThreshold",
                    new XElement(ModConstants.SaveState, this.State),
                    new XElement(ModConstants.SaveProgress, this.Progress),
                    new XElement(ModConstants.SaveCss, this.CanSwitchSafely),
                    new XElement(ModConstants.SaveSos, this.SwitchOnceSafe),
                    new XElement(ModConstants.SaveResetTick, this.ResetTick),
                    new XElement(ModConstants.SaveResetCount, this.ResetCount)
                )
            );

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSave}{ModConstants.Threshold}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
