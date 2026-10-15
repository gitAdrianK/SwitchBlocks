namespace SwitchBlocks.Patches
{
    using System.Linq;
    using Controls;
    using HarmonyLib;
    using JumpKing.Controller;

    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    public static class PatchControllerManager
    {
        public static bool CanSwitchOnPress { get; set; }

        public static bool IsPressed { get; set; }

        // ReSharper disable once InconsistentNaming
        public static void Postfix(ControllerManager __instance)
        {
            var preferences = ModEntry.Preferences;
            if (preferences == null || !CanSwitchOnPress)
            {
                IsPressed = false;
                return;
            }

            IsPressed = __instance
                .GetMain()
                .GetPad()
                .GetPressedButtons()
                .Any(preferences.KeyBindings[EBinding.Switch].Contains);
        }
    }
}
