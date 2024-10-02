using JumpKing;
using JumpKing.SaveThread;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the group block.
    /// </summary>
    public class DataGroup
    {
        [Serializable]
        public class Group
        {
            public bool State { get; set; }
            public float Progress { get; set; }
            public int ActivatedTick { get; set; }

            public Group()
            {
                State = true;
                Progress = 1.0f;
                ActivatedTick = Int32.MaxValue;
            }
        }

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
                if (!SaveManager.instance.IsNewGame && File.Exists($"{path}save_group.sav"))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader($"{path}save_group.sav");
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
            _groups = new SerializableDictionary<int, Group>();
            _hasSwitched = false;
            _touched = new HashSet<int>();
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
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new Group());
            }
            return Instance._groups[id].State;
        }

        public static void SetState(int id, bool state)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new Group());
            }
            Instance._groups[id].State = state;
        }

        public static float GetProgress(int id)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new Group());
            }
            return Instance._groups[id].Progress;
        }

        public static void SetProgress(int id, float progress)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new Group());
            }
            Instance._groups[id].Progress = progress;
        }

        public static int GetTick(int id)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new Group());
            }
            return Instance._groups[id].ActivatedTick;
        }

        public static void SetTick(int id, int tick)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new Group());
            }
            Instance._groups[id].ActivatedTick = tick;
        }

        /// <summary>
        /// Groups belonging to the respective id.
        /// A group has the data related to a platform.
        /// </summary>
        public static SerializableDictionary<int, Group> Groups
        {
            get => Instance._groups;
            set => Instance._groups = value;
        }
        public SerializableDictionary<int, Group> _groups;

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

        public static HashSet<int> Touched
        {
            get => Instance._touched;
            set => Instance._touched = value;
        }
        public HashSet<int> _touched;
    }
}