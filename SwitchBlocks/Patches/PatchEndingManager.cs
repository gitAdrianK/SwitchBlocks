namespace SwitchBlocks.Patches
{
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;

    /// <summary>
    /// Adds a postfix to the vanilla JumpKing.GameManager.MultiEnding.EndingManager method "CheckWin".
    /// </summary>
    [HarmonyPatch("JumpKing.GameManager.MultiEnding.EndingManager", "CheckWin")]
    public class PatchEndingManager
    {
        /// <summary>If the game has finished/ the babe has been reached.</summary>
        public static bool HasFinished { get; private set; }

        /// <summary>
        /// Sets <see cref="HasFinished"/> to the same result as the CheckWin function.
        /// </summary>
        /// <param name="__result">Result of the original function.</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        public static void Postfix(bool __result) => HasFinished = __result;
    }
}
