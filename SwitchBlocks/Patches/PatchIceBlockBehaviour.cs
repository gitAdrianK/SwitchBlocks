// ReSharper disable InconsistentNaming
namespace SwitchBlocks.Patches
{
    using Behaviours;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.BlockBehaviours;

    [HarmonyPatch(typeof(IceBlockBehaviour), "get_IsPlayerOnBlock")]
    public static class PatchIceBlockBehaviour
    {
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
            => __result = BehaviourPost.IsPlayerOnIce;
    }
}
