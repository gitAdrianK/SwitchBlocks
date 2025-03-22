namespace SwitchBlocks.Patching
{
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Setups;
    using JK = JumpKing;

    /// <summary>
    /// Adds a postfix to the vanilla <see cref="JK.WindManager"/>.
    /// </summary>
    public class WindManager
    {
        /// <summary>
        /// Patches the get_CurrentVelocityRaw method of the <see cref="JK.WindManager"/>
        /// </summary>
        /// <param name="harmony">The <see cref="Harmony"/> instance used to patch the method.</param>
        public WindManager(Harmony harmony)
        {
            var getCurrentVelocity = typeof(JK.WindManager).GetMethod("get_CurrentVelocityRaw");
            var velocityPatch = new HarmonyMethod(typeof(WindManager).GetMethod(nameof(VelocityPostfix)));
            _ = harmony.Patch(
                getCurrentVelocity,
                postfix: velocityPatch);
        }

        /// <summary>
        /// Flips the sign of the wind velocity if the player is on a screen that has wind enabled if the state
        /// for that block type is <c>true</c>.
        /// </summary>
        /// <param name="__result">Result of the original function.</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        public static void VelocityPostfix(ref float __result)
        {
            if (SetupAuto.IsUsed
                && SetupAuto.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataAuto.Instance.State)
                {
                    __result = -__result;
                    return;
                }
            }
            if (SetupBasic.IsUsed
                && SetupBasic.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataBasic.Instance.State)
                {
                    __result = -__result;
                    return;
                }
            }
            if (SetupCountdown.IsUsed
                && SetupCountdown.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataCountdown.Instance.State)
                {
                    __result = -__result;
                    return;
                }
            }
            if (SetupJump.IsUsed
                && SetupJump.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataJump.Instance.State)
                {
                    __result = -__result;
                    return;
                }
            }
        }
    }
}
