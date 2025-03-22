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
                    ModStrings.FOLDER,
                    ModStrings.SAVES,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.AUTO}{ModStrings.SUFFIX_SAV}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataAuto();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    // The files are auto generated, we skip null checks etc.,
                    // because it should never fail.
                    instance = new DataAuto
                    {
                        State = bool.Parse(root.Element(ModStrings.SAVE_STATE).Value),
                        Progress = float.Parse(root.Element(ModStrings.SAVE_PROGRESS).Value, CultureInfo.InvariantCulture),
                        CanSwitchSafely = bool.Parse(root.Element(ModStrings.SAVE_CSS).Value),
                        SwitchOnceSafe = bool.Parse(root.Element(ModStrings.SAVE_SOS).Value),
                        WarnCount = int.Parse(root.Element(ModStrings.SAVE_WARN_COUNT).Value),
                        ResetTick = int.Parse(root.Element(ModStrings.SAVE_RESET_TICK).Value),
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
                ModStrings.FOLDER,
                ModStrings.SAVES);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XDocument(
                new XElement("DataAuto",
                    new XElement(ModStrings.SAVE_STATE, this.State),
                    new XElement(ModStrings.SAVE_PROGRESS, this.Progress),
                    new XElement(ModStrings.SAVE_CSS, this.CanSwitchSafely),
                    new XElement(ModStrings.SAVE_SOS, this.SwitchOnceSafe),
                    new XElement(ModStrings.SAVE_WARN_COUNT, this.WarnCount),
                    new XElement(ModStrings.SAVE_RESET_TICK, this.ResetTick)
                )
            );

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.AUTO}{ModStrings.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        /// <summary>Current state.</summary>
        public bool State { get; set; }
        /// <summary>Animation progress.</summary>
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
