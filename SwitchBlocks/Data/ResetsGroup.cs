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
        public static ResetsGroup TryDeserialize()
        {
            var file = Path.Combine(
                    Game1.instance.contentManager.root,
                    ModStrings.FOLDER,
                    ModStrings.SAVES,
                    $"{ModStrings.PREFIX_RESETS}{ModStrings.GROUP}{ModStrings.SUFFIX_SAV}");
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
                if ((xel = root.Element(ModStrings.SAVE_RESETS)) != null)
                {
                    return GetNewDict(xel.Elements(ModStrings.SAVE_RESET));
                }
                else if ((xel = root.Element(ModStrings.SAVE_SEED)) != null)
                {
                    return GetLegacyDict(xel.Elements("item"));
                }
            };
            return new ResetsGroup();
        }

        private static ResetsGroup GetNewDict(IEnumerable<XElement> xels) => new ResetsGroup
        {
            Resets = xels.ToDictionary(
                key => int.Parse(key.Element(ModStrings.SAVE_POSITION).Value),
                value => value.Elements(ModStrings.SAVE_ID).Select(id => int.Parse(id.Value)).ToArray())
        };

        private static ResetsGroup GetLegacyDict(IEnumerable<XElement> xels) => new ResetsGroup
        {
            Resets = xels.ToDictionary(
                key => int.Parse(key.Element("key").Element("int").Value),
                value => value.Element("value").Element("ArrayOfInt").Elements("int").Select(id => int.Parse(id.Value)).ToArray())
        };

        private ResetsGroup() => this.Resets = new Dictionary<int, int[]>();

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
                new XElement("ResetsGroup",
                    new XElement(ModStrings.SAVE_RESETS,
                        this.Resets.Any()
                        ? this.Resets.OrderBy(kv => kv.Key).Select(kv =>
                            new XElement(ModStrings.SAVE_RESET,
                                new XElement(ModStrings.SAVE_POSITION, kv.Key),
                                kv.Value.Select(id => new XElement(ModStrings.SAVE_ID, id))))
                        : null)));

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModStrings.PREFIX_RESETS}{ModStrings.GROUP}{ModStrings.SUFFIX_SAV}"),
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                doc.Save(fs);
            }
        }

        public Dictionary<int, int[]> Resets { get; set; }
    }
}
