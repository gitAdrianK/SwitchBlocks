using JumpKing;
using JumpKing.SaveThread;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the sand block.
    /// </summary>
    public class DataSand
    {


        private static DataSand instance;
        public static DataSand Instance
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
                string file = $"{path}save_{ModStrings.SAND}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSand));
                        instance = (DataSand)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new DataSand();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new DataSand();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private DataSand()
        {
            _state = false;
            _hasSwitched = false;
            _hasEntered = false;
        }

        public void SaveToFile()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSand));
            TextWriter textWriter = new StreamWriter($"{path}save_sand.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        /// <summary>
        /// Its current state.
        /// </summary>
        public static bool State
        {
            get => Instance._state;
            set => Instance._state = value;
        }
        public bool _state;

        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public static bool HasSwitched
        {
            get => Instance._hasSwitched;
            set => Instance._hasSwitched = value;
        }
        public bool _hasSwitched;

        /// <summary>
        /// Whether the player is currently inside the block.
        /// </summary>
        public static bool HasEntered
        {
            get => Instance._hasEntered;
            set => Instance._hasEntered = value;
        }
        public bool _hasEntered;
    }
}