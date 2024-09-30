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
                if (!SaveManager.instance.IsNewGame && File.Exists($"{path}save_sand.sav"))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader($"{path}save_sand.sav");
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
            state = false;
            hasSwitched = false;
            hasEntered = false;
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
            get => Instance.state;
            set => Instance.state = value;
        }
        private bool state;

        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public static bool HasSwitched
        {
            get => Instance.hasSwitched;
            set => Instance.hasSwitched = value;
        }
        private bool hasSwitched;

        /// <summary>
        /// Whether the player is currently inside the block.
        /// </summary>
        public static bool HasEntered
        {
            get => Instance.hasEntered;
            set => Instance.hasEntered = value;
        }
        private bool hasEntered;
    }
}