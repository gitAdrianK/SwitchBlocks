using JumpKing;
using JumpKing.SaveThread;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Data
{
    /// <summary>
    /// Contains data relevant for the auto block.
    /// </summary>
    public class DataAuto
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

                JKContentManager contentManager = Game1.instance.contentManager;
                char sep = Path.DirectorySeparatorChar;
                string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}saves{sep}";
                if (!SaveManager.instance.IsNewGame && File.Exists($"{path}save_auto.sav"))
                {
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader($"{path}save_auto.sav");
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataAuto));
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

        public void Reset()
        {
            instance = null;
        }

        private DataAuto()
        {
            state = false;
            progress = 0.0f;
            canSwitchSafely = true;
            switchOnceSafe = false;
            warnCount = 0;
            resetTick = 0;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataAuto));
            TextWriter textWriter = new StreamWriter($"{path}save_auto.sav");
            xmlSerializer.Serialize(textWriter, Instance);
        }

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
        /// Tick the auto block has been reset.
        /// </summary>
        public static int ResetTick
        {
            get => Instance.resetTick;
            set => Instance.resetTick = value;
        }
        private int resetTick;
    }
}