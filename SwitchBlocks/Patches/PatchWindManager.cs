// ReSharper disable InvertIf

namespace SwitchBlocks.Patches
{
    using System.Diagnostics.CodeAnalysis;
    using Data;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing;
    using Setups;

    /// <summary>
    ///     Adds a postfix to the vanilla <see cref="WindManager" />.
    /// </summary>
    [HarmonyPatch(typeof(WindManager), "get_CurrentVelocityRaw")]
    public static class PatchWindManager
    {
        /// <summary>
        ///     Flips the sign of the wind velocity if the player is on a screen that has wind enabled if the state
        ///     for that block type is <c>true</c>.
        /// </summary>
        /// <param name="result">Result of the original function.</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        [UsedImplicitly]
        public static void Postfix(ref float result)
        {
            if (SetupAuto.IsUsed
                && SetupAuto.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataAuto.Instance.State)
                {
                    result = -result;
                    return;
                }
            }

            if (SetupBasic.IsUsed
                && SetupBasic.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataBasic.Instance.State)
                {
                    result = -result;
                    return;
                }
            }

            if (SetupCountdown.IsUsed
                && SetupCountdown.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataCountdown.Instance.State)
                {
                    result = -result;
                    return;
                }
            }

            if (SetupJump.IsUsed
                && SetupJump.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataJump.Instance.State)
                {
                    result = -result;
                }
            }
        }
    }
}
