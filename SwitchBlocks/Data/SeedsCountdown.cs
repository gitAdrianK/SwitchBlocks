namespace SwitchBlocks.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using JumpKing;

    /// <summary>
    /// Contains seed relevant for the countdown block.
    /// </summary>
    public class SeedsCountdown
    {
        /// <summary>
        /// Tries to load seeds from file. Default otherwise.
        /// </summary>
        /// <returns>Seeds.</returns>
        public static SeedsCountdown TryDeserialize()
        {
            var contentManagerRoot = Game1.instance.contentManager.root;
            var file = Path.Combine(
                    contentManagerRoot,
                    ModConsts.FOLDER,
                    ModConsts.SAVES,
                    $"{ModConsts.PREFIX_SEEDS}{ModConsts.COUNTDOWN}{ModConsts.SUFFIX_SAV}");
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    var xels = root.Element(ModConsts.SAVE_SEEDS).Elements(ModConsts.SAVE_SEED);
                    return new SeedsCountdown
                    {
                        Seeds = xels.ToDictionary(
                            key => int.Parse(key.Element(ModConsts.SAVE_POSITION).Value),
                            value => int.Parse(value.Element(ModConsts.SAVE_ID).Value))
                    };
                };
            }
            return new SeedsCountdown();
        }

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
                new XElement("SeedsCountdown",
                    new XElement(ModConsts.SAVE_SEEDS,
                        this.Seeds.Count() != 0
                        ? this.Seeds.OrderBy(kv => kv.Key).Select(kv =>
                            new XElement(ModConsts.SAVE_SEED,
                                new XElement(ModConsts.SAVE_POSITION, kv.Key),
                                new XElement(ModConsts.SAVE_ID, kv.Value)))
                        : null)));

            using (var fs = new FileStream(
                Path.Combine(
                    path,
                    $"{ModConsts.PREFIX_SEEDS}{ModConsts.COUNTDOWN}{ModConsts.SUFFIX_SAV}"),
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
        private SeedsCountdown() => this.Seeds = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of the blocks position and id.
        /// </summary>
        public Dictionary<int, int> Seeds { get; set; }
    }
}
