namespace SwitchBlocks.Patching
{
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;

    public class EndingManager
    {
        public static bool HasFinished { get; private set; }

        public EndingManager(Harmony harmony)
        {
            var endingManager = AccessTools.TypeByName("JumpKing.GameManager.MultiEnding.EndingManager");
            var checkWin = endingManager.GetMethod("CheckWin");
            var checkWinPatch = new HarmonyMethod(typeof(EndingManager).GetMethod(nameof(CheckWinPostfix)));
            _ = harmony.Patch(
                checkWin,
                postfix: checkWinPatch);
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        public static void CheckWinPostfix(bool __result) => HasFinished = __result;
    }
}
