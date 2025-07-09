namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    ///     Contains data relevant for the countdown block.
    /// </summary>
    public class DataCountdown : IDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataCountdown instance;

        /// <summary>
        ///     Private ctor.
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
            this.Touched = new HashSet<int>();
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
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
                    ModConstants.Folder,
                    ModConstants.Saves,
                    $"{ModConstants.PrefixSave}{ModConstants.Countdown}{ModConstants.SuffixSav}");
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
                        CanSwitchSafely =
                            bool.TryParse(root.Element(ModConstants.SaveCss)?.Value, out boolResult) && boolResult,
                        SwitchOnceSafe =
                            bool.TryParse(root.Element(ModConstants.SaveSos)?.Value, out boolResult) && boolResult,
                        WarnCount =
                            int.TryParse(root.Element(ModConstants.SaveWarnCount)?.Value, out var intResult)
                                ? intResult
                                : 0,
                        ActivatedTick =
                            int.TryParse(root.Element(ModConstants.SaveActivated)?.Value, out intResult)
                                ? intResult
                                : 0,
                        Touched = new HashSet<int>(
                            root.Element(ModConstants.SaveTouched)?
                                .Elements(ModConstants.SaveId)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>())
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

        /// <summary>If the block can switch safely.</summary>
        public bool CanSwitchSafely { get; set; }

        /// <summary>If the block should switch next opportunity.</summary>
        public bool SwitchOnceSafe { get; set; }

        /// <summary>The amount of times the warning sound has been played.</summary>
        public int WarnCount { get; set; }

        /// <summary>Tick the countdown block has been activated.</summary>
        public int ActivatedTick { get; set; }

        /// <summary>Single use lever block group IDs that have been touched/activated.</summary>
        public HashSet<int> Touched { get; private set; }

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
                new XElement("DataCountdown",
                    new XElement(ModConstants.SaveState, this.State),
                    new XElement(ModConstants.SaveProgress, this.Progress),
                    new XElement(ModConstants.SaveHasSwitched, this.HasSwitched),
                    new XElement(ModConstants.SaveCss, this.CanSwitchSafely),
                    new XElement(ModConstants.SaveSos, this.SwitchOnceSafe),
                    new XElement(ModConstants.SaveWarnCount, this.WarnCount),
                    new XElement(ModConstants.SaveActivated, this.ActivatedTick),
                    new XElement(ModConstants.SaveTouched,
                        this.Touched.Count != 0
                            ? new List<XElement>(this.Touched.Select(id => new XElement(ModConstants.SaveId, id)))
                            : null)));

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSave}{ModConstants.Countdown}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
