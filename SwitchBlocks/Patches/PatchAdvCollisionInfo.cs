// ReSharper disable InvertIf

// ReSharper disable InconsistentNaming

namespace SwitchBlocks.Patches
{
    using System.Collections.Generic;
    using Blocks;
    using Data;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Level;
    using Setups;

    /// <summary>
    ///     Adds a postfix to the vanilla <see cref="AdvCollisionInfo.Sand" />.
    /// </summary>
    [HarmonyPatch(typeof(AdvCollisionInfo), nameof(AdvCollisionInfo.Sand), MethodType.Getter)]
    public static class PatchAdvCollisionInfo
    {
        /// <summary>FieldRef of the <c>collidedBlocks</c> field of <see cref="AdvCollisionInfo" />.</summary>
        private static readonly AccessTools.FieldRef<AdvCollisionInfo, List<IBlock>> CollidedBlocksRef =
            AccessTools.FieldRefAccess<AdvCollisionInfo, List<IBlock>>(
                AccessTools.Field("JumpKing.Level.AdvCollisionInfo:collidedBlocks"));

        /// <summary>
        ///     Logical ORs the result should the player collide with a sand block of any type,
        ///     if the type and that blocks state would see the block "active".
        /// </summary>
        /// <param name="__instance">The instance the method is called with.</param>
        /// <param name="__result">The original methods result.</param>
        [UsedImplicitly]
        public static void Postfix(AdvCollisionInfo __instance, ref bool __result)
        {
            if (SetupAuto.IsUsed)
            {
                if (DataAuto.Instance.State)
                {
                    __result |= __instance.IsCollidingWith<BlockAutoSandOn>();
                }
                else
                {
                    __result |= __instance.IsCollidingWith<BlockAutoSandOff>();
                }

                return;
            }

            if (SetupBasic.IsUsed)
            {
                if (DataBasic.Instance.State)
                {
                    __result |= __instance.IsCollidingWith<BlockBasicSandOn>();
                }
                else
                {
                    __result |= __instance.IsCollidingWith<BlockBasicSandOff>();
                }

                return;
            }

            if (SetupCountdown.IsUsed)
            {
                if (DataCountdown.Instance.State)
                {
                    __result |= __instance.IsCollidingWith<BlockCountdownSandOn>();
                }
                else
                {
                    __result |= __instance.IsCollidingWith<BlockCountdownSandOff>();
                }

                return;
            }

            if (SetupJump.IsUsed)
            {
                if (DataJump.Instance.State)
                {
                    __result |= __instance.IsCollidingWith<BlockJumpSandOn>();
                }
                else
                {
                    __result |= __instance.IsCollidingWith<BlockJumpSandOff>();
                }
            }
        }
    }
}
