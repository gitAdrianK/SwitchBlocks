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
    /// Contains data relevant for the sequence block.
    /// </summary>
    public class DataSequence : IGroupDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataSequence instance;
        /// <summary>
        /// Returns the instance should it already exist.
        /// If it doesn't exist loads it from file.
        /// </summary>
        public static DataSequence Instance
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
                    $"{ModConsts.PREFIX_SAVE}{ModConsts.SEQUENCE}{ModConsts.SUFFIX_SAV}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataSequence();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;

                    Dictionary<int, BlockGroup> groupsDict = null;

                    XElement xel;
                    if ((xel = root.Element(ModConsts.SAVE_GROUPS)) != null)
                    {
                        groupsDict = GetNewDict(xel.Elements(ModConsts.SAVE_GROUP));
                    }
                    else if ((xel = root.Element("Groups")) != null)
                    {
                        groupsDict = GetLegacyDict(xel.Elements("item"));
                    }

                    instance = new DataSequence
                    {
                        Groups = groupsDict ?? new Dictionary<int, BlockGroup>(),
                        HasSwitched = bool.TryParse(root.Element(ModConsts.SAVE_HAS_SWITCHED)?.Value, out var boolResult) && boolResult,
                        Touched = int.TryParse(root.Element(ModConsts.SAVE_TOUCHED)?.Value, out var intResult) ? intResult : 0,
                        Active = new HashSet<int>(
                            root.Element(ModConsts.SAVE_ACTIVE)?
                                .Elements(ModConsts.SAVE_ID)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>()),
                        Finished = new HashSet<int>(
                            root.Element(ModConsts.SAVE_FINISHED)?
                                .Elements(ModConsts.SAVE_ID)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>())
                    };
                };
                return instance;
            }
        }

        /// <summary>
        /// Gets block groups from the new file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement"/> root of the new data format.</param>
        /// <returns>Parsed <see cref="BlockGroup"/>.</returns>
        private static Dictionary<int, BlockGroup> GetNewDict(IEnumerable<XElement> xels)
        {
            try
            {
                return xels.ToDictionary(
                    key => int.Parse(key.Element(ModConsts.SAVE_ID).Value),
                    value => new BlockGroup
                    {
                        State = bool.Parse(value.Element(ModConsts.SAVE_STATE).Value),
                        Progress = float.Parse(value.Element(ModConsts.SAVE_PROGRESS).Value, CultureInfo.InvariantCulture),
                        ActivatedTick = int.Parse(value.Element(ModConsts.SAVE_ACTIVATED).Value),
                    });
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets block groups from the legacy file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement"/> root of the legacy data format.</param>
        /// <returns>Parsed <see cref="BlockGroup"/>.</returns>
        private static Dictionary<int, BlockGroup> GetLegacyDict(IEnumerable<XElement> xels)
        {
            try
            {
                return xels.ToDictionary(
                    key => int.Parse(key.Element("key").Element("int").Value),
                    value => new BlockGroup
                    {
                        State = bool.Parse(value.Element("value").Element("BlockGroup").Element("State").Value),
                        Progress = float.Parse(value.Element("value").Element("BlockGroup").Element("Progress").Value, CultureInfo.InvariantCulture),
                        ActivatedTick = int.Parse(value.Element("value").Element("BlockGroup").Element("ActivatedTick").Value),
                    });
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the singleton instance to null.
        /// </summary>
        public void Reset() => instance = null;

        /// <summary>
        /// Private ctor.
        /// </summary>
        private DataSequence()
        {
            this.Groups = new Dictionary<int, BlockGroup>();
            this.HasSwitched = false;
            this.Touched = 0;
            this.Active = new HashSet<int>();
            this.Finished = new HashSet<int>();
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
                new XElement("DataSequence",
                    new XElement(ModConsts.SAVE_GROUPS,
                        this.Groups.Count() != 0
                        ? this.Groups.Select(kv =>
                            new XElement(ModConsts.SAVE_GROUP,
                                new XElement(ModConsts.SAVE_ID, kv.Key),
                                new XElement(ModConsts.SAVE_STATE, kv.Value.State),
                                new XElement(ModConsts.SAVE_PROGRESS, kv.Value.Progress),
                                new XElement(ModConsts.SAVE_ACTIVATED, kv.Value.ActivatedTick)))
                        : null),
                    new XElement(ModConsts.SAVE_HAS_SWITCHED, this.HasSwitched),
                    new XElement(ModConsts.SAVE_TOUCHED, this.Touched),
                    new XElement(ModConsts.SAVE_ACTIVE,
                        this.Active.Count() != 0
                        ? new List<XElement>(this.Active.Select(id => new XElement(ModConsts.SAVE_ID, id)))
                        : null),
                    new XElement(ModConsts.SAVE_FINISHED,
                        this.Finished.Count() != 0
                        ? new List<XElement>(this.Finished.Select(id => new XElement(ModConsts.SAVE_ID, id)))
                        : null)));

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModConsts.PREFIX_SAVE}{ModConsts.SEQUENCE}{ModConsts.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        /// <inheritdoc/>
        public Dictionary<int, BlockGroup> Groups { get; set; }

        /// <inheritdoc/>
        public HashSet<int> Active { get; set; }

        /// <inheritdoc/>
        public HashSet<int> Finished { get; set; }

        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }

        /// <summary>
        /// Id of the currently touched group.
        /// Since the active groups are always n and n+1 we dont need to keep track of multiple Ids
        /// like the group type.
        /// </summary>
        public int Touched { get; set; }
    }
}
