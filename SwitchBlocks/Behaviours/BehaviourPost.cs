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

    /// <summary>
    /// Behaviour attached to the post block.
    /// </summary>
    public class BehaviourPost : IBlockBehaviour
    {
        // Documentation is false, higher numbers are run first!
        public float BlockPriority => ModConsts.PRIO_LAST;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }
        /// <summary>If the player is on any ice block.</summary>
        public static bool IsPlayerOnIce { get; set; }
        /// <summary>If the player is on any snow block.</summary>
        public static bool IsPlayerOnSnow { get; set; }
        ///<summary>If the player is on any sand block.</summary>
        public static bool IsPlayerOnSand { get; set; }
        ///<summary>If the player is on any sand block that is currently pushing the player up.</summary>
        public static bool IsPlayerOnSandUp { get; set; }
        /// <summary>The velocity of the previous time this behaviour has run.</summary>
        public static Vector2 PrevVelocity { get; set; } = new Vector2(0, 0);

        /// <inheritdoc/>
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc/>
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc/>
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc/>
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc/>
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc/>
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo == null)
            {
                return true;
            }

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
