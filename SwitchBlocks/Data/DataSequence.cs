namespace SwitchBlocks.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using JumpKing.SaveThread;
    using SwitchBlocks.Util;

    /// <summary>
    /// Contains data relevant for the sequence block.
    /// </summary>
    [Serializable, XmlRoot("DataSequence")]
    public class DataSequence : IGroupDataProvider
    {
        private static DataSequence instance;
        public static DataSequence Instance
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
                var file = $"{path}save_{ModStrings.SEQUENCE}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        var xmlSerializer = new XmlSerializer(typeof(DataSequence));
                        instance = (DataSequence)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new DataSequence();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new DataSequence();
                }
                return instance;
            }
        }

        public void Reset() => instance = null;

        private DataSequence()
        {
            this._groups = new SerializableDictionary<int, BlockGroup>();
            this._hasSwitched = false;
            this._touched = 0;
            this._active = new HashSet<int>();
            this._finished = new List<int>();
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
            var xmlSerializer = new XmlSerializer(typeof(DataSequence));
            TextWriter textWriter = new StreamWriter($"{path}save_sequence.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        public bool GetState(int id)
        {
            if (!this._groups.TryGetValue(id, out var group))
            {
                return false;
                //throw new Exception($"Could not get state data for group nr {id}! Did not exist!");
            }
            return group.State;
        }

        public void SetState(int id, bool state)
        {
            if (!this._groups.TryGetValue(id, out var group))
            {
                return;
                //throw new Exception($"Could not set state data for group nr {id}! Did not exist!");
            }
            group.State = state;
        }

        public float GetProgress(int id)
        {
            if (!this._groups.TryGetValue(id, out var group))
            {
                return 0.0f;
                //throw new Exception($"Could not get progress data for group nr {id}! Did not exist!");
            }
            return group.Progress;
        }

        public void SetProgress(int id, float progress)
        {
            if (!this._groups.TryGetValue(id, out var group))
            {
                return;
                //throw new Exception($"Could not set progress data for group nr {id}! Did not exist!");
            }
            group.Progress = progress;
        }

        public int GetTick(int id)
        {
            if (!this._groups.TryGetValue(id, out var group))
            {
                return 0;
                //throw new Exception($"Could not get tick data for group nr {id}! Did not exist!");
            }
            return group.ActivatedTick;
        }

        public void SetTick(int id, int tick)
        {
            if (!this._groups.TryGetValue(id, out var group))
            {
                return;
                //throw new Exception($"Could not set tick data for group nr {id}! Did not exist!");
            }
            group.ActivatedTick = tick;
        }

        /// <summary>
        /// Groups belonging to the respective id.
        /// A group has the data related to a platform.
        /// </summary>
        public SerializableDictionary<int, BlockGroup> Groups
        {
            get => this._groups;
            set => this._groups = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public SerializableDictionary<int, BlockGroup> _groups;

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
        /// Id of the currently touched group.
        /// Since the active groups are always n and n+1 we dont need to keep track of multiple Ids
        /// like the group type.
        /// </summary>
        public int Touched
        {
            get => this._touched;
            set => this._touched = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public int _touched;

        /// <summary>
        /// GroupIds that are currently in the process of changing state from active to inactive or vice versa.
        /// They are considered active until the progress has reached 0/1.
        /// </summary>
        public HashSet<int> Active
        {
            get => this._active;
            set => this._active = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public HashSet<int> _active;

        /// <summary>
        /// GroupIds that have finished going from their default startstate to the other state.
        /// </summary>
        public List<int> Finished
        {
            get => this._finished;
            set => this._finished = value;
        }
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Only used for XML")]
        public List<int> _finished;
    }
}
