// ReSharper disable InconsistentNaming

namespace SwitchBlocks.Patches
{
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;
    using JetBrains.Annotations;

    /// <summary>
    ///     Adds a postfix to the vanilla JumpKing.GameManager.MultiEnding.EndingManager method "CheckWin".
    /// </summary>
    [HarmonyPatch("JumpKing.GameManager.MultiEnding.EndingManager", "CheckWin")]
    public static class PatchEndingManager
    {
        /// <summary>If the game has finished/ the babe has been reached.</summary>
        public static bool HasFinished { get; private set; }

        /// <summary>
        ///     Sets <see cref="HasFinished" /> to the same result as the CheckWin function.
        /// </summary>
        /// <param name="__result">Result of the original function.</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        [UsedImplicitly]
        public static void Postfix(bool __result) => HasFinished = __result;
    }
}
