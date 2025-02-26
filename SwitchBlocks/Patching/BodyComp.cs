namespace SwitchBlocks.Patching
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;
    using JumpKing.Level;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Data;
    using JK = JumpKing.Player;

    public class BodyComp
    {
        public BodyComp(Harmony harmony)
        {
            var isOnBlock = typeof(JK.BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });

            var isOnCustomBlock = new HarmonyMethod(typeof(BodyComp).GetMethod(nameof(IsOnCustomBlock)));
            _ = harmony.Patch(
                isOnBlock,
                postfix: isOnCustomBlock);
        }

        /// <summary>
        /// Function to be patched in with harmony, adds the custom blocks from this mod to also return true
        /// in the IsOnBlock function when asked if the player is on a custom block imitating the vanilla blocks behaviour.
        /// </summary>
        /// <param name="__result">Result of the original function, returning true if the player is on a custom block</param>
        /// <param name="blockType">Original object the function is called with</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        public static void IsOnCustomBlock(ref bool __result, Type blockType)
        {
            if (blockType == typeof(SandBlock))
            {
                __result |= DataSand.Instance.HasEntered;
            }
            if (blockType == typeof(IceBlock))
            {
                __result |= BehaviourPost.IsPlayerOnIce;
            }
            if (blockType == typeof(SnowBlock))
            {
                __result |= BehaviourPost.IsPlayerOnSnow;
            }
        }
    }
}
