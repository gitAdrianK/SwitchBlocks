namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the auto block.
    /// </summary>
    public class DataAuto : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataAuto instance;
        /// <summary>
        /// Returns the instance should it already exist.
        /// If it doesn't exist loads it from file.
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
                    ModConsts.FOLDER,
                    ModConsts.SAVES,
                    $"{ModConsts.PREFIX_SAVE}{ModConsts.AUTO}{ModConsts.SUFFIX_SAV}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataAuto();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    instance = new DataAuto
                    {
                        State = bool.TryParse(root.Element(ModConsts.SAVE_STATE)?.Value, out var boolResult) && boolResult,
                        Progress = float.TryParse(root.Element(ModConsts.SAVE_PROGRESS)?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatResult) ? floatResult : 0.0f,
                        CanSwitchSafely = bool.TryParse(root.Element(ModConsts.SAVE_CSS)?.Value, out boolResult) && boolResult,
                        SwitchOnceSafe = bool.TryParse(root.Element(ModConsts.SAVE_SOS)?.Value, out boolResult) && boolResult,
                        WarnCount = int.TryParse(root.Element(ModConsts.SAVE_WARN_COUNT)?.Value, out var intResult) ? intResult : 0,
                        ResetTick = int.TryParse(root.Element(ModConsts.SAVE_RESET_TICK)?.Value, out intResult) ? intResult : 0,
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
        /// Saves the data to file.
        /// </summary>
        public void SaveToFile()
        {
            var path = Path.Combine(
                Game1.instance.contentManager.root,
                ModConsts.FOLDER,
                ModConsts.SAVES);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XDocument(
                new XElement("DataAuto",
                    new XElement(ModConsts.SAVE_STATE, this.State),
                    new XElement(ModConsts.SAVE_PROGRESS, this.Progress),
                    new XElement(ModConsts.SAVE_CSS, this.CanSwitchSafely),
                    new XElement(ModConsts.SAVE_SOS, this.SwitchOnceSafe),
                    new XElement(ModConsts.SAVE_WARN_COUNT, this.WarnCount),
                    new XElement(ModConsts.SAVE_RESET_TICK, this.ResetTick)
                )
            );

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModConsts.PREFIX_SAVE}{ModConsts.AUTO}{ModConsts.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        /// <inheritdoc/>
        public bool State { get; set; }
        /// <inheritdoc/>
        public float Progress { get; set; }
        /// <summary>If the block can switch safely.</summary>
        public bool CanSwitchSafely { get; set; }
        /// <summary>If the block should switch next opportunity.</summary>
        public bool SwitchOnceSafe { get; set; }
        /// <summary>The amount of times the warning sound has been played.</summary>
        public int WarnCount { get; set; }
        /// <summary>Tick the auto block has been reset.</summary>
        public int ResetTick { get; set; }
    }
}
