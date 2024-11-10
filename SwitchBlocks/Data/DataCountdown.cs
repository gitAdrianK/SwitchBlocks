using JumpKing;
using JumpKing.SaveThread;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the countdown block.
    /// </summary>
    public class DataCountdown
    {
        private static DataCountdown instance;
        public static DataCountdown Instance
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
                string file = $"{path}save_{ModStrings.COUNTDOWN}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataCountdown));
                        instance = (DataCountdown)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new DataCountdown();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new DataCountdown();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private DataCountdown()
        {
            _state = false;
            _progress = 0.0f;
            _hasSwitched = false;
            _canSwitchSafely = true;
            _switchOnceSafe = false;
            _warnCount = 0;
            _activatedTick = Int32.MinValue;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataCountdown));
            TextWriter textWriter = new StreamWriter($"{path}save_countdown.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        // Used to be a static class, I can update all occurences to include instance or 
        // provide gettters like this.

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
        /// Tick the countdown block has been activated.
        /// </summary>
        public static int ActivatedTick
        {
            get => Instance._activatedTick;
            set => Instance._activatedTick = value;
        }
        public int _activatedTick;
    }
}
