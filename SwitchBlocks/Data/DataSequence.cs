// ReSharper disable IdentifierTypo

namespace SwitchBlocks.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    ///     Contains data relevant for the sequence block.
    /// </summary>
    public class DataSequence : IGroupDataProvider
    {
        /// <summary>Singleton instance.</summary>
        private static DataSequence instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DataSequence()
        {
            this.Groups = new Dictionary<int, BlockGroup>();
            this.HasSwitched = false;
            this.Active = new HashSet<int>();
            this.Finished = new HashSet<int>();
        }

        /// <summary>
        ///     Returns the instance should it already exist.
        ///     If it doesn't exist loads it from file.
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
                    ModConstants.Folder,
                    ModConstants.Saves,
                    $"{ModConstants.PrefixSave}{ModConstants.Sequence}{ModConstants.SuffixSav}");
                if (SaveManager.instance.IsNewGame || !File.Exists(file))
                {
                    instance = new DataSequence();
                    return instance;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root is null)
                    {
                        instance = new DataSequence();
                        return instance;
                    }

                    Dictionary<int, BlockGroup> groupsDict = null;

                    XElement xel;
                    if ((xel = root.Element(ModConstants.SaveGroups)) != null)
                    {
                        groupsDict = GetNewDict(xel.Elements(ModConstants.SaveGroup));
                    }
                    else if ((xel = root.Element("Groups")) != null)
                    {
                        groupsDict = GetLegacyDict(xel.Elements("item"));
                    }

                    instance = new DataSequence
                    {
                        Groups = groupsDict ?? new Dictionary<int, BlockGroup>(),
                        HasSwitched =
                            bool.TryParse(root.Element(ModConstants.SaveHasSwitched)?.Value, out var boolResult) &&
                            boolResult,
                        Active = new HashSet<int>(
                            root.Element(ModConstants.SaveActive)?
                                .Elements(ModConstants.SaveId)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>()),
                        Finished = new HashSet<int>(
                            root.Element(ModConstants.SaveFinished)?
                                .Elements(ModConstants.SaveId)
                                .Select(id => int.Parse(id.Value))
                            ?? Enumerable.Empty<int>()),
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

        /// <inheritdoc />
        public Dictionary<int, BlockGroup> Groups { get; private set; }

        /// <inheritdoc />
        public HashSet<int> Active { get; private set; }

        /// <inheritdoc />
        public HashSet<int> Finished { get; private set; }

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
                    key => int.TryParse(key.Element(ModConstants.SaveId)?.Value, out var result) ? result : 0,
                    value => new BlockGroup
                    {
                        State = bool.Parse(value.Element(ModConstants.SaveState)?.Value ?? string.Empty),
                        Progress =
                            float.TryParse(value.Element(ModConstants.SaveProgress)?.Value, NumberStyles.Float,
                                CultureInfo.InvariantCulture, out var fResult)
                                ? fResult
                                : 0,
                        ActivatedTick =
                            int.TryParse(value.Element(ModConstants.SaveActivated)?.Value, out var result2)
                                ? result2
                                : 0,
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
                    key => int.Parse(key.Element("key")?.Element("int")?.Value ?? string.Empty),
                    value => new BlockGroup
                    {
                        State = bool.Parse(value.Element("value")?.Element("BlockGroup")?.Element("State")?.Value ??
                                           throw new InvalidOperationException()),
                        Progress =
                            float.Parse(
                                value.Element("value")?.Element("BlockGroup")?.Element("Progress")?.Value ??
                                throw new InvalidOperationException(),
                                CultureInfo.InvariantCulture),
                        ActivatedTick = int.Parse(value.Element("value")
                            ?.Element("BlockGroup")
                            ?.Element("ActivatedTick")
                            ?.Value ?? throw new InvalidOperationException()),
                    });
            }
            catch (NullReferenceException)
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
                new XElement("DataSequence",
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
                           $"{ModConstants.PrefixSave}{ModConstants.Sequence}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
