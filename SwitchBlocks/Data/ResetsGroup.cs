// ReSharper disable IdentifierTypo

namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    ///     Contains resets relevant for the group block.
    /// </summary>
    public class ResetsGroup
    {
        /// <summary>
        ///     Private ctor.
        /// </summary>
        private ResetsGroup() => this.Resets = new Dictionary<int, int[]>();

        /// <summary>
        ///     Mapping of the position and the IDs a reset block is supposed to be able to reset,
        ///     should a single 0 be the only id this block can reset, reset all.
        /// </summary>
        public Dictionary<int, int[]> Resets { get; private set; }

        /// <summary>
        ///     Tries to load resets from file. Default otherwise.
        /// </summary>
        /// <returns>Resets.</returns>
        public static ResetsGroup TryDeserialize()
        {
            var file = Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixResets}{ModConstants.Group}{ModConstants.SuffixSav}");
            if (!File.Exists(file))
            {
                return new ResetsGroup();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;
                if (root == null)
                {
                    return new ResetsGroup();
                }

                XElement xel;
                // Legacy data saves the seed as <_seed><item>, new saves it as <_resets><_reset>
                if ((xel = root.Element(ModConstants.SaveResets)) != null)
                {
                    return GetNewDict(xel.Elements(ModConstants.SaveReset));
                }

                if ((xel = root.Element(ModConstants.SaveSeed)) != null)
                {
                    return GetLegacyDict(xel.Elements("item"));
                }
            }

            return new ResetsGroup();
        }

        private static ResetsGroup GetNewDict(IEnumerable<XElement> xels) => new ResetsGroup
        {
            Resets = xels.ToDictionary(
                key => int.TryParse(key.Element(ModConstants.SavePosition)?.Value, out var result) ? result : 0,
                value => value.Elements(ModConstants.SaveId).Select(id => int.Parse(id.Value)).ToArray()),
        };

        /// <summary>
        ///     Gets ResetsGroup from the legacy file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement" /> root of the legacy data format.</param>
        /// <returns>Parsed <see cref="BlockGroup" />.</returns>
        private static ResetsGroup GetLegacyDict(IEnumerable<XElement> xels) => new ResetsGroup
        {
            Resets = xels.ToDictionary(
                key => int.TryParse(key.Element("key")?.Element("int")?.Value, out var result) ? result : 0,
                value => value.Element("value")?.Element("ArrayOfInt")?.Elements("int")
                    .Select(id => int.Parse(id.Value))
                    .ToArray()),
        };

        /// <summary>
        ///     Saves the data to file. Given there is something to save.
        /// </summary>
        public void SaveToFile()
        {
            if (this.Resets.Count == 0)
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
                new XElement("ResetsGroup",
                    new XElement(ModConstants.SaveResets,
                        this.Resets.Count != 0
                            ? this.Resets.OrderBy(kv => kv.Key).Select(kv =>
                                new XElement(ModConstants.SaveReset,
                                    new XElement(ModConstants.SavePosition, kv.Key),
                                    kv.Value.Select(id => new XElement(ModConstants.SaveId, id))))
                            : null)));

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixResets}{ModConstants.Group}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }

            // Additionally, if the WS folder structure can be found, we also save to that folder,
            // that way they should be included in steam uploads.
            var root = new DirectoryInfo(Game1.instance.contentManager.root);
            if (root.Name != "bin" || root.Parent == null)
            {
                return;
            }

            // The switchBlocksMod folder has to exist so we can be double sure.
            path = Path.Combine(root.Parent.FullName, ModConstants.Folder);
            if (!Directory.Exists(path))
            {
                return;
            }

            path = Path.Combine(path, ModConstants.Saves);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixResets}{ModConstants.Group}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
