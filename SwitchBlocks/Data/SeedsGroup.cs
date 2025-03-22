namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    /// Contains seed relevant for the group block.
    /// </summary>
    public class SeedsGroup
    {
        /// <summary>
        /// Tries to load seeds from file. Default otherwise.
        /// </summary>
        /// <returns>Seeds.</returns>
        public static SeedsGroup TryDeserialize()
        {
            var contentManagerRoot = Game1.instance.contentManager.root;
            // The new seeds file is called seeds_group.sav
            var file = Path.Combine(
                    contentManagerRoot,
                    ModStrings.FOLDER,
                    ModStrings.SAVES,
                    $"{ModStrings.PREFIX_SEEDS}{ModStrings.GROUP}{ModStrings.SUFFIX_SAV}");
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    return GetNewSeeds(root.Element(ModStrings.SAVE_SEEDS).Elements(ModStrings.SAVE_SEED));
                };
            }
            // The legacy seeds file is called cache_group.sav
            file = Path.Combine(
                    contentManagerRoot,
                    ModStrings.FOLDER,
                    ModStrings.SAVES,
                    $"{ModStrings.PREFIX_CACHE}{ModStrings.GROUP}{ModStrings.SUFFIX_SAV}");
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    return GetLegacySeeds(root.Element(ModStrings.SAVE_SEED).Elements("item"));
                };
            }
            return new SeedsGroup();
        }

        private static SeedsGroup GetNewSeeds(IEnumerable<XElement> xels) => new SeedsGroup
        {
            Seeds = xels.ToDictionary(
                key => int.Parse(key.Element(ModStrings.SAVE_POSITION).Value),
                value => int.Parse(value.Element(ModStrings.SAVE_ID).Value))
        };

        /// <summary>
        /// Gets SeedsGroup from the legacy file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement"/> root of the legacy data format.</param>
        /// <returns>Parsed <see cref="BlockGroup"/>.</returns>
        private static SeedsGroup GetLegacySeeds(IEnumerable<XElement> xels) => new SeedsGroup
        {
            Seeds = xels.ToDictionary(
                key => int.Parse(key.Element("key").Element("int").Value),
                value => int.Parse(value.Element("value").Element("int").Value))
        };

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
                new XElement("SeedsGroup",
                    new XElement(ModStrings.SAVE_SEEDS,
                        this.Seeds.Count() != 0
                        ? this.Seeds.OrderBy(kv => kv.Key).Select(kv =>
                            new XElement(ModStrings.SAVE_SEED,
                                new XElement(ModStrings.SAVE_POSITION, kv.Key),
                                new XElement(ModStrings.SAVE_ID, kv.Value)))
                        : null)));

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModStrings.PREFIX_SEEDS}{ModStrings.GROUP}{ModStrings.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        /// <summary>
        /// Groups belonging to the respective id.
        /// A group has the data related to a platform.
        /// </summary>
        public Dictionary<int, int> Seeds { get; set; }
    }
}
