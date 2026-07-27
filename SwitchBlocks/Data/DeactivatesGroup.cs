// ReSharper disable IdentifierTypo

namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    ///     Contains deactivates relevant for the group block.
    /// </summary>
    public class DeactivatesGroup
    {
        /// <summary>
        ///     Private ctor.
        /// </summary>
        private DeactivatesGroup() => this.Deactivates = new Dictionary<int, int[]>();

        /// <summary>
        ///     Mapping of the position and the IDs a deactivate block is supposed to be able to deactivate,
        ///     should a single 0 be the only id this block can deactivate, deactivate all.
        /// </summary>
        public Dictionary<int, int[]> Deactivates { get; private set; }

        /// <summary>
        ///     Tries to load deactivates from file. Default otherwise.
        /// </summary>
        /// <returns>Deactivates.</returns>
        public static DeactivatesGroup TryDeserialize(string path = null)
        {
            var file = path ?? Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixDeactivates}{ModConstants.Group}{ModConstants.SuffixSav}");
            if (!File.Exists(file))
            {
                return new DeactivatesGroup();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;
                if (root == null)
                {
                    return new DeactivatesGroup();
                }

                XElement xel;
                if ((xel = root.Element(ModConstants.SaveDeactivates)) != null)
                {
                    return new DeactivatesGroup
                    {
                        Deactivates = xel.Elements(ModConstants.SaveDeactivate).ToDictionary(
                            key => int.TryParse(key.Element(ModConstants.SavePosition)?.Value, out var result)
                                ? result
                                : 0,
                            value => value.Elements(ModConstants.SaveId).Select(id => int.Parse(id.Value)).ToArray()),
                    };
                }
            }

            return new DeactivatesGroup();
        }

        /// <summary>
        ///     Saves the data to file. Given there is something to save.
        /// </summary>
        public void SaveToFile()
        {
            if (this.Deactivates.Count == 0)
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
                new XElement("DeactivatesGroup",
                    new XElement(ModConstants.SaveDeactivates,
                        this.Deactivates.Count != 0
                            ? this.Deactivates.OrderBy(kv => kv.Key).Select(kv =>
                                new XElement(ModConstants.SaveDeactivate,
                                    new XElement(ModConstants.SavePosition, kv.Key),
                                    kv.Value.Select(id => new XElement(ModConstants.SaveId, id))))
                            : null)));

            using (var fs = new FileStream(
                       Path.Combine(
                           path,
                           $"{ModConstants.PrefixDeactivates}{ModConstants.Group}{ModConstants.SuffixSav}"),
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
                           $"{ModConstants.PrefixDeactivates}{ModConstants.Group}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
