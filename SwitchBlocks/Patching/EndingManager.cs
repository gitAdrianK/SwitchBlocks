namespace SwitchBlocks.Patching
{
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;

    public class EndingManager
    {
        /// <summary>If the game has finished/ the babe has been reached.</summary>
        public static bool HasFinished { get; private set; }

        /// <summary>
        /// Adds a postfix to the vanilla EndingManager.
        /// </summary>
        /// <param name="harmony"><see cref="Harmony"/> instance used to patch the method.</param>
        public EndingManager(Harmony harmony)
        {
            var endingManager = AccessTools.TypeByName("JumpKing.GameManager.MultiEnding.EndingManager");
            var checkWin = endingManager.GetMethod("CheckWin");
            var checkWinPatch = new HarmonyMethod(typeof(EndingManager).GetMethod(nameof(CheckWinPostfix)));
            _ = harmony.Patch(
                checkWin,
                postfix: checkWinPatch);
        }

        /// <summary>
        /// Sets <see cref="HasFinished"/> to the same result as the CheckWin function.
        /// </summary>
        /// <param name="__result">Result of the original function.</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        public static void CheckWinPostfix(bool __result) => HasFinished = __result;
    }
}
