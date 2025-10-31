// ReSharper disable InconsistentNaming

namespace SwitchBlocks.Patches
{
    using Behaviours;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    [HarmonyPatch(typeof(WaterBlockBehaviour), nameof(WaterBlockBehaviour.IsPlayerOnBlock), MethodType.Getter)]
    public static class PatchWaterBlockBehaviour
    {
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
            => __result |= BehaviourPost.IsPlayerOnWater;
    }
}
