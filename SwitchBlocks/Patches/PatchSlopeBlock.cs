namespace SwitchBlocks.Patches
{
    using System.Reflection;
    using Blocks;
    using Data;
    using HarmonyLib;
    using JumpKing.Level;

    /// <summary>
    ///     Adds a postfix to the vanilla <see cref="SlopeBlock" />.
    /// </summary>
    [HarmonyPatch]
    public static class PatchSlopeBlock
    {
        /// <summary>
        ///     Since the method is implemented on an interface the annotation way of specifying the method did not
        ///     seem to work. We specify the method to patch here.
        /// </summary>
        /// <returns><see cref="MethodBase" /> of the to patch method.</returns>
        public static MethodBase TargetMethod() => typeof(SlopeBlock).GetMethod("JumpKing.Level.IBlock.Intersects",
            BindingFlags.Instance | BindingFlags.NonPublic);

        // ReSharper disable InconsistentNaming
        /// <summary>
        ///     Should the <see cref="SlopeBlock" /> be one of our <see cref="ModSlope" />s, that is currently unable to
        ///     block the player, change the <see cref="BlockCollisionType" /> from blocking to non-blocking
        ///     and set the "can switch safely" state of the correct block type.
        /// </summary>
        /// <param name="__instance"><see cref="SlopeBlock" /> the method was called on.</param>
        /// <param name="__result"><see cref="BlockCollisionType" /> the method returned.</param>
        public static void Postfix(SlopeBlock __instance, ref BlockCollisionType __result)
        {
            if (__result != BlockCollisionType.Collision_Blocking
                || !(__instance is ModSlope slope)
                || slope.CanBlockPlayer)
            {
                return;
            }

            __result = BlockCollisionType.Collision_NonBlocking;

            // Quite frankly, I would have expected setting CSS here not to work, it however, does.
            switch (slope)
            {
                case BlockAutoSlopeOn _:
                case BlockAutoSlopeOff _:
                    DataAuto.Instance.CanSwitchSafely = false;
                    break;
                case BlockCountdownSlopeOn _:
                case BlockCountdownSlopeOff _:
                    DataCountdown.Instance.CanSwitchSafely = false;
                    break;
                case BlockJumpSlopeOn _:
                case BlockJumpSlopeOff _:
                    DataJump.Instance.CanSwitchSafely = false;
                    break;
            }
        }
    }
}
