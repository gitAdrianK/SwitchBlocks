namespace SwitchBlocks.Behaviours
{
    using ErikMaths;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using JumpKing.MiscEntities.WorldItems;
    using JumpKing.MiscEntities.WorldItems.Inventory;
    using Microsoft.Xna.Framework;

    public class BehaviourPost : IBlockBehaviour
    {
        // Documentation is false, higher numbers are run first!
        public float BlockPriority => 1.0f;

        public bool IsPlayerOnBlock { get; set; }

        public static bool IsPlayerOnIce { get; set; }
        public static bool IsPlayerOnSnow { get; set; }
        public static Vector2 PrevVelocity { get; set; } = new Vector2(0, 0);

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            var isSnakeringEnabled = InventoryManager.HasItemEnabled(Items.SnakeRing);
            if (IsPlayerOnIce
                && !advCollisionInfo.Ice
                && !isSnakeringEnabled)
            {
                var bodyComp = behaviourContext.BodyComp;
                bodyComp.Velocity.X = ErikMath.MoveTowards(bodyComp.Velocity.X, 0f, PlayerValues.ICE_FRICTION);
            }

            if (isSnakeringEnabled)
            {
                IsPlayerOnSnow = false;
            }

            PrevVelocity = behaviourContext.BodyComp.Velocity;

            return true;
        }
    }
}
