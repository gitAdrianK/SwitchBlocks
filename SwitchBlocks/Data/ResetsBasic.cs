// ReSharper disable IdentifierTypo

namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    ///     Contains resets relevant for the basic block.
    /// </summary>
    public class ResetsBasic
    {
        /// <summary>
        ///     Private ctor.
        /// </summary>
        private ResetsBasic() => this.Resets = new Dictionary<int, int[]>();

        /// <summary>
        ///     Mapping of the position and the IDs a reset block is supposed to be able to reset,
        ///     should a single 0 be the only id this block can reset, reset all.
        /// </summary>
        public Dictionary<int, int[]> Resets { get; private set; }

        /// <summary>
        ///     Tries to load resets from file. Default otherwise.
        /// </summary>
        /// <returns>Resets.</returns>
        public static ResetsBasic TryDeserialize(string path = null)
        {
            var file = path ?? Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixResets}{ModConstants.Basic}{ModConstants.SuffixSav}");
            if (!File.Exists(file))
            {
                return new ResetsBasic();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;
                if (root == null)
                {
                    return new ResetsBasic();
                }

                var xel = root.Element(ModConstants.SaveResets);
                if (xel != null)
                {
                    return GetNewDict(xel.Elements(ModConstants.SaveReset));
                }
            }

            return new ResetsBasic();
        }

        private static ResetsBasic GetNewDict(IEnumerable<XElement> xels) => new ResetsBasic
        {
            Resets = xels.ToDictionary(
                key => int.TryParse(key.Element(ModConstants.SavePosition)?.Value, out var result) ? result : 0,
                value => value.Elements(ModConstants.SaveId).Select(id => int.Parse(id.Value)).ToArray()),
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
                new XElement("ResetsBasic",
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
                           $"{ModConstants.PrefixResets}{ModConstants.Basic}{ModConstants.SuffixSav}"),
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
                           $"{ModConstants.PrefixResets}{ModConstants.Basic}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
