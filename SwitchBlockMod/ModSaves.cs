using JumpKing;
using SwitchBlocksMod.Data;
using System;
using System.IO;

namespace SwitchBlocksMod.Util
{
    /// <summary>
    /// Saves and loads relevant data for the mod.
    /// </summary>
    public static class ModSaves
    {
        /// <summary>
        /// Saves the various block states and relevant fields.<br />
        /// Only creates a savefile if the SwitchBlocksMod folder exists.<br />
        /// Saves to "Steam Workshop Path\1061090\MAP ID\SwitchBlocksMod\save"
        /// </summary>
        public static void Save()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}switchBlocksMod{sep}";

            // Level being null means the vanilla map is being started/ended.
            if (contentManager.level == null)
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                return;
            }
            BinaryWriter binaryWriter = null;
            try
            {
                binaryWriter = new BinaryWriter(File.Open($"{path}save", FileMode.Create));
                // Auto
                binaryWriter.Write(DataAuto.State);
                binaryWriter.Write(DataAuto.Progress);
                binaryWriter.Write(DataAuto.RemainingTime);
                binaryWriter.Write(DataAuto.HasBlinkedOnce);
                binaryWriter.Write(DataAuto.HasBlinkedTwice);
                // Basic
                binaryWriter.Write(DataBasic.State);
                binaryWriter.Write(DataBasic.Progress);
                binaryWriter.Write(DataBasic.HasSwitched);
                // Countdown
                binaryWriter.Write(DataCountdown.State);
                binaryWriter.Write(DataCountdown.Progress);
                binaryWriter.Write(DataCountdown.HasSwitched);
                binaryWriter.Write(DataCountdown.RemainingTime);
                binaryWriter.Write(DataCountdown.HasBlinkedOnce);
                binaryWriter.Write(DataCountdown.HasBlinkedTwice);
                // Sand
                binaryWriter.Write(DataSand.State);
                binaryWriter.Write(DataSand.HasSwitched);
                // Jump
                binaryWriter.Write(DataJump.State);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                binaryWriter?.Dispose();
            }
        }

        /// <summary>
        /// Loads the various blocks states and fields, if present.<br />
        /// Loads from "Steam Workshop Path\1061090\MAP ID\SwitchBlocksMod\save"
        /// </summary>
        public static void Load()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}switchBlocksMod{sep}";

            if (contentManager.level == null)
            {
                return;
            }
            if (JumpKing.SaveThread.SaveManager.instance.IsNewGame)
            {
                return;
            }
            if (!File.Exists($"{path}save"))
            {
                return;
            }
            BinaryReader binaryReader = null;
            try
            {
                binaryReader = new BinaryReader(File.Open($"{path}save", FileMode.Open));
                // Auto
                DataAuto.State = binaryReader.ReadBoolean();
                DataAuto.Progress = binaryReader.ReadSingle();
                DataAuto.RemainingTime = binaryReader.ReadSingle();
                DataAuto.HasBlinkedOnce = binaryReader.ReadBoolean();
                DataAuto.HasBlinkedTwice = binaryReader.ReadBoolean();
                // Basic
                DataBasic.State = binaryReader.ReadBoolean();
                DataBasic.Progress = binaryReader.ReadSingle();
                DataBasic.HasSwitched = binaryReader.ReadBoolean();
                // Countdown
                DataCountdown.State = binaryReader.ReadBoolean();
                DataCountdown.Progress = binaryReader.ReadSingle();
                DataCountdown.HasSwitched = binaryReader.ReadBoolean();
                DataCountdown.RemainingTime = binaryReader.ReadSingle();
                DataCountdown.HasBlinkedOnce = binaryReader.ReadBoolean();
                DataCountdown.HasBlinkedTwice = binaryReader.ReadBoolean();
                // Sand
                DataSand.State = binaryReader.ReadBoolean();
                DataSand.HasSwitched = binaryReader.ReadBoolean();

                // 1.0.0 End
                if (binaryReader.PeekChar() == -1)
                {
                    return;
                }

                // Jump
                DataJump.State = binaryReader.ReadBoolean();

                // 1.2.0 End
                if (binaryReader.PeekChar() == -1)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                binaryReader?.Dispose();
            }
        }
    }
}
