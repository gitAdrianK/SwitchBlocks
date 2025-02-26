namespace SwitchBlocks.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the auto block.
    /// </summary>
    public class DataAuto : IDataProvider
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

                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                var file = $"{path}save_{ModStrings.AUTO}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        var xmlSerializer = new XmlSerializer(typeof(DataAuto));
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

        public void Reset() => instance = null;

        private DataAuto()
        {
            this._state = false;
            this._progress = 0.0f;
            this._canSwitchSafely = true;
            this._switchOnceSafe = false;
            this._warnCount = 0;
            this._resetTick = 0;
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
            var xmlSerializer = new XmlSerializer(typeof(DataAuto));
            TextWriter textWriter = new StreamWriter($"{path}save_{ModStrings.AUTO}.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

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
        /// Tick the auto block has been reset.
        /// </summary>
        public int ResetTick
        {
            get => this._resetTick;
            set => this._resetTick = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public int _resetTick;
    }
}
