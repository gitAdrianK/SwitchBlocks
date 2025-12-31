// ReSharper disable InconsistentNaming

namespace SwitchBlocks.Patches
{
    using Behaviours;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    /// <summary>
    ///     Adds a postfix to the vanilla <see cref="WaterBlockBehaviour" />.
    /// </summary>
    [HarmonyPatch(typeof(WaterBlockBehaviour), nameof(WaterBlockBehaviour.IsPlayerOnBlock), MethodType.Getter)]
    public static class PatchWaterBlockBehaviour
    {
        /// <summary>
        ///     Logical ORs the result with the bool <see cref="BehaviourPost.IsPlayerOnWater" />.
        /// </summary>
        /// <param name="__result">The original methods result.</param>
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
            => __result |= BehaviourPost.IsPlayerOnWater;
    }
}
