namespace SwitchBlocks.Patches
{
    using Behaviours.Dummy;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    /// <summary>
    ///     Adds a postfix to the vanilla <see cref="IceBlockBehaviour" />.
    /// </summary>
    [HarmonyPatch(typeof(IceBlockBehaviour), nameof(IceBlockBehaviour.IsPlayerOnBlock), MethodType.Getter)]
    public static class PatchIceBlockBehaviour
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        ///     Logical ORs the result with the bool <see cref="BehaviourPost.IsPlayerOnIce" />.
        /// </summary>
        /// <param name="__result">The original methods result.</param>
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
            => __result |= BehaviourPost.IsPlayerOnIce;
    }
}
