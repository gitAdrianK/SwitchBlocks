namespace SwitchBlocks.Patches
{
    using System;
    using System.Collections.Generic;
    using HarmonyLib;
    using JumpKing.Mods;

    public static class PatchModLoader
    {
        /// <summary>Delegate of the method "WriteLoadLog".</summary>
        private static readonly Action<List<string>, bool> DelegateWriteLoadLogs =
            AccessTools.MethodDelegate<Action<List<string>, bool>>("JumpKing.Mods.ModLoader:WriteLoadLogs",
                ModLoader.Instance);

        /// <summary>
        ///     Adds a message to the list of messages should the game be started in debug mode.
        /// </summary>
        /// <param name="message">Message to add.</param>
        public static void AddDebugMessage(string message)
        {
            if (!ModDebug.IsDebug)
            {
                return;
            }

            var instance = ModDebug.Instance;
            if (instance.DebugLogMessages == null)
            {
                instance.DebugLogMessages = new List<string>();
            }

            instance.DebugLogMessages.Add(message);
        }

        /// <summary>
        ///     Writes messages to the ModLoadLog should the game be started in debug mode.
        /// </summary>
        public static void WriteDebugLoadLog()
        {
            if (!ModDebug.IsDebug)
            {
                return;
            }

            var instance = ModDebug.Instance;
            DelegateWriteLoadLogs(instance.DebugLogMessages, false);
            instance.DebugLogMessages.Clear();
            instance.DebugLogMessages = null;
        }
    }
}
