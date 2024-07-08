using HarmonyLib;
using System;
using System.Reflection;

namespace SwitchBlocks.Patching
{
    public class EndingManager
    {
        public static bool HasFinished { get; private set; }

        public EndingManager()
        {
            Harmony harmony = ModEntry.Harmony;

            Type endingManager = AccessTools.TypeByName("JumpKing.GameManager.MultiEnding.EndingManager");
            MethodInfo checkWin = endingManager.GetMethod("CheckWin");
            HarmonyMethod checkWinPatch = new HarmonyMethod(typeof(EndingManager).GetMethod(nameof(CheckWinPostfix)));
            harmony.Patch(
                checkWin,
                postfix: checkWinPatch);
        }

        public static void CheckWinPostfix(bool __result)
        {
            HasFinished = __result;
        }
    }
}
