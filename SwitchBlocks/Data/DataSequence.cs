using JumpKing;
using JumpKing.SaveThread;
using SwitchBlocks.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the sequence block.
    /// </summary>
    [Serializable, XmlRoot("DataSequence")]
    public class DataSequence
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

                JKContentManager contentManager = Game1.instance.contentManager;
                char sep = Path.DirectorySeparatorChar;
                string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                string file = $"{path}save_{ModStrings.SEQUENCE}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSequence));
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

        public void Reset()
        {
            instance = null;
        }

        private DataSequence()
        {
            _groups = new SerializableDictionary<int, BlockGroup>();
            _hasSwitched = false;
            _touched = 0;
            _active = new HashSet<int>();
            _finished = new List<int>();
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSequence));
            TextWriter textWriter = new StreamWriter($"{path}save_sequence.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        public static bool GetState(int id)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                group = new BlockGroup(false);
                Groups.Add(id, group);
            }
            return group.State;
        }

        public static void SetState(int id, bool state)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                group = new BlockGroup(state);
                Groups.Add(id, group);
            }
            group.State = state;
        }

        public static float GetProgress(int id)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                group = new BlockGroup(false);
                Groups.Add(id, group);
            }
            return group.Progress;
        }

        public static void SetProgress(int id, float progress)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                group = new BlockGroup(false);
                Groups.Add(id, group);
            }
            group.Progress = progress;
        }

        public static int GetTick(int id)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                group = new BlockGroup(false);
                Groups.Add(id, group);
            }
            return group.ActivatedTick;
        }

        public static void SetTick(int id, int tick)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                group = new BlockGroup(false);
                Groups.Add(id, group);
            }
            group.ActivatedTick = tick;
        }

        /// <summary>
        /// Groups belonging to the respective id.
        /// A group has the data related to a platform.
        /// </summary>
        public static SerializableDictionary<int, BlockGroup> Groups
        {
            get => Instance._groups;
            set => Instance._groups = value;
        }
        public SerializableDictionary<int, BlockGroup> _groups;

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
        /// Id of the currently touched group.
        /// Since the active groups are always n and n+1 we dont need to keep track of multiple Ids
        /// like the group type.
        /// </summary>
        public static int Touched
        {
            get => Instance._touched;
            set => Instance._touched = value;
        }
        public int _touched;

        /// <summary>
        /// GroupIds that are currently in the process of changing state from active to inactive or vice versa.
        /// They are considered active until the progress has reached 0/1.
        /// </summary>
        public static HashSet<int> Active
        {
            get => Instance._active;
            set => Instance._active = value;
        }
        public HashSet<int> _active;

        /// <summary>
        /// GroupIds that have finished going from their default startstate to the other state.
        /// </summary>
        public static List<int> Finished
        {
            get => Instance._finished;
            set => Instance._finished = value;
        }
        public List<int> _finished;
    }
}
