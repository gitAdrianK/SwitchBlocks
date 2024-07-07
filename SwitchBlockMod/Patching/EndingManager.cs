using HarmonyLib;
using System;
using System.Reflection;

namespace SwitchBlocksMod.Patching
{
    public class EndingManager
    {
        public static bool HasFinished { get; private set; }

        public EndingManager(Harmony harmony)
        {
            Type endingManager = AccessTools.TypeByName("JumpKing.GameManager.MultiEnding.EndingManager");
            MethodInfo checkWin = endingManager.GetMethod("CheckWin");
            HarmonyMethod checkWinPatch = new HarmonyMethod(typeof(ModEntry).GetMethod(nameof(CheckWinPostfix)));
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
