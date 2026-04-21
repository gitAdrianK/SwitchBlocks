namespace SwitchBlocks.Patches
{
    using Behaviours.Dummy;
    using HarmonyLib;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;

    [HarmonyPatch(typeof(ResolveYCollisionBehaviour), nameof(ResolveYCollisionBehaviour.ExecuteBehaviour))]
    public static class PatchResolveYCollisionBehaviour
    {
        /// <summary>FieldRef of the <c>m_collisionQuery</c> field of <see cref="ResolveYCollisionBehaviour" />.</summary>
        private static readonly AccessTools.FieldRef<ResolveYCollisionBehaviour, ICollisionQuery> QueryRef =
            AccessTools.FieldRefAccess<ResolveYCollisionBehaviour, ICollisionQuery>("m_collisionQuery");

        // ReSharper disable once InconsistentNaming
        public static void Prefix(ResolveYCollisionBehaviour __instance, BehaviourContext behaviourContext)
        {
            if (!BehaviourPost.IsPlayerOnTypeSandUp)
            {
                return;
            }

            var bodyComp = behaviourContext.BodyComp;
            if (!QueryRef(__instance).CheckCollision(bodyComp.GetHitbox(), out _, out AdvCollisionInfo _))
            {
                return;
            }

            bodyComp.Position.Y += 0.75f;
        }
    }
}
