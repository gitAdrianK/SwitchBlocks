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
    /// Contains data relevant for the group block.
    /// </summary>
    [Serializable, XmlRoot("DataGroup")]
    public class DataGroup
    {
        private static DataGroup instance;
        public static DataGroup Instance
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
                string file = $"{path}save_{ModStrings.GROUP}.sav";
                if (!SaveManager.instance.IsNewGame && File.Exists(file))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(file);
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataGroup));
                        instance = (DataGroup)xmlSerializer.Deserialize(streamReader);
                    }
                    catch
                    {
                        instance = new DataGroup();
                    }
                    finally
                    {
                        streamReader?.Close();
                        streamReader?.Dispose();
                    }
                }
                else
                {
                    instance = new DataGroup();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private DataGroup()
        {
            _groups = new SerializableDictionary<int, BlockGroup>();
            _hasSwitched = false;
            _touched = new HashSet<int>();
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataGroup));
            TextWriter textWriter = new StreamWriter($"{path}save_group.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        public static bool GetState(int id)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                return false;
                //throw new Exception($"Could not get state data for group nr {id}! Did not exist!");
            }
            return group.State;
        }

        public static void SetState(int id, bool state)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                return;
                //throw new Exception($"Could not set tick data for group nr {id}! Did not exist!");
            }
            group.State = state;
        }

        public static float GetProgress(int id)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                return 0.0f;
                //throw new Exception($"Could not get tick data for group nr {id}! Did not exist!");
            }
            return group.Progress;
        }

        public static void SetProgress(int id, float progress)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                return;
                //throw new Exception($"Could not set tick data for group nr {id}! Did not exist!");
            }
            group.Progress = progress;
        }

        public static int GetTick(int id)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
            {
                return 0;
                //throw new Exception($"Could not get tick data for group nr {id}! Did not exist!");
            }
            return group.ActivatedTick;
        }

        public static void SetTick(int id, int tick)
        {
            if (!Instance._groups.TryGetValue(id, out BlockGroup group))
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
        /// GroupIds of block groups that are currently touched by the player.
        /// </summary>
        public static HashSet<int> Touched
        {
            get => Instance._touched;
            set => Instance._touched = value;
        }
        public HashSet<int> _touched;

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
