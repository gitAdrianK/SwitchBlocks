// ReSharper disable InconsistentNaming

namespace SwitchBlocks.Patches
{
    using System;
    using System.Reflection;
    using Blocks;
    using Data;
    using ErikMaths;
    using HarmonyLib;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    [HarmonyPatch]
    public static class PatchSlopeBlock
    {
        public static MethodBase TargetMethod() => typeof(SlopeBlock).GetMethod("JumpKing.Level.IBlock.Intersects",
            BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Postfix(SlopeBlock __instance, ref BlockCollisionType __result, Rectangle p_hitbox)
        {
            if (!(__instance is ModSlope slope)
                || slope.CanBlockPlayer
                || !p_hitbox.Intersects(slope.GetRect()))
            {
                return;
            }

            __result = BlockCollisionType.Collision_NonBlocking;

            switch (slope.GetSlopeType())
            {
                case SlopeType.TopLeft:
                case SlopeType.BottomLeft:
                    if (!LineUtil.Intersects(
                            slope.Diagonal,
                            new Line
                            {
                                p0 = new Point(p_hitbox.Right, p_hitbox.Top),
                                p1 = new Point(p_hitbox.Right, p_hitbox.Bottom),
                            },
                            out _))
                    {
                        return;
                    }

                    break;
                case SlopeType.TopRight:
                case SlopeType.BottomRight:
                    if (!LineUtil.Intersects(
                            slope.Diagonal,
                            new Line
                            {
                                p0 = new Point(p_hitbox.Left, p_hitbox.Top),
                                p1 = new Point(p_hitbox.Left, p_hitbox.Bottom),
                            },
                            out _))
                    {
                        return;
                    }

                    break;
                case SlopeType.None:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Quite frankly, I would have expected setting CSS here not to work, it however, does.
            switch (slope)
            {
                case BlockAutoSlopeOn _:
                case BlockAutoSlopeOff _:
                    DataAuto.Instance.CanSwitchSafely = false;
                    break;
                case BlockCountdownSlopeOn _:
                case BlockCountdownSlopeOff _:
                    DataCountdown.Instance.CanSwitchSafely = false;
                    break;
                case BlockJumpSlopeOn _:
                case BlockJumpSlopeOff _:
                    DataJump.Instance.CanSwitchSafely = false;
                    break;
            }
        }
    }
}
