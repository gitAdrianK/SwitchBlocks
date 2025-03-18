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
        private static DataSequence instance;
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
                    ModStrings.FOLDER,
                    ModStrings.SAVES,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.SEQUENCE}{ModStrings.SUFFIX_SAV}");
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
                    if ((xel = root.Element(ModStrings.SAVE_GROUPS)) != null)
                    {
                        groupsDict = GetNewDict(xel.Elements(ModStrings.SAVE_GROUP));
                    }
                    else if ((xel = root.Element("Groups")) != null)
                    {
                        groupsDict = GetLegacyDict(xel.Elements("item"));
                    }

                    instance = new DataSequence
                    {
                        Groups = groupsDict ?? new Dictionary<int, BlockGroup>(),
                        HasSwitched = bool.Parse(root.Element(ModStrings.SAVE_HAS_SWITCHED).Value),
                        Touched = int.Parse(root.Element(ModStrings.SAVE_TOUCHED).Value),
                        Active = new HashSet<int>(
                            root.Element(ModStrings.SAVE_ACTIVE)?
                                .Elements(ModStrings.SAVE_POSITION)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>()),
                        Finished = new HashSet<int>(
                            root.Element(ModStrings.SAVE_FINISHED)?
                                .Elements(ModStrings.SAVE_POSITION)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>())
                    };
                };
                return instance;
            }
        }

        private static Dictionary<int, BlockGroup> GetNewDict(IEnumerable<XElement> xels)
            => xels.ToDictionary(
                key => int.Parse(key.Element(ModStrings.SAVE_ID).Value),
                value => new BlockGroup
                {
                    State = bool.Parse(value.Element(ModStrings.SAVE_STATE).Value),
                    Progress = float.Parse(value.Element(ModStrings.SAVE_PROGRESS).Value, CultureInfo.InvariantCulture),
                    ActivatedTick = int.Parse(value.Element(ModStrings.SAVE_ACTIVATED).Value),
                });

        private static Dictionary<int, BlockGroup> GetLegacyDict(IEnumerable<XElement> xels)
            => xels.ToDictionary(
                key => int.Parse(key.Element("key").Element("int").Value),
                value => new BlockGroup
                {
                    State = bool.Parse(value.Element("value").Element("BlockGroup").Element("State").Value),
                    Progress = float.Parse(value.Element("value").Element("BlockGroup").Element("Progress").Value, CultureInfo.InvariantCulture),
                    ActivatedTick = int.Parse(value.Element("value").Element("BlockGroup").Element("ActivatedTick").Value),
                });

        public void Reset() => instance = null;

        private DataSequence()
        {
            this.Groups = new Dictionary<int, BlockGroup>();
            this.HasSwitched = false;
            this.Touched = 0;
            this.Active = new HashSet<int>();
            this.Finished = new HashSet<int>();
        }

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
                new XElement("DataSequence",
                    new XElement(ModStrings.SAVE_GROUPS,
                        this.Groups.Any()
                        ? this.Groups.Select(kv =>
                            new XElement(ModStrings.SAVE_GROUP,
                                new XElement(ModStrings.SAVE_ID, kv.Key),
                                new XElement(ModStrings.SAVE_STATE, kv.Value.State),
                                new XElement(ModStrings.SAVE_PROGRESS, kv.Value.Progress),
                                new XElement(ModStrings.SAVE_ACTIVATED, kv.Value.ActivatedTick)))
                        : null),
                    new XElement(ModStrings.SAVE_HAS_SWITCHED, this.HasSwitched),
                    new XElement(ModStrings.SAVE_TOUCHED, this.Touched),
                    new XElement(ModStrings.SAVE_ACTIVE,
                        this.Active.Any()
                        ? new List<XElement>(this.Active.Select(id => new XElement(ModStrings.SAVE_POSITION, id)))
                        : null),
                    new XElement(ModStrings.SAVE_FINISHED,
                        this.Finished.Any()
                        ? new List<XElement>(this.Finished.Select(id => new XElement(ModStrings.SAVE_POSITION, id)))
                        : null)));

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModStrings.PREFIX_SAVE}{ModStrings.SEQUENCE}{ModStrings.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        public bool GetState(int id) => this.Groups.TryGetValue(id, out var group) && group.State;

        public float GetProgress(int id) => this.Groups.TryGetValue(id, out var group) ? group.Progress : 0.0f;

        public void SetTick(int id, int tick)
        {
            if (!this.Groups.TryGetValue(id, out var group))
            {
                return;
                //throw new Exception($"Could not set tick data for group nr {id}! Did not exist!");
            }
            group.ActivatedTick = tick;
        }

        /// <summary>
        /// Groups belonging to the respective id.
        /// A group has the data related to a platform.
        /// </summary>
        public Dictionary<int, BlockGroup> Groups { get; set; }

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

        /// <summary>
        /// GroupIds that are currently in the process of changing state from active to inactive or vice versa.
        /// They are considered active until the progress has reached 0/1.
        /// </summary>
        public HashSet<int> Active { get; set; }

        /// <summary>
        /// GroupIds that have finished going from their default startstate to the other state.
        /// </summary>
        public HashSet<int> Finished { get; set; }
    }
}
