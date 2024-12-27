namespace SwitchBlocks.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using JumpKing.SaveThread;

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

                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                var file = $"{path}save_{ModStrings.COUNTDOWN}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        var xmlSerializer = new XmlSerializer(typeof(DataCountdown));
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

        public void Reset() => instance = null;

        private DataCountdown()
        {
            this._state = false;
            this._progress = 0.0f;
            this._hasSwitched = false;
            this._canSwitchSafely = true;
            this._switchOnceSafe = false;
            this._warnCount = 0;
            this._activatedTick = int.MinValue;
        }

        public void SaveToFile()
        {
            var contentManager = Game1.instance.contentManager;
            var sep = Path.DirectorySeparatorChar;
            var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }
            var xmlSerializer = new XmlSerializer(typeof(DataCountdown));
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
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _state;

        /// <summary>
        /// Animation progress.
        /// </summary>
        public static float Progress
        {
            get => Instance._progress;
            set => Instance._progress = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
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
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _hasSwitched;

        /// <summary>
        /// If the block can switch safely.
        /// </summary>
        public static bool CanSwitchSafely
        {
            get => Instance._canSwitchSafely;
            set => Instance._canSwitchSafely = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _canSwitchSafely;

        /// <summary>
        /// If the block should switch next opportunity.
        /// </summary>
        public static bool SwitchOnceSafe
        {
            get => Instance._switchOnceSafe;
            set => Instance._switchOnceSafe = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _switchOnceSafe;

        /// <summary>
        /// The amount of times the warning sound has been played.
        /// </summary>
        public static int WarnCount
        {
            get => Instance._warnCount;
            set => Instance._warnCount = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public int _warnCount;

        /// <summary>
        /// Tick the countdown block has been activated.
        /// </summary>
        public static int ActivatedTick
        {
            get => Instance._activatedTick;
            set => Instance._activatedTick = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public int _activatedTick;
    }
}
