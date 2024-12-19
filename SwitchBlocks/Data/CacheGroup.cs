namespace SwitchBlocks.Data
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using JumpKing;
    using SwitchBlocks.Util;

    /// <summary>
    /// Contains cache relevant for the group block.
    /// </summary>
    [Serializable, XmlRoot("CacheGroup")]
    public class CacheGroup
    {
        private static CacheGroup instance;
        public static CacheGroup Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                var file = $"{path}cache_{ModStrings.GROUP}.sav";
                if (File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        var xmlSerializer = new XmlSerializer(typeof(CacheGroup));
                        instance = (CacheGroup)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new CacheGroup();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new CacheGroup();
                }
                return instance;
            }
        }

        public void Reset() => instance = null;

        private CacheGroup() => this._seed = new SerializableDictionary<int, int>();

        public void SaveToFile()
        {
            var sorted = this._seed.OrderBy(kv => kv.Key)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            this._seed.Clear();
            foreach (var kv in sorted)
            {
                this._seed.Add(kv.Key, kv.Value);
            }

            var contentManager = Game1.instance.contentManager;
            var sep = Path.DirectorySeparatorChar;
            var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }
            var xmlSerializer = new XmlSerializer(typeof(CacheGroup));
            TextWriter textWriter = new StreamWriter($"{path}cache_{ModStrings.GROUP}.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        /// <summary>
        /// Groups belonging to the respective id.
        /// A group has the data related to a platform.
        /// </summary>
        public static SerializableDictionary<int, int> Seed
        {
            get => Instance._seed;
            private set => Instance._seed = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public SerializableDictionary<int, int> _seed;
    }
}
