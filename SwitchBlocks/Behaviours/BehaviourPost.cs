namespace SwitchBlocks.Behaviours
{
    using Blocks;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using JumpKing.MiscEntities.WorldItems;
    using JumpKing.MiscEntities.WorldItems.Inventory;
    using JumpKing.Player;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockPost" />.
    /// </summary>
    public class BehaviourPost : IBlockBehaviour
    {
        /// <summary>If the player is on any ice block.</summary>
        public static bool IsPlayerOnIce { get; set; }

        /// <summary>If the player is on any snow block.</summary>
        public static bool IsPlayerOnSnow { get; set; }

        ///<summary>If the player is on any sand block.</summary>
        public static bool IsPlayerOnSand { get; set; }

        ///<summary>If the player is on any move up block.</summary>
        public static bool IsPlayerOnMoveUp { get; set; }

        ///<summary>If the player is on any sand block that is currently pushing the player up.</summary>
        public static bool IsPlayerOnSandUp { get; set; }

        /// <summary>The velocity of the previous time this behaviour has run.</summary>
        public static Vector2 PrevVelocity { get; private set; } = new Vector2(0, 0);

        // Documentation is false, higher numbers are run first!
        public float BlockPriority => ModConstants.PrioLast;

        /// <inheritdoc />
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc />
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            if (IsPlayerOnMoveUp)
            {
                return behaviourContext.BodyComp.Velocity.Y < 0.0f ? 0.0f : inputGravity;
            }

            return inputGravity;
        }

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc />
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo is null)
            {
                return true;
            }

            if (InventoryManager.HasItemEnabled(Items.SnakeRing))
            {
                IsPlayerOnSnow = false;
            }

            PrevVelocity = behaviourContext.BodyComp.Velocity;

            return true;
        }
    }
}
