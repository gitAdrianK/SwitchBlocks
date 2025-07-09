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
    ///     Contains data relevant for the group block.
    /// </summary>
    public class DataGroup : IGroupDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataGroup instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DataGroup()
        {
            this.Groups = new Dictionary<int, BlockGroup>();
            this.HasSwitched = false;
            this.Touched = new HashSet<int>();
            this.Active = new HashSet<int>();
            this.Finished = new HashSet<int>();
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
        /// </summary>
        public static DataGroup Instance
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
                    $"{ModConstants.PrefixSave}{ModConstants.Group}{ModConstants.SuffixSav}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataGroup();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;

                    Dictionary<int, BlockGroup> groupsDict = null;

                    // Both legacy and new data saves all groups as <_groups>
                    var xel = root?.Element(ModConstants.SaveGroups);
                    // Legacy data then saves a group as <item>, new saves it as <_group>
                    var xels = xel?.Elements(ModConstants.SaveGroup);
                    if (xels != null)
                    {
                        var xElements = xels as XElement[] ?? xels.ToArray();
                        if (xElements.Length != 0)
                        {
                            groupsDict = GetNewDict(xElements);
                        }
                        else if ((xels = xel.Elements("item")).Count() != 0)
                        {
                            groupsDict = GetLegacyDict(xels);
                        }
                    }

                    instance = new DataGroup
                    {
                        Groups = groupsDict ?? new Dictionary<int, BlockGroup>(),
                        HasSwitched =
                            bool.TryParse(root?.Element(ModConstants.SaveHasSwitched)?.Value, out var boolResult) &&
                            boolResult,
                        Touched = new HashSet<int>(
                            root?.Element(ModConstants.SaveTouched)?
                                .Elements(ModConstants.SaveId)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>()),
                        Active = new HashSet<int>(
                            root?.Element(ModConstants.SaveActive)?
                                .Elements(ModConstants.SaveId)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>()),
                        Finished = new HashSet<int>(
                            root?.Element(ModConstants.SaveFinished)?
                                .Elements(ModConstants.SaveId)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>())
                    };
                }

                return instance;
            }
        }

        /// <summary>
        ///     GroupIds of block groups that are currently touched by the player.
        /// </summary>
        public HashSet<int> Touched { get; private set; }

        /// <summary>
        ///     Whether the state has switched touching a lever.<br />
        ///     One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched { get; set; }

        /// <inheritdoc />
        public Dictionary<int, BlockGroup> Groups { get; set; }

        /// <inheritdoc />
        public HashSet<int> Active { get; set; }

        /// <inheritdoc />
        public HashSet<int> Finished { get; set; }

        /// <summary>
        ///     Gets block groups from the new file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement" /> root of the new data format.</param>
        /// <returns>Parsed <see cref="BlockGroup" />.</returns>
        private static Dictionary<int, BlockGroup> GetNewDict(IEnumerable<XElement> xels)
        {
            try
            {
                return xels.ToDictionary(
                    key => int.Parse(key.Element(ModConstants.SaveId).Value),
                    value => new BlockGroup
                    {
                        State = bool.Parse(value.Element(ModConstants.SaveState).Value),
                        Progress = float.Parse(value.Element(ModConstants.SaveProgress).Value,
                            CultureInfo.InvariantCulture),
                        ActivatedTick = int.Parse(value.Element(ModConstants.SaveActivated).Value)
                    });
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Gets block groups from the legacy file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement" /> root of the legacy data format.</param>
        /// <returns>Parsed <see cref="BlockGroup" />.</returns>
        private static Dictionary<int, BlockGroup> GetLegacyDict(IEnumerable<XElement> xels)
        {
            try
            {
                return xels.ToDictionary(
                    key => int.Parse(key.Element("key").Element("int").Value),
                    value => new BlockGroup
                    {
                        State = bool.Parse(value.Element("value").Element("BlockGroup").Element("State").Value),
                        Progress =
                            float.Parse(value.Element("value").Element("BlockGroup").Element("Progress").Value,
                                CultureInfo.InvariantCulture),
                        ActivatedTick = int.Parse(value.Element("value").Element("BlockGroup")
                            .Element("ActivatedTick").Value)
                    });
            }
            catch
            {
                return null;
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
                new XElement("DataGroup",
                    new XElement(ModConstants.SaveGroups,
                        this.Groups.Count != 0
                            ? this.Groups.Select(kv =>
                                new XElement(ModConstants.SaveGroup,
                                    new XElement(ModConstants.SaveId, kv.Key),
                                    new XElement(ModConstants.SaveState, kv.Value.State),
                                    new XElement(ModConstants.SaveProgress, kv.Value.Progress),
                                    new XElement(ModConstants.SaveActivated, kv.Value.ActivatedTick)))
                            : null),
                    new XElement(ModConstants.SaveHasSwitched, this.HasSwitched),
                    new XElement(ModConstants.SaveTouched,
                        this.Touched.Count != 0
                            ? new List<XElement>(this.Touched.Select(id => new XElement(ModConstants.SaveId, id)))
                            : null),
                    new XElement(ModConstants.SaveActive,
                        this.Active.Count != 0
                            ? new List<XElement>(this.Active.Select(id => new XElement(ModConstants.SaveId, id)))
                            : null),
                    new XElement(ModConstants.SaveFinished,
                        this.Finished.Count != 0
                            ? new List<XElement>(this.Finished.Select(id => new XElement(ModConstants.SaveId, id)))
                            : null)));

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSave}{ModConstants.Group}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
