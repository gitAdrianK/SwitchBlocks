// ReSharper disable IdentifierTypo

namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    ///     Contains seed relevant for the group block.
    /// </summary>
    public class SeedsGroup
    {
        /// <summary>
        ///     Private ctor.
        /// </summary>
        private SeedsGroup() => this.Seeds = new Dictionary<int, int>();

        /// <summary>
        ///     Groups belonging to the respective id.
        ///     A group has the data related to a platform.
        /// </summary>
        public Dictionary<int, int> Seeds { get; private set; }

        /// <summary>
        ///     Tries to load seeds from file. Default otherwise.
        /// </summary>
        /// <returns>Seeds.</returns>
        public static SeedsGroup TryDeserialize()
        {
            var contentManagerRoot = Game1.instance.contentManager.root;
            // The new seeds file is called seeds_group.sav
            var file = Path.Combine(
                contentManagerRoot,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixSeeds}{ModConstants.Group}{ModConstants.SuffixSav}");
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    return GetNewSeeds(root?.Element(ModConstants.SaveSeeds)?.Elements(ModConstants.SaveSeed));
                }
            }

            // The legacy seeds file is called cache_group.sav
            file = Path.Combine(
                contentManagerRoot,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixCache}{ModConstants.Group}{ModConstants.SuffixSav}");
            if (!File.Exists(file))
            {
                return new SeedsGroup();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;
                return GetLegacySeeds(root?.Element(ModConstants.SaveSeed)?.Elements("item"));
            }
        }

        private static SeedsGroup GetNewSeeds(IEnumerable<XElement> xels)
        {
            var xElements = xels as XElement[] ?? xels.ToArray();
            if (xElements.Length == 0)
            {
                return new SeedsGroup();
            }

            return new SeedsGroup
            {
                Seeds = xElements.ToDictionary(
                    key => int.Parse(key.Element(ModConstants.SavePosition)?.Value ?? string.Empty),
                    value => int.Parse(value.Element(ModConstants.SaveId)?.Value ?? string.Empty))
            };
        }

        /// <summary>
        ///     Gets SeedsGroup from the legacy file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement" /> root of the legacy data format.</param>
        /// <returns>Parsed <see cref="BlockGroup" />.</returns>
        private static SeedsGroup GetLegacySeeds(IEnumerable<XElement> xels) => new SeedsGroup
        {
            Seeds = xels.ToDictionary(
                key => int.Parse(key.Element("key")?.Element("int")?.Value ?? string.Empty),
                value => int.Parse(value.Element("value")?.Element("int")?.Value ?? string.Empty))
        };

        /// <summary>
        ///     Saves the data to file. Given there is something to save.
        /// </summary>
        public void SaveToFile()
        {
            if (this.Seeds.Count == 0)
            {
                return;
            }

            var path = Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                ModConstants.Saves);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XDocument(
                new XElement("SeedsGroup",
                    new XElement(ModConstants.SaveSeeds,
                        this.Seeds.Count != 0
                            ? this.Seeds.OrderBy(kv => kv.Key).Select(kv =>
                                new XElement(ModConstants.SaveSeed,
                                    new XElement(ModConstants.SavePosition, kv.Key),
                                    new XElement(ModConstants.SaveId, kv.Value)))
                            : null)));

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixSeeds}{ModConstants.Group}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
