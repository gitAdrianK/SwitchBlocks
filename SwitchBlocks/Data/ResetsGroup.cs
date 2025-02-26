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
    /// Contains resets relevant for the group block.
    /// </summary>
    [Serializable, XmlRoot("ResetsGroup")]
    public class ResetsGroup
    {
        public static ResetsGroup TryDeserialize()
        {
            var contentManager = Game1.instance.contentManager;
            var sep = Path.DirectorySeparatorChar;
            var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
            var file = $"{path}resets_{ModStrings.GROUP}.sav";
            if (File.Exists(file))
            {
                StreamReader streamReader = null;
                try
                {
                    streamReader = new StreamReader(file);
                    var xmlSerializer = new XmlSerializer(typeof(ResetsGroup));
                    return (ResetsGroup)xmlSerializer.Deserialize(streamReader);
                }
                catch
                {
                    return new ResetsGroup();
                }
                finally
                {
                    streamReader?.Close();
                    streamReader?.Dispose();
                }
            }
            else
            {
                return new ResetsGroup();
            }
        }

        private ResetsGroup() => this._seed = new SerializableDictionary<int, int[]>();

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
            var xmlSerializer = new XmlSerializer(typeof(ResetsGroup));
            TextWriter textWriter = new StreamWriter($"{path}resets_{ModStrings.GROUP}.sav");
            xmlSerializer.Serialize(textWriter, this);
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public SerializableDictionary<int, int[]> _seed;

        public SerializableDictionary<int, int[]> Seed => this._seed;
    }
}
