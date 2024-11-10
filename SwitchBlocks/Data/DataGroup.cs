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
                Groups.Add(id, new BlockGroup(true));
            }
            return Instance._groups[id].State;
        }

        public static void SetState(int id, bool state)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new BlockGroup(true));
            }
            Instance._groups[id].State = state;
        }

        public static float GetProgress(int id)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new BlockGroup(true));
            }
            return Instance._groups[id].Progress;
        }

        public static void SetProgress(int id, float progress)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new BlockGroup(true));
            }
            Instance._groups[id].Progress = progress;
        }

        public static int GetTick(int id)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new BlockGroup(true));
            }
            return Instance._groups[id].ActivatedTick;
        }

        public static void SetTick(int id, int tick)
        {
            if (!Instance._groups.ContainsKey(id))
            {
                Groups.Add(id, new BlockGroup(true));
            }
            Instance._groups[id].ActivatedTick = tick;
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

        public static HashSet<int> Touched
        {
            get => Instance._touched;
            set => Instance._touched = value;
        }
        public HashSet<int> _touched;
    }
}
