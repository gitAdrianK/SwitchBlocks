namespace SwitchBlocks.Data
{
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the countdown block.
    /// </summary>
    public class DataCountdown : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataCountdown instance;
        /// <summary>
        /// Returns the instance should it already exist.
        /// If it doesn't exist loads it from file.
        /// </summary>
        public static DataCountdown Instance
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
                    $"{ModConsts.PREFIX_SAVE}{ModConsts.COUNTDOWN}{ModConsts.SUFFIX_SAV}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataCountdown();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    instance = new DataCountdown
                    {
                        State = bool.Parse(root.Element(ModConsts.SAVE_STATE).Value),
                        Progress = float.Parse(root.Element(ModConsts.SAVE_PROGRESS).Value, CultureInfo.InvariantCulture),
                        HasSwitched = bool.Parse(root.Element(ModConsts.SAVE_HAS_SWITCHED).Value),
                        CanSwitchSafely = bool.Parse(root.Element(ModConsts.SAVE_CSS).Value),
                        SwitchOnceSafe = bool.Parse(root.Element(ModConsts.SAVE_SOS).Value),
                        WarnCount = int.Parse(root.Element(ModConsts.SAVE_WARN_COUNT).Value),
                        ActivatedTick = int.Parse(root.Element(ModConsts.SAVE_ACTIVATED).Value),
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
        private DataCountdown()
        {
            this.State = false;
            this.Progress = 0.0f;
            this.HasSwitched = false;
            this.CanSwitchSafely = true;
            this.SwitchOnceSafe = false;
            this.WarnCount = 0;
            this.ActivatedTick = int.MinValue;
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
                new XElement("DataCountdown",
                    new XElement(ModConsts.SAVE_STATE, this.State),
                    new XElement(ModConsts.SAVE_PROGRESS, this.Progress),
                    new XElement(ModConsts.SAVE_HAS_SWITCHED, this.HasSwitched),
                    new XElement(ModConsts.SAVE_CSS, this.CanSwitchSafely),
                    new XElement(ModConsts.SAVE_SOS, this.SwitchOnceSafe),
                    new XElement(ModConsts.SAVE_WARN_COUNT, this.WarnCount),
                    new XElement(ModConsts.SAVE_ACTIVATED, this.ActivatedTick)
                )
            );

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModConsts.PREFIX_SAVE}{ModConsts.COUNTDOWN}{ModConsts.SUFFIX_SAV}"),
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
        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }
        /// <summary>If the block can switch safely.</summary>
        public bool CanSwitchSafely { get; set; }
        /// <summary>If the block should switch next opportunity.</summary>
        public bool SwitchOnceSafe { get; set; }
        /// <summary>The amount of times the warning sound has been played.</summary>
        public int WarnCount { get; set; }
        /// <summary>Tick the countdown block has been activated.</summary>
        public int ActivatedTick { get; set; }
    }
}
