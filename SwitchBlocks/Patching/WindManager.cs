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
