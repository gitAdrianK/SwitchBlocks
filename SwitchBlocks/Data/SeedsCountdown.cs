namespace SwitchBlocks.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    ///     Contains seed relevant for the countdown block.
    /// </summary>
    public class SeedsCountdown
    {
        /// <summary>
        ///     Private ctor.
        /// </summary>
        private SeedsCountdown() => this.Seeds = new Dictionary<int, int>();

        /// <summary>
        ///     Mapping of the blocks position and id.
        /// </summary>
        public Dictionary<int, int> Seeds { get; private set; }

        /// <summary>
        ///     Tries to load seeds from file. Default otherwise.
        /// </summary>
        /// <returns>Seeds.</returns>
        public static SeedsCountdown TryDeserialize()
        {
            var contentManagerRoot = Game1.instance.contentManager.root;
            var file = Path.Combine(
                contentManagerRoot,
                ModConstants.Folder,
                ModConstants.Saves,
                $"{ModConstants.PrefixSeeds}{ModConstants.Countdown}{ModConstants.SuffixSav}");
            if (!File.Exists(file))
            {
                return new SeedsCountdown();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var xels = XDocument.Load(fs).Root?.Element(ModConstants.SaveSeeds)?.Elements(ModConstants.SaveSeed);
                if (xels == null)
                {
                    return new SeedsCountdown();
                }

                return new SeedsCountdown
                {
                    Seeds = xels.ToDictionary(
                        key => int.Parse(key.Element(ModConstants.SavePosition)?.Value ??
                                         throw new InvalidOperationException()),
                        value => int.Parse(value.Element(ModConstants.SaveId)?.Value ??
                                           throw new InvalidOperationException())),
                };
            }
        }

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
                new XElement("SeedsCountdown",
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
                           $"{ModConstants.PrefixSeeds}{ModConstants.Countdown}{ModConstants.SuffixSav}"),
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
                           $"{ModConstants.PrefixSeeds}{ModConstants.Countdown}{ModConstants.SuffixSav}"),
                       FileMode.Create,
                       FileAccess.Write,
                       FileShare.None))
            {
                doc.Save(fs);
            }
        }
    }
}
