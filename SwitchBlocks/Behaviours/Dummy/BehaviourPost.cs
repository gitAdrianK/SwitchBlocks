namespace SwitchBlocks.Behaviours.Dummy
{
    using System;
    using Blocks.Dummy;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using JumpKing.MiscEntities.WorldItems;
    using JumpKing.MiscEntities.WorldItems.Inventory;
    using Microsoft.Xna.Framework;
    using Patches;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockPost" />.
    /// </summary>
    public class BehaviourPost : IBlockBehaviour
    {
        // Yes, I realize that it's a bunch of static variables and that I should maybe change it.

        /// <summary>If the player is on any ice block.</summary>
        public static bool IsPlayerOnIce { get; set; }

        /// <summary>If the player is on any snow block.</summary>
        public static bool IsPlayerOnSnow { get; set; }

        /// <summary>If the player is on any water block.</summary>
        public static bool IsPlayerOnWater { get; set; }

        ///<summary>If the player is on any sand type block.</summary>
        public static bool IsPlayerOnTypeSand { get; set; }

        ///<summary>If the player is on any sand block that is currently pushing the player up.</summary>
        public static bool IsPlayerOnTypeSandUp { get; set; }

        ///<summary>If the player is on any move up block.</summary>
        public static bool IsPlayerOnMoveUp { get; set; }

        ///<summary>If the player is on any infinity jump block.</summary>
        public static bool IsPlayerOnInfinityJump { get; set; }

        /// <summary>The velocity of the previous time this behaviour has run.</summary>
        public static Vector2 PrevVelocity { get; private set; } = new Vector2(0, 0);

        // Vanilla sand like behaviour is handled by a patch to the AdvCollisionInfo.Sand.
        // This is because collision doesn't quite work correctly otherwise.

        // Even though it would make more sense for priorities to be reversed, it does not work the other way around!
        /// <inheritdoc />
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
            var bodyComp = behaviourContext.BodyComp;
            if (IsPlayerOnMoveUp)
            {
                return bodyComp.Velocity.Y < 0.0f ? 0.0f : inputGravity;
            }

            if (IsPlayerOnInfinityJump && bodyComp.Velocity.Y > PlayerValues.MAX_FALL - 1.0f)
            {
                return 0.0f;
            }

            return inputGravity;
        }

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            if (IsPlayerOnInfinityJump && bodyComp.Velocity.Y > 0.0f)
            {
                return Math.Min(inputYVelocity * 0.4f, PlayerValues.MAX_FALL - 1.0f);
            }

            return inputYVelocity;
        }

        /// <inheritdoc />
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo == null)
            {
                return true;
            }

            if (InventoryManager.HasItemEnabled(Items.SnakeRing))
            {
                IsPlayerOnSnow = false;
            }

            var bodyComp = behaviourContext.BodyComp;
            if (IsPlayerOnInfinityJump)
            {
                PatchBodyComp.SetKnocked(false);
                Camera.UpdateCamera(bodyComp.Position.ToPoint());
            }

            PrevVelocity = bodyComp.Velocity;

            return true;
        }
    }
}
