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
            this.Touched = new HashSet<int>();
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
        /// </summary>
        public static DataBasic Instance
        {
            get
            {
                if (instance == null)
                {
                    Initialize(false);
                }

                return instance;
            }
            private set => instance = value;
        }

        /// <summary>
        ///     Whether the state has switched touching a lever.<br />
        ///     One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }

        /// <summary>Single use lever block group IDs that have been touched/activated.</summary>
        public HashSet<int> Touched { get; private set; }

        /// <inheritdoc />
        public bool State { get; set; }

        /// <inheritdoc />
        public float Progress { get; set; }

        /// <inheritdoc />
        public float ProgressUnclamped { get; set; }

        /// <inheritdoc />
        public int Tick { get; set; }

        /// <inheritdoc />
        public bool SwitchOnceSafe => false;

        /// <summary>
        ///     Initializes the save singleton from file.
        ///     If the save is set to carry over it will be ignored if the game is new.
        /// </summary>
        /// <param name="saveCarriesOver">Should the save carry over from previous plays.</param>
        public static void Initialize(bool saveCarriesOver)
        {
            var file = Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixSave}{ModConstants.Basic}{ModConstants.SuffixSav}");
            if ((SaveManager.instance.IsNewGame && !saveCarriesOver) || !File.Exists(file))
            {
                instance = new DataBasic();
                return;
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;
                if (root == null)
                {
                    instance = new DataBasic();
                    return;
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
                            : 0,
                    Touched = new HashSet<int>(
                        root.Element(ModConstants.SaveTouched)?
                            .Elements(ModConstants.SaveId)
                            .Select(id => int.Parse(id.Value))
                        ?? Enumerable.Empty<int>()),
                };
            }
        }

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
                    new XElement(ModConstants.SaveActivated, this.Tick),
                    new XElement(ModConstants.SaveTouched,
                        this.Touched.Count != 0
                            ? new List<XElement>(this.Touched.Select(id => new XElement(ModConstants.SaveId, id)))
                            : null)));

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
