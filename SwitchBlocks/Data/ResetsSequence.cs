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
    public class ResetsSequence
    {
        /// <summary>
        ///     Private ctor.
        /// </summary>
        private ResetsSequence() => this.Resets = new Dictionary<int, int[]>();

        /// <summary>
        ///     Mapping of the position and the IDs a reset block is supposed to be able to reset,
        ///     should a single 0 be the only id this block can reset, reset all.
        /// </summary>
        public Dictionary<int, int[]> Resets { get; private set; }

        /// <summary>
        ///     Tries to load resets from file. Default otherwise.
        /// </summary>
        /// <returns>Resets.</returns>
        public static ResetsSequence TryDeserialize()
        {
            var file = Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixResets}{ModConstants.Sequence}{ModConstants.SuffixSav}");
            if (!File.Exists(file))
            {
                return new ResetsSequence();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var xel = doc.Root?.Element(ModConstants.SaveResets);
                if (xel != null)
                {
                    return new ResetsSequence
                    {
                        Resets = xel.Elements(ModConstants.SaveReset).ToDictionary(
                            key => int.TryParse(key.Element(ModConstants.SavePosition)?.Value, out var result)
                                ? result
                                : 0,
                            value => value.Elements(ModConstants.SaveId).Select(id => int.Parse(id.Value)).ToArray()),
                    };
                }
            }

            return new ResetsSequence();
        }

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
                new XElement("ResetsSequence",
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
                           $"{ModConstants.PrefixResets}{ModConstants.Sequence}{ModConstants.SuffixSav}"),
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
                           $"{ModConstants.PrefixResets}{ModConstants.Sequence}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
