using HarmonyLib;
using JumpKing.Level;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Data;
using System;
using System.Reflection;
using JK = JumpKing.Player;

namespace SwitchBlocks.Patching
{
    public class BodyComp
    {
        public BodyComp(Harmony harmony)
        {
            MethodInfo isOnBlock = typeof(JK.BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });

            HarmonyMethod isOnCustomBlock = new HarmonyMethod(typeof(BodyComp).GetMethod(nameof(IsOnCustomBlock)));
            harmony.Patch(
                isOnBlock,
                postfix: isOnCustomBlock);
        }

        /// <summary>
        /// Function to be patched in with harmony, adds the custom blocks from this mod to also return true
        /// in the IsOnBlock function when asked if the player is on a custom block imitating the vanilla blocks behaviour.
        /// </summary>
        /// <param name="__result">Result of the original function, returning true if the player is on a custom block</param>
        /// <param name="blockType">Original object the function is called with</param>
        public static void IsOnCustomBlock(ref bool __result, Type blockType)
        {
            if (blockType == typeof(SandBlock))
            {
                __result |= DataSand.HasEntered;
            }
            if (blockType == typeof(IceBlock))
            {
                __result |= BehaviourPost.IsPlayerOnIce
                    || BehaviourGroupIceA.IsPlayerOnIce
                    || BehaviourGroupIceB.IsPlayerOnIce
                    || BehaviourGroupIceC.IsPlayerOnIce
                    || BehaviourGroupIceD.IsPlayerOnIce
                    || BehaviourSequenceIceA.IsPlayerOnIce
                    || BehaviourSequenceIceB.IsPlayerOnIce
                    || BehaviourSequenceIceC.IsPlayerOnIce
                    || BehaviourSequenceIceD.IsPlayerOnIce;
            }
            if (blockType == typeof(SnowBlock))
            {
                __result |= BehaviourPost.IsPlayerOnSnow
                    || BehaviourGroupSnow.IsPlayerOnSnow
                    || BehaviourSequenceSnow.IsPlayerOnSnow;
            }
        }
    }
}
