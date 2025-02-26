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
    public class DataCountdown : IDataProvider
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
        public bool State
        {
            get => this._state;
            set => this._state = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _state;

        /// <summary>
        /// Animation progress.
        /// </summary>
        public float Progress
        {
            get => this._progress;
            set => this._progress = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public float _progress;

        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public bool HasSwitched
        {
            get => this._hasSwitched;
            set => this._hasSwitched = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _hasSwitched;

        /// <summary>
        /// If the block can switch safely.
        /// </summary>
        public bool CanSwitchSafely
        {
            get => this._canSwitchSafely;
            set => this._canSwitchSafely = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _canSwitchSafely;

        /// <summary>
        /// If the block should switch next opportunity.
        /// </summary>
        public bool SwitchOnceSafe
        {
            get => this._switchOnceSafe;
            set => this._switchOnceSafe = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _switchOnceSafe;

        /// <summary>
        /// The amount of times the warning sound has been played.
        /// </summary>
        public int WarnCount
        {
            get => this._warnCount;
            set => this._warnCount = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public int _warnCount;

        /// <summary>
        /// Tick the countdown block has been activated.
        /// </summary>
        public int ActivatedTick
        {
            get => this._activatedTick;
            set => this._activatedTick = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public int _activatedTick;
    }
}
