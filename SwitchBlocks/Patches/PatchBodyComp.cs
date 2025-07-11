// ReSharper disable InconsistentNaming

namespace SwitchBlocks.Patches
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Behaviours;
    using Data;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Level;
    using JumpKing.Player;

    /// <summary>
    ///     Adds a postfix to the vanilla <see cref="BodyComp" />.
    /// </summary>
    [HarmonyPatch(typeof(BodyComp), "IsOnBlock", typeof(Type))]
    public static class PatchBodyComp
    {
        /// <summary>
        ///     Patches the IsOnBlock method of the <see cref="BodyComp" />, adds the custom blocks from this mod to also return
        ///     <c>true</c>
        ///     in the IsOnBlock function when asked if the <see cref="BodyComp" /> is on a custom block imitating the vanilla
        ///     blocks behaviour.
        /// </summary>
        /// <param name="__result">Result of the original function, returning <c>true</c> if the player is on a custom block.</param>
        /// <param name="blockType">Original <see cref="Type" /> the function is called with.</param>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Harmony naming convention")]
        [UsedImplicitly]
        public static void Postfix(ref bool __result, Type blockType)
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
