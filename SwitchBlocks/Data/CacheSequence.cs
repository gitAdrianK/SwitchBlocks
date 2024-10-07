using JumpKing;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains cache relevant for the sequence block.
    /// </summary>
    public class CacheSequence
    {
        private static CacheSequence instance;
        public static CacheSequence Instance
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
                if (File.Exists($"{path}cache_sequence.sav"))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader($"{path}cache_sequence.sav");
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheSequence));
                        instance = (CacheSequence)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new CacheSequence();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new CacheSequence();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private CacheSequence()
        {
            _seed = new SerializableDictionary<Vector3, int>();
        }

        public void SaveToFile()
        {
            Dictionary<Vector3, int> sorted = _seed.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            _seed.Clear();
            foreach (KeyValuePair<Vector3, int> kv in sorted)
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheSequence));
            TextWriter textWriter = new StreamWriter($"{path}cache_sequence.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        /// <summary>
        /// Groups belonging to the respective id.
        /// A group has the data related to a platform.
        /// </summary>
        public static SerializableDictionary<Vector3, int> Seed
        {
            get => Instance._seed;
            set => Instance._seed = value;
        }
        public SerializableDictionary<Vector3, int> _seed;
    }
}