namespace SwitchBlocks.Patches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using BehaviorTree;
    using HarmonyLib;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;

    /// <summary>
    /// Adds a transpiler to the vanilla <see cref="Walk"/>.
    /// </summary>
    [HarmonyPatch(typeof(Walk), "MyRun")]
    public static class PatchWalk
    {
        /// <summary>
        /// Add instructions to not early return <see cref="BTresult.Failure"/>
        /// should the <see cref="PlayerEntity"/> be on a sand block moving upwards
        /// under the additional condition that the reason for moving up is not
        /// one of the custom sand types that is currently moving the player up.
        /// </summary>
        /// <param name="instructions">Original IL instructions.</param>
        /// <returns>Modified IL instructions adding our additional condition.</returns>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = new List<CodeInstruction>(instructions);

            var insertionIndex = -1;
            var continueFound = false;
            var continueLabel = il.DefineLabel();

            int i;
            // Find the first part, that is where we want to insert out own IL instructions.
            for (i = 0; i < code.Count - 1; i++)
            {
                if (code[i].opcode == OpCodes.Ldc_I4_2 && code[i + 1].opcode == OpCodes.Ret)
                {
                    insertionIndex = i;
                    break;
                }
            }
            // Find the second part, that is where we want to jump to in case of success.
            for (; i < code.Count - 1; i++)
            {
                if (code[i].opcode == OpCodes.Ldarg_0 && code[i + 1].opcode == OpCodes.Call)
                {
                    continueFound = true;
                    code[i].labels.Add(continueLabel);
                    break;
                }
            }

            if (insertionIndex == -1 || !continueFound)
            {
                return code.AsEnumerable();
            }

            var insert = new List<CodeInstruction>
            {
                new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.PropertyGetter(typeof(BehaviourPost), nameof(BehaviourPost.IsPlayerOnSandUp))),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
            };
            code.InsertRange(insertionIndex, insert);

            return code.AsEnumerable();
        }

        // The original IL instructions of the check that returns BTresult.Failure.
        // .
        // .
        // .
        // Check for velocity being under 0.0f.
        // /* 0x0000BE18 02           */ IL_0000: ldarg.0
        // /* 0x0000BE19 28EC020006   */ IL_0001: call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
        // /* 0x0000BE1E 7C9C010004   */ IL_0006: ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
        // /* 0x0000BE23 7B6100000A   */ IL_000B: ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
        // /* 0x0000BE28 2200000000   */ IL_0010: ldc.r4    0.0
        // /* 0x0000BE2D 3419         */ IL_0015: bge.un.s  IL_0030
        //
        // Check for being on the sand block.
        // /* 0x0000BE2F 02           */ IL_0017: ldarg.0
        // /* 0x0000BE30 28EC020006   */ IL_0018: call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
        // /* 0x0000BE35 D0DD010002   */ IL_001D: ldtoken   JumpKing.Level.SandBlock
        // /* 0x0000BE3A 284400000A   */ IL_0022: call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
        // /* 0x0000BE3F 6FB6020006   */ IL_0027: callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
        // /* 0x0000BE44 2C02         */ IL_002C: brfalse.s IL_0030
        //
        // Return.
        // /* 0x0000BE46 18           */ IL_002E: ldc.i4.2
        // /* 0x0000BE47 2A           */ IL_002F: ret
        //
        // Continue with function.
        // /* 0x0000BE48 02           */ IL_0030: ldarg.0
        // /* 0x0000BE49 28ED020006   */ IL_0031: call      instance class JumpKing.Player.InputComponent JumpKing.Player.PlayerNode::get_input()
        // .
        // .
        // .

        // Modified method to not early return should the player be on sand up.
        // .
        // .
        // .
        // OLD: Check for velocity being under 0.0f.
        // /* 0x00000000 02           */ IL_0000: ldarg.0
        // /* 0x00000001 28EC020006   */ IL_0001: call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
        // /* 0x00000006 7C9C010004   */ IL_0006: ldflda    valuetype [MonoGame.Framework]Microsoft.Xna.Framework.Vector2 JumpKing.Player.BodyComp::Velocity
        // /* 0x0000000B 7B????????   */ IL_000B: ldfld     float32 [MonoGame.Framework]Microsoft.Xna.Framework.Vector2::Y
        // /* 0x00000010 2200000000   */ IL_0010: ldc.r4    0.0
        // /* 0x00000015 3420         */ IL_0015: bge.un.s  IL_0037
        //
        // OLD: Check for being on the sand block.
        // /* 0x00000017 02           */ IL_0017: ldarg.0
        // /* 0x00000018 28EC020006   */ IL_0018: call      instance class JumpKing.Player.BodyComp JumpKing.Player.PlayerNode::get_body()
        // /* 0x0000001D D0DD010002   */ IL_001D: ldtoken   JumpKing.Level.SandBlock
        // /* 0x00000022 28????????   */ IL_0022: call      class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
        // /* 0x00000027 6FB6020006   */ IL_0027: callvirt  instance bool JumpKing.Player.BodyComp::IsOnBlock(class [mscorlib]System.Type)
        // /* 0x0000002C 2C09         */ IL_002C: brfalse.s IL_0037
        //
        // NEW: Check for not being on a sand up block
        // /* 0x0000002E 28????????   */ IL_002E: call      bool [SwitchBlocks] SwitchBlocks.Behaviours.BehaviourPost::get_IsPlayerOnSandUp()
        // /* 0x00000033 2C02         */ IL_0033: brfalse.s IL_0037
        //
        // OLD: Return.
        // /* (20,5)-(20,29) main.cs */
        // /* 0x00000035 18           */ IL_0035: ldc.i4.2
        // /* 0x00000036 2A           */ IL_0036: ret
        //
        // OLD: Continue with function.
        // /* (21,4)-(21,55) main.cs */
        // /* 0x00000037 02           */ IL_0037: ldarg.0
        // /* 0x00000038 28ED020006   */ IL_0038: call      instance class JumpKing.Player.InputComponent JumpKing.Player.PlayerNode::get_input()
        // .
        // .
        // .
    }
}
