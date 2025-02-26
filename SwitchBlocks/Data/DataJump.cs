namespace SwitchBlocks.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using JumpKing.SaveThread;

    /// <summary>
    /// Contains data relevant for the jump block.
    /// </summary>
    public class DataJump : IDataProvider
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

                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                var file = $"{path}save_{ModStrings.JUMP}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        var xmlSerializer = new XmlSerializer(typeof(DataJump));
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

        public void Reset() => instance = null;

        private DataJump()
        {
            this._state = false;
            this._progress = 0.0f;
            this._canSwitchSafely = true;
            this._switchOnceSafe = false;
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
            var xmlSerializer = new XmlSerializer(typeof(DataJump));
            TextWriter textWriter = new StreamWriter($"{path}save_jump.sav");
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
    }
}
