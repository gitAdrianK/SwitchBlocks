namespace SwitchBlocks.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the basic block.
    /// </summary>
    public class DataBasic : IDataProvider
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

                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                var file = $"{path}save_{ModStrings.BASIC}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        var xmlSerializer = new XmlSerializer(typeof(DataBasic));
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

        public void Reset() => instance = null;

        private DataBasic()
        {
            this._state = false;
            this._progress = 0.0f;
            this._hasSwitched = false;
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
            var xmlSerializer = new XmlSerializer(typeof(DataBasic));
            TextWriter textWriter = new StreamWriter($"{path}save_{ModStrings.BASIC}.sav");
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
    }
}
