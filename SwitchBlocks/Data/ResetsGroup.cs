namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    /// Contains resets relevant for the group block.
    /// </summary>
    public class ResetsGroup
    {
        /// <summary>
        /// Tries to load resets from file. Default otherwise.
        /// </summary>
        /// <returns>Resets.</returns>
        public static ResetsGroup TryDeserialize()
        {
            var file = Path.Combine(
                    Game1.instance.contentManager.root,
                    ModConsts.FOLDER,
                    ModConsts.SAVES,
                    $"{ModConsts.PREFIX_RESETS}{ModConsts.GROUP}{ModConsts.SUFFIX_SAV}");
            if (!File.Exists(file))
            {
                return new ResetsGroup();
            }

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(fs);
                var root = doc.Root;

                XElement xel;
                // Legacy data saves the seed as <_seed><item>, new saves it as <_resets><_reset>
                if ((xel = root.Element(ModConsts.SAVE_RESETS)) != null)
                {
                    return GetNewDict(xel.Elements(ModConsts.SAVE_RESET));
                }
                else if ((xel = root.Element(ModConsts.SAVE_SEED)) != null)
                {
                    return GetLegacyDict(xel.Elements("item"));
                }
            };
            return new ResetsGroup();
        }

        private static ResetsGroup GetNewDict(IEnumerable<XElement> xels) => new ResetsGroup
        {
            Resets = xels.ToDictionary(
                key => int.Parse(key.Element(ModConsts.SAVE_POSITION).Value),
                value => value.Elements(ModConsts.SAVE_ID).Select(id => int.Parse(id.Value)).ToArray())
        };

        /// <summary>
        /// Gets ResetsGroup from the legacy file format.
        /// </summary>
        /// <param name="xels"><see cref="XElement"/> root of the legacy data format.</param>
        /// <returns>Parsed <see cref="BlockGroup"/>.</returns>
        private static ResetsGroup GetLegacyDict(IEnumerable<XElement> xels) => new ResetsGroup
        {
            Resets = xels.ToDictionary(
                key => int.Parse(key.Element("key").Element("int").Value),
                value => value.Element("value").Element("ArrayOfInt").Elements("int").Select(id => int.Parse(id.Value)).ToArray())
        };

        /// <summary>
        /// Saves the data to file.
        /// </summary>
        public void SaveToFile()
        {
            var path = Path.Combine(
                Game1.instance.contentManager.root,
                ModConsts.FOLDER,
                ModConsts.SAVES);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }

            var doc = new XDocument(
                new XElement("ResetsGroup",
                    new XElement(ModConsts.SAVE_RESETS,
                        this.Resets.Count() != 0
                        ? this.Resets.OrderBy(kv => kv.Key).Select(kv =>
                            new XElement(ModConsts.SAVE_RESET,
                                new XElement(ModConsts.SAVE_POSITION, kv.Key),
                                kv.Value.Select(id => new XElement(ModConsts.SAVE_ID, id))))
                        : null)));

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModConsts.PREFIX_RESETS}{ModConsts.GROUP}{ModConsts.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        /// <summary>
        /// Private ctor.
        /// </summary>
        private ResetsGroup() => this.Resets = new Dictionary<int, int[]>();

        /// <summary>
        /// Mapping of the position and the ids a reset block is supposed to be able to reset,
        /// should a single 0 be the only id this block can reset, reset all.
        /// </summary>
        public Dictionary<int, int[]> Resets { get; set; }
    }
}
