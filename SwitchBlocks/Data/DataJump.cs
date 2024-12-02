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
                string file = $"{path}save_{ModStrings.JUMP}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
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
            _canSwitchSafely = true;
            _switchOnceSafe = false;
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

        /// <summary>
        /// If the block can switch safely.
        /// </summary>
        public static bool CanSwitchSafely
        {
            get => Instance._canSwitchSafely;
            set => Instance._canSwitchSafely = value;
        }
        public bool _canSwitchSafely;

        /// <summary>
        /// If the block should switch next opportunity.
        /// </summary>
        public static bool SwitchOnceSafe
        {
            get => Instance._switchOnceSafe;
            set => Instance._switchOnceSafe = value;
        }
        public bool _switchOnceSafe;
    }
}
