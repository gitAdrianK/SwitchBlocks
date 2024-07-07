using HarmonyLib;
using JumpKing.Level;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using System;
using System.Reflection;
using JK = JumpKing.Player;

namespace SwitchBlocksMod.Patching
{
    public class BodyComp
    {
        public BodyComp(Harmony harmony)
        {
            MethodInfo isOnBlock = typeof(JK.BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });
            HarmonyMethod isOnCustomSand = new HarmonyMethod(typeof(ModEntry).GetMethod(nameof(IsOnCustomSand)));
            harmony.Patch(
                isOnBlock,
                postfix: isOnCustomSand);
        }

        /// <summary>
        /// Function to be patched in with harmony, adds the custom sand blocks from this mod to also return true
        /// in the IsOnBlock function when asked if the player is on a sand block.
        /// </summary>
        /// <param name="__instance">Object instance of the body comp</param>
        /// <param name="__result">Result of the original function, returning true if the player is on a sand block</param>
        /// <param name="blockType">Original object the function is called with</param>
        public static void IsOnCustomSand(object __instance, ref bool __result, Type blockType)
        {
            if (blockType == typeof(SandBlock))
            {
                __result = __result || DataSand.HasEntered;
            }
            if (blockType == typeof(BlockSandOn) || blockType == typeof(BlockSandOn))
            {
                __result = false;
            }

        }

    }
}
