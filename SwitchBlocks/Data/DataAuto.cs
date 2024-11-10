using JumpKing;
using JumpKing.SaveThread;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the auto block.
    /// </summary>
    public class DataAuto
    {

        private static DataAuto instance;
        public static DataAuto Instance
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
                string file = $"{path}save_{ModStrings.AUTO}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataAuto));
                        instance = (DataAuto)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new DataAuto();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new DataAuto();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private DataAuto()
        {
            _state = false;
            _progress = 0.0f;
            _canSwitchSafely = true;
            _switchOnceSafe = false;
            _warnCount = 0;
            _resetTick = 0;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataAuto));
            TextWriter textWriter = new StreamWriter($"{path}save_{ModStrings.AUTO}.sav");
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

        /// <summary>
        /// The amount of times the warning sound has been played.
        /// </summary>
        public static int WarnCount
        {
            get => Instance._warnCount;
            set => Instance._warnCount = value;
        }
        public int _warnCount;

        /// <summary>
        /// Tick the auto block has been reset.
        /// </summary>
        public static int ResetTick
        {
            get => Instance._resetTick;
            set => Instance._resetTick = value;
        }
        public int _resetTick;
    }
}
