using JumpKing;
using SwitchBlocks.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
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

                JKContentManager contentManager = Game1.instance.contentManager;
                char sep = Path.DirectorySeparatorChar;
                string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                string file = $"{path}cache_{ModStrings.GROUP}.sav";
                if (File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheGroup));
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

        public void Reset()
        {
            instance = null;
        }

        private CacheGroup()
        {
            _seed = new SerializableDictionary<int, int>();
        }

        public void SaveToFile()
        {
            Dictionary<int, int> sorted = _seed.OrderBy(kv => kv.Key)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            _seed.Clear();
            foreach (KeyValuePair<int, int> kv in sorted)
            {
                _seed.Add(kv.Key, kv.Value);
            }

            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheGroup));
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
        public SerializableDictionary<int, int> _seed;
    }
}
