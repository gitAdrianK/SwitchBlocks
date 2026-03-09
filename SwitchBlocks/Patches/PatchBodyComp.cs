namespace SwitchBlocks.Patches
{
    using System;
    using Behaviours.Dummy;
    using Data;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing.Level;
    using JumpKing.Player;

    /// <summary>
    ///     Adds a postfix to the vanilla <see cref="BodyComp" /> and provide an easy way to set
    ///     the "_knocked" field.
    /// </summary>
    [HarmonyPatch(typeof(BodyComp), nameof(BodyComp.IsOnBlock), typeof(Type))]
    public static class PatchBodyComp
    {
        /// <summary>FieldRef of the <c>_knocked</c> field of <see cref="BodyComp" />.</summary>
        private static readonly AccessTools.FieldRef<BodyComp, bool> KnockedRef =
            AccessTools.FieldRefAccess<BodyComp, bool>("_knocked");

        /// <summary>
        ///     The current players <see cref="BodyComp" />.
        ///     This should be set in <see cref="ModEntry.OnLevelStart" /> as entities are created new for every level start.
        /// </summary>
        public static BodyComp BodyComp { get; set; }

        // ReSharper disable InconsistentNaming
        /// <summary>
        ///     Patches the IsOnBlock method of the <see cref="BodyComp" />.
        ///     Adds the custom blocks from this mod to also return <c>true</c>
        ///     in the IsOnBlock function when asked if the <see cref="BodyComp" /> is on a custom block imitating the vanilla
        ///     block's sounds and other functionality tied to it. Does NOT run behaviours if they are based on IsPlayerOnBlock.
        /// </summary>
        /// <param name="__result">Result of the original function, returning <c>true</c> if the player is on a custom block.</param>
        /// <param name="blockType">Original <see cref="Type" /> the function is called with.</param>
        [UsedImplicitly]
        public static void Postfix(ref bool __result, Type blockType)
        {
            if (blockType == typeof(SandBlock))
            {
                if (DataSand.Instance != null)
                {
                    __result |= DataSand.Instance.HasEntered;
                }

                __result |= BehaviourPost.IsPlayerOnTypeSand;
                __result |= BehaviourPost.IsPlayerOnInfinityJump;
            }
            else if (blockType == typeof(IceBlock))
            {
                __result |= BehaviourPost.IsPlayerOnIce;
                __result |= BehaviourPost.IsPlayerOnInfinityJump;
            }
            else if (blockType == typeof(SnowBlock))
            {
                __result |= BehaviourPost.IsPlayerOnSnow;
            }
        }

        /// <summary>
        ///     Sets the <c>_knocked</c> field of the players <see cref="BodyComp" />.
        /// </summary>
        /// <param name="isKnocked">New value to be assigned.</param>
        public static void SetKnocked(bool isKnocked) => KnockedRef(BodyComp) = isKnocked;
    }
}
