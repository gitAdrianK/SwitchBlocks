using JumpKing;
using JumpKing.SaveThread;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the jump block.
    /// </summary>
    public class DataJump
    {

        private static DataJump instance;
        public static DataJump Instance
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
                if (!SaveManager.instance.IsNewGame && File.Exists($"{path}save_jump.sav"))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader($"{path}save_jump.sav");
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataJump));
                        instance = (DataJump)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new DataJump();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new DataJump();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private DataJump()
        {
            _state = false;
            _progress = 0.0f;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataJump));
            TextWriter textWriter = new StreamWriter($"{path}save_jump.sav");
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
        private bool _state;

        /// <summary>
        /// Animation progress.
        /// </summary>
        public static float Progress
        {
            get => Instance._progress;
            set => Instance._progress = value;
        }
        public float _progress;
    }
}