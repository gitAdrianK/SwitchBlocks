using JumpKing;
using JumpKing.SaveThread;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the countdown block.
    /// </summary>
    public class DataCountdown
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

                JKContentManager contentManager = Game1.instance.contentManager;
                char sep = Path.DirectorySeparatorChar;
                string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                if (!SaveManager.instance.IsNewGame && File.Exists($"{path}save_countdown.sav"))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader($"{path}save_countdown.sav");
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataCountdown));
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

        public void Reset()
        {
            instance = null;
        }

        private DataCountdown()
        {
            state = false;
            progress = 0.0f;
            hasSwitched = false;
            canSwitchSafely = true;
            switchOnceSafe = false;
            warnCount = 0;
            activatedTick = Int32.MinValue;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataCountdown));
            TextWriter textWriter = new StreamWriter($"{path}save_countdown.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

        // Used to be a static class, I can update all occurences to include instance or 
        // provide gettters like this.

        /// <summary>
        /// Its current state.
        /// </summary>
        public static bool State
        {
            get => Instance.state;
            set => Instance.state = value;
        }
        private bool state;

        /// <summary>
        /// Animation progress.
        /// </summary>
        public static float Progress
        {
            get => Instance.progress;
            set => Instance.progress = value;
        }
        private float progress;

        /// <summary>
        /// Whether the state has switched touching a lever.<br />
        /// One time touching the lever = one switch
        /// </summary>
        public static bool HasSwitched
        {
            get => Instance.hasSwitched;
            set => Instance.hasSwitched = value;
        }
        private bool hasSwitched;

        /// <summary>
        /// If the block can switch safely.
        /// </summary>
        public static bool CanSwitchSafely
        {
            get => Instance.canSwitchSafely;
            set => Instance.canSwitchSafely = value;
        }
        private bool canSwitchSafely;

        /// <summary>
        /// If the block should switch next opportunity.
        /// </summary>
        public static bool SwitchOnceSafe
        {
            get => Instance.switchOnceSafe;
            set => Instance.switchOnceSafe = value;
        }
        private bool switchOnceSafe;

        /// <summary>
        /// The amount of times the warning sound has been played.
        /// </summary>
        public static int WarnCount
        {
            get => Instance.warnCount;
            set => Instance.warnCount = value;
        }
        private int warnCount;

        /// <summary>
        /// Tick the countdown block has been activated.
        /// </summary>
        public static int ActivatedTick
        {
            get => Instance.activatedTick;
            set => Instance.activatedTick = value;
        }
        private int activatedTick;
    }
}