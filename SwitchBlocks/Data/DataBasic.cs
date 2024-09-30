using JumpKing;
using JumpKing.SaveThread;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the basic block.
    /// </summary>
    public class DataBasic
    {
        private static DataBasic instance;
        public static DataBasic Instance
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
                if (!SaveManager.instance.IsNewGame && File.Exists($"{path}save_basic.sav"))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader($"{path}save_basic.sav");
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataBasic));
                        instance = (DataBasic)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new DataBasic();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {

                    instance = new DataBasic();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private DataBasic()
        {
            state = false;
            progress = 0.0f;
            hasSwitched = false;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataBasic));
            TextWriter textWriter = new StreamWriter($"{path}save_basic.sav");
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
        /// Animation progress.
        /// </summary>
        public static float Progress
        {
            get => Instance.progress;
            set => Instance.progress = value;
        }
        private float progress;

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
    }
}