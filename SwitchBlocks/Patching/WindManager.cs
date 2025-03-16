namespace SwitchBlocks.Patching
{
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;
    using JumpKing.Level;
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;
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
            if (SettingsWind.IsUsed
                && SetupWind.WindEnabled.Contains(LevelManager.CurrentScreen.GetIndex0())
                && DataWind.Instance.State)
            {
                __result = -__result;
            }
        }
    }
}
