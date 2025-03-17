namespace SwitchBlocks.Patching
{
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Setups;
    using JK = JumpKing;

    public class WindManager
    {
        public WindManager(Harmony harmony)
        {
            var getCurrentVelocity = typeof(JK.WindManager).GetMethod("get_CurrentVelocityRaw");
            var velocityPatch = new HarmonyMethod(typeof(WindManager).GetMethod(nameof(VelocityPostfix)));
            _ = harmony.Patch(
                getCurrentVelocity,
                postfix: velocityPatch);
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        public static void VelocityPostfix(ref float __result)
        {
            if (SetupAuto.IsUsed
                && SetupAuto.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataAuto.Instance.State)
                {
                    __result = -__result * DataAuto.Instance.Progress;
                }
                else
                {
                    __result *= 1.0f - DataAuto.Instance.Progress;
                }
            }
            if (SetupBasic.IsUsed
                && SetupBasic.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataBasic.Instance.State)
                {
                    __result = -__result * DataBasic.Instance.Progress;
                }
                else
                {
                    __result *= 1.0f - DataBasic.Instance.Progress;
                }
            }
            if (SetupCountdown.IsUsed
                && SetupCountdown.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataCountdown.Instance.State)
                {
                    __result = -__result * DataCountdown.Instance.Progress;
                }
                else
                {
                    __result *= 1.0f - DataCountdown.Instance.Progress;
                }
            }
            if (SetupJump.IsUsed
                && SetupJump.WindEnabled.Contains(Camera.CurrentScreen))
            {
                if (DataJump.Instance.State)
                {
                    __result = -__result * DataJump.Instance.Progress;
                }
                else
                {
                    __result *= 1.0f - DataJump.Instance.Progress;
                }
            }
        }
    }
}
