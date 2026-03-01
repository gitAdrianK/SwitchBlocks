namespace SwitchBlocks.Patches
{
    using System;
    using Behaviours.Dummy;
    using HarmonyLib;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using JumpKing.Player;
    using Util;

    [HarmonyPatch(typeof(ResolveXCollisionBehaviour), nameof(ResolveXCollisionBehaviour.ExecuteBehaviour))]
    public static class PatchResolveXCollisionBehaviour
    {
        /// <summary>FieldRef of the <c>m_collisionQuery</c> field of <see cref="ResolveXCollisionBehaviour" />.</summary>
        private static readonly AccessTools.FieldRef<ResolveXCollisionBehaviour, ICollisionQuery> QueryRef =
            AccessTools.FieldRefAccess<ResolveXCollisionBehaviour, ICollisionQuery>(
                AccessTools.Field(typeof(ResolveXCollisionBehaviour), "m_collisionQuery"));

        // ReSharper disable once InconsistentNaming
        public static void Prefix(ResolveXCollisionBehaviour __instance, BehaviourContext behaviourContext)
        {
            var conveyorBlock = BehaviourConveyor.ConveyorBlock;
            if (!BehaviourConveyor.IsPlayerOnConveyor || conveyorBlock == null)
            {
                return;
            }

            var bodyComp = behaviourContext.BodyComp;
            if (!bodyComp.IsOnGround
                || IsBouncingAgainstConveyorBlock(bodyComp)
                || !QueryRef(__instance).CheckCollision(bodyComp.GetHitbox(), out var overlap,
                    out AdvCollisionInfo advCollisionInfo))
            {
                return;
            }

            if (advCollisionInfo.IsCollidingWith<SlopeBlock>())
            {
                bodyComp.Position.X = BehaviourConveyor.ConveyorPrevPosition.X;
                return;
            }

            // Expansion Blocks does an additional "against wall" check, but it seems to also work without doing so.
            // I wonder what not doing so breaks.
            bodyComp.Position.X -= overlap.Width * Math.Sign(((IConveyor)conveyorBlock).Speed);
        }

        /// <summary>
        ///     Checks if the player is just touching the conveyor and not standing on top if it.
        /// </summary>
        /// <param name="bodyComp"><see cref="BodyComp" /> to check.</param>
        /// <returns><c>true</c> if the player is on the conveyor, <c>false</c> otherwise.</returns>
        private static bool IsBouncingAgainstConveyorBlock(BodyComp bodyComp)
        {
            if (!BehaviourConveyor.WasPlayerOnConveyor)
            {
                return false;
            }

            return !(BehaviourConveyor.ConveyorBlock.GetRect().Location.Y >=
                     bodyComp.Position.Y + bodyComp.GetHitbox().Height);
        }
    }
}
