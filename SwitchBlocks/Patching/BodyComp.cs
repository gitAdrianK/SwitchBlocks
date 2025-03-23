namespace SwitchBlocks.Patching
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using HarmonyLib;
    using JumpKing.Level;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Data;
    using JK = JumpKing.Player;

    /// <summary>
    /// Adds a postfix to the vanilla <see cref="JK.BodyComp"/>.
    /// </summary>
    public class BodyComp
    {
        /// <summary>
        /// Patches the IsOnBlock method of the <see cref="JK.BodyComp"/>.
        /// </summary>
        /// <param name="harmony"><see cref="Harmony"/> instance used to patch the method.</param>
        public BodyComp(Harmony harmony)
        {
            var isOnBlock = typeof(JK.BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });

            var isOnCustomBlock = new HarmonyMethod(typeof(BodyComp).GetMethod(nameof(IsOnCustomBlock)));
            _ = harmony.Patch(
                isOnBlock,
                postfix: isOnCustomBlock);
        }

        /// <summary>
        /// Function to be patched in with <see cref="Harmony"/>, adds the custom blocks from this mod to also return <c>true</c>
        /// in the IsOnBlock function when asked if the <see cref="JK.BodyComp"/> is on a custom block imitating the vanilla blocks behaviour.
        /// </summary>
        /// <param name="__result">Result of the original function, returning <c>true</c> if the player is on a custom block.</param>
        /// <param name="blockType">Original <see cref="Type"/> the function is called with.</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        public static void IsOnCustomBlock(ref bool __result, Type blockType)
        {
            if (blockType == typeof(SandBlock))
            {
                __result |= DataSand.Instance.HasEntered;
                __result |= BehaviourPost.IsPlayerOnSand;
            }
            else if (blockType == typeof(IceBlock))
            {
                __result |= BehaviourPost.IsPlayerOnIce;
            }
            else if (blockType == typeof(SnowBlock))
            {
                __result |= BehaviourPost.IsPlayerOnSnow;
            }
        }
    }
}
