namespace SwitchBlocks.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the sand block.
    /// </summary>
    public class DataSand : IDataProvider
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

                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                var file = $"{path}save_{ModStrings.SAND}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        var xmlSerializer = new XmlSerializer(typeof(DataSand));
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

        public void Reset() => instance = null;

        private DataSand()
        {
            this._state = false;
            this._hasSwitched = false;
            this._hasEntered = false;
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
            var xmlSerializer = new XmlSerializer(typeof(DataSand));
            TextWriter textWriter = new StreamWriter($"{path}save_sand.sav");
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
        /// Progress is not being saved between play sessions as it is unnecessary.
        /// </summary>
        [XmlIgnore]
        public float Progress { get; set; }

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
        /// Whether the player is currently inside the block.
        /// </summary>
        public bool HasEntered
        {
            get => this._hasEntered;
            set => this._hasEntered = value;
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public bool _hasEntered;
    }
}
